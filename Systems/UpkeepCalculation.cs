using System;
using System.Text;
using Colossal.Entities;
using Game;
using Game.Prefabs;
using StarQ.Shared.Extensions;
using Unity.Entities;

namespace SmartUpkeepManager.Systems
{
    public partial class SmartUpkeepSystem : GameSystemBase
    {
        public void CalculateUpkeep()
        {
            EntityQuery serviceBudgetQuery = SystemAPI
                .QueryBuilder()
                .WithAll<Game.Simulation.ServiceBudgetData>()
                .Build();

            if (serviceBudgetQuery.IsEmpty)
                return;

            Entity serviceBudgetEntity = serviceBudgetQuery.GetSingletonEntity();
            if (
                !EntityManager.TryGetBuffer(
                    serviceBudgetEntity,
                    true,
                    out DynamicBuffer<Game.Simulation.ServiceBudgetData> serviceBudgetData
                )
            )
                return;

            StringBuilder sb = new();
            sb.AppendLine("");

            foreach (var item in buildingDict)
            {
                prevVal.Clear();
                try
                {
                    string name = item.Key;
                    Entity entity = item.Value;
                    float upkeepValue = 0f;
                    float prevUpkeepValue = 0f;
                    float multiplier = 1f;
                    float prevMultiplierValue = 0f;

                    if (
                        EntityManager.TryGetComponent(entity, out PrefabData prefabData)
                        && prefabSystem.TryGetPrefab(entity, out PrefabBase prefabBase)
                    )
                    {
                        if (
                            EntityManager.TryGetComponent(
                                entity,
                                out ConsumptionData consumptionData
                            )
                        )
                        {
                            if (ogUpkeep.ContainsKey(name) && ogUpkeep[name] != 0)
                            {
                                // Mixed
                                if (prefabBase.TryGet(out MaintenanceDepot maintenanceDepot))
                                {
                                    if (
                                        maintenanceDepot.m_MaintenanceType.HasFlag(
                                            Game.Simulation.MaintenanceType.Road
                                        )
                                    )
                                    {
                                        Adder(
                                            ref prevUpkeepValue,
                                            ref upkeepValue,
                                            Mod.m_Setting.RoadMaintenance,
                                            "RoadMaintenance"
                                        );
                                    }
                                    if (
                                        maintenanceDepot.m_MaintenanceType.HasFlag(
                                            Game.Simulation.MaintenanceType.Snow
                                        )
                                    )
                                    {
                                        Adder(
                                            ref prevUpkeepValue,
                                            ref upkeepValue,
                                            Mod.m_Setting.SnowPloughing,
                                            "SnowPloughing"
                                        );
                                    }
                                    if (
                                        maintenanceDepot.m_MaintenanceType.HasFlag(
                                            Game.Simulation.MaintenanceType.Park
                                        )
                                    )
                                    {
                                        Adder(
                                            ref prevUpkeepValue,
                                            ref upkeepValue,
                                            Mod.m_Setting.ParkMaintenance,
                                            "ParkMaintenance"
                                        );
                                        Adder(
                                            ref prevUpkeepValue,
                                            ref upkeepValue,
                                            Mod.m_Setting.ParkMaintenanceVehicle
                                                * maintenanceDepot.m_VehicleCapacity,
                                            "ParkMaintenanceVehicle"
                                        );
                                    }
                                    if (
                                        maintenanceDepot.m_MaintenanceType.HasFlag(
                                            Game.Simulation.MaintenanceType.Vehicle
                                        )
                                    )
                                    {
                                        Adder(
                                            ref prevUpkeepValue,
                                            ref upkeepValue,
                                            Mod.m_Setting.Towing,
                                            "Towing"
                                        );
                                    }
                                    if (
                                        maintenanceDepot.m_MaintenanceType.HasFlag(
                                            Game.Simulation.MaintenanceType.Snow
                                        )
                                        || maintenanceDepot.m_MaintenanceType.HasFlag(
                                            Game.Simulation.MaintenanceType.Road
                                        )
                                        || maintenanceDepot.m_MaintenanceType.HasFlag(
                                            Game.Simulation.MaintenanceType.Vehicle
                                        )
                                    )
                                    {
                                        Adder(
                                            ref prevUpkeepValue,
                                            ref upkeepValue,
                                            Mod.m_Setting.RoadMaintenanceVehicle
                                                * maintenanceDepot.m_VehicleCapacity,
                                            "RoadMaintenanceVehicle"
                                        );
                                    }
                                }

                                if (prefabBase.TryGet(out ObjectSubObjects objectSubObjects))
                                {
                                    int parkingSpaces = 0;
                                    int bicycleSpaces = 0;
                                    float platforms = 0f;

                                    for (int i = 0; i < objectSubObjects.m_SubObjects.Length; i++)
                                    {
                                        ObjectSubObjectInfo objectSubObjectInfo =
                                            objectSubObjects.m_SubObjects[i];
                                        if (
                                            objectSubObjectInfo == null
                                            || objectSubObjectInfo.m_Object == null
                                        )
                                            continue;

                                        if (
                                            objectSubObjectInfo.m_Object.TryGet(
                                                out ObjectSubLanes osl
                                            )
                                        )
                                        {
                                            foreach (
                                                ObjectSubLaneInfo subLaneInfo in osl.m_SubLanes
                                            )
                                            {
                                                if (
                                                    subLaneInfo.m_LanePrefab
                                                        is NetLanePrefab lanePrefab
                                                    && lanePrefab.TryGet(
                                                        out Game.Prefabs.ParkingLane parkingLane
                                                    )
                                                )
                                                {
                                                    int count = ParkingHelper.GetParkingSlotCount(
                                                        subLaneInfo.m_BezierCurve,
                                                        parkingLane
                                                    );
                                                    if (
                                                        parkingLane.m_RoadType
                                                        == Game.Net.RoadTypes.Car
                                                    )
                                                        parkingSpaces += count;
                                                    else if (
                                                        parkingLane.m_RoadType
                                                        == Game.Net.RoadTypes.Bicycle
                                                    )
                                                        bicycleSpaces += count;
                                                }
                                            }
                                        }

                                        if (
                                            objectSubObjectInfo.m_Object.TryGet(
                                                out TransportStop ts
                                            )
                                        )
                                        {
                                            switch (ts.m_TransportType)
                                            {
                                                case TransportType.Bicycle:
                                                    platforms += 0.1f;
                                                    break;
                                                case TransportType.Taxi:
                                                    platforms += 0.5f;
                                                    break;
                                                case TransportType.Bus:
                                                    platforms += 1.0f;
                                                    break;
                                                case TransportType.Tram:
                                                    platforms += 1.5f;
                                                    break;
                                                case TransportType.Train:
                                                case TransportType.Subway:
                                                    platforms += 2.0f;
                                                    break;
                                                case TransportType.Ferry:
                                                    platforms += 2.5f;
                                                    break;
                                                case TransportType.Ship:
                                                    platforms += 3.0f;
                                                    break;
                                                case TransportType.Airplane:
                                                    platforms += 4.0f;
                                                    break;
                                                case TransportType.Rocket:
                                                    platforms += 5.0f;
                                                    break;
                                            }
                                            if (ts.m_CargoTransport)
                                                platforms *= 1.5f;
                                        }

                                        //        string objName = objectSubObjectInfo.m_Object.name;
                                        //        if (
                                        //            !objName.StartsWith("ParkingLot")
                                        //            && !objName.Contains("Decal0")
                                        //            && !objName.StartsWith("Integrated")
                                        //        )
                                        //            continue;
                                        //        if (
                                        //            objName == "ParkingLotDecal01"
                                        //            || objName == "ParkingLotDiagonalDecal01"
                                        //            || objName == "ParkingLotDisabledDecal01"
                                        //            || objName == "ParkingLotElectricDecal01"
                                        //            || objName == "ParkingLotServiceDecal01"
                                        //        )
                                        //            parkingSpaces++;
                                        //        else if (
                                        //            objName == "ParkingLotDoubleDecal01"
                                        //            || objName == "ParkingLotDoubleDecal02"
                                        //            || objName == "ParkingLotDoubleServiceDecal01"
                                        //            || objName == "ParkingLotDoubleServiceDecal02"
                                        //        )
                                        //            parkingSpaces += 2;
                                        //        else if (
                                        //            objName == "ParkingLotDecal02"
                                        //            || objName == "ParkingLotDiagonalDecal02"
                                        //            || objName == "ParkingLotDisabledDecal02"
                                        //            || objName == "ParkingLotElectricDecal02"
                                        //            || objName == "ParkingLotServiceDecal02"
                                        //        )
                                        //            parkingSpaces += 3;
                                        //        else if (
                                        //            objName == "ParkingLotDecal03"
                                        //            || objName == "ParkingLotDiagonalDecal03"
                                        //            || objName == "ParkingLotServiceDecal03"
                                        //        )
                                        //            parkingSpaces += 5;
                                        //        else if (
                                        //            objName == "ParkingLotDecal04"
                                        //            || objName == "ParkingLotServiceDecal04"
                                        //        )
                                        //            parkingSpaces += 10;
                                        //        else if (
                                        //            objName.StartsWith("ParkingLot")
                                        //            && objName.Contains("Decal0")
                                        //        )
                                        //            parkingSpaces++;
                                        //        else if (objName.StartsWith("Integrated"))
                                        //            platforms++;
                                    }

                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.ParkingSpots * parkingSpaces,
                                        "ParkingSpots"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.ParkingSpots * 0.1f * bicycleSpaces,
                                        "ParkingSpots"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.PlatformMaintenance * platforms,
                                        "Platforms"
                                    );
                                }

                                // Roads
                                if (prefabBase.TryGet(out ParkingFacility parkingFacility))
                                {
                                    float multiBike =
                                        parkingFacility.m_RoadTypes == Game.Net.RoadTypes.Bicycle
                                            ? 0.1f
                                            : 1f;
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.ParkingSpots
                                            * multiBike
                                            * parkingFacility.m_GarageMarkerCapacity,
                                        "GarageMarker"
                                    );
                                }

                                // Electricity
                                if (prefabBase.TryGet(out SolarPowered solarPowered))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.SolarPowered
                                            * (solarPowered.m_Production / 1000f),
                                        "SolarPowered"
                                    );
                                }

                                if (prefabBase.TryGet(out GroundWaterPowered groundWaterPowered))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.GroundWaterPowered
                                            * (groundWaterPowered.m_Production / 1000f),
                                        "GroundWaterPowered"
                                    );
                                }

                                if (prefabBase.TryGet(out WaterPowered waterPowered))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.WaterPowered
                                            * (
                                                waterPowered.m_CapacityFactor
                                                * waterPowered.m_ProductionFactor
                                            )
                                            * 10,
                                        "WaterPowered"
                                    );
                                }

                                if (prefabBase.TryGet(out WindPowered windPowered))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.WindPowered
                                            * (windPowered.m_Production / 1000f),
                                        "WindPowered"
                                    );
                                }

                                if (prefabBase.TryGet(out GarbagePowered gabagePowered))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.GarbagePowered
                                            * gabagePowered.m_ProductionPerUnit
                                            * (gabagePowered.m_Capacity / 1000f),
                                        "GarbagePowered"
                                    );
                                }

                                if (prefabBase.TryGet(out PowerPlant powerPlant))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.ElectricityProduction
                                            * (powerPlant.m_ElectricityProduction / 1000f),
                                        "ElectricityProduction"
                                    );
                                }

                                if (prefabBase.TryGet(out Battery battery))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.BatteryOut * (battery.m_PowerOutput / 1000f),
                                        "BatteryOut"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.BatteryCap * (battery.m_Capacity / 1000f),
                                        "BatteryCap"
                                    );
                                }

                                if (prefabBase.TryGet(out Transformer transformer))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.Transformer,
                                        "Transformer"
                                    );
                                }

                                //Water & Sewage
                                if (prefabBase.TryGet(out WaterPumpingStation waterPumpingStation))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.WaterPumpCap
                                            * (waterPumpingStation.m_Capacity / 1000f),
                                        "WaterPumpCap"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.Purification
                                            * waterPumpingStation.m_Purification,
                                        "WaterPurification"
                                    );
                                }

                                if (prefabBase.TryGet(out SewageOutlet sewageOutlet))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.SewageOutCap
                                            * (sewageOutlet.m_Capacity / 1000f),
                                        "SewageOutCap"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.Purification * sewageOutlet.m_Purification,
                                        "SewagePurification"
                                    );
                                }

                                //Healthcare & Deathcare
                                if (prefabBase.TryGet(out Hospital hospital))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.Ambulance * hospital.m_AmbulanceCapacity,
                                        "Ambulance"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.MedicalHelicopter
                                            * hospital.m_MedicalHelicopterCapacity,
                                        "MedicalHelicopter"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.Patient * hospital.m_PatientCapacity,
                                        "Patient"
                                    );
                                    //Adder(ref prevUpkeepValue, ref upkeepValue,  Mod.m_Setting.HealthBonus * Hospital.m_TreatmentBonus, "HealthBonus");

                                    float weightedX = SegmentedWeight(
                                        hospital.m_HealthRange.x,
                                        Mod.m_Setting.HealthRange
                                    );
                                    float weightedY = SegmentedWeight(
                                        hospital.m_HealthRange.y,
                                        Mod.m_Setting.HealthRange
                                    );

                                    float extra = 0;
                                    if (hospital.m_TreatDiseases || hospital.m_TreatInjuries)
                                    {
                                        if (hospital.m_PatientCapacity > 0)
                                        {
                                            extra =
                                                (
                                                    Mod.m_Setting.Treatment
                                                    / hospital.m_PatientCapacity
                                                )
                                                * (
                                                    hospital.m_TreatmentBonus
                                                    / hospital.m_PatientCapacity
                                                );
                                        }
                                        else
                                        {
                                            extra = Mod.m_Setting.Treatment;
                                        }
                                    }
                                    else
                                    {
                                        if (hospital.m_TreatmentBonus > 0)
                                        {
                                            extra = Mod.m_Setting.HealthBonus;
                                        }
                                    }
                                    var valx =
                                        (
                                            (
                                                Math.Abs(weightedX - weightedY)
                                                / Mod.m_Setting.HealthRange
                                            ) / 100f
                                        ) * extra;
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        valx,
                                        "HealthBonusRange"
                                    );
                                    //Adder(ref prevUpkeepValue, ref upkeepValue,  Hospital.m_TreatDiseases ? Mod.m_Setting.Treatment : 0f, "TreatDiseases");
                                    //Adder(ref prevUpkeepValue, ref upkeepValue,  Hospital.m_TreatInjuries ? Mod.m_Setting.Treatment : 0f, "TreatInjuries");
                                }

                                if (prefabBase.TryGet(out DeathcareFacility deathcareFacility))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.Hearse * deathcareFacility.m_HearseCapacity,
                                        "Hearse"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        (Mod.m_Setting.BodyStorage / 100f)
                                            * deathcareFacility.m_StorageCapacity
                                            * (deathcareFacility.m_LongTermStorage ? 1f : 0.3f),
                                        "BodyStorage"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.BodyProcessing
                                            * deathcareFacility.m_ProcessingRate,
                                        "BodyProcessing"
                                    );
                                }

                                // Garbage Management
                                if (prefabBase.TryGet(out GarbageFacility garbageFacility))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.GarbageTruck
                                            * garbageFacility.m_VehicleCapacity,
                                        "GarbageTruck"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.DumpTruck
                                            * garbageFacility.m_TransportCapacity,
                                        "DumpTruck"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        (Mod.m_Setting.GarbageCap / 1000f)
                                            * garbageFacility.m_GarbageCapacity
                                            * (garbageFacility.m_LongTermStorage ? 1f : 0.3f)
                                            * (garbageFacility.m_IndustrialWasteOnly ? 0.1f : 1f),
                                        "GarbageCap"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        (Mod.m_Setting.GarbageProcessing / 1000f)
                                            * (garbageFacility.m_ProcessingSpeed / 1000f)
                                            * (garbageFacility.m_IndustrialWasteOnly ? 0.1f : 1f),
                                        "GarbageProcessing"
                                    );
                                }

                                // Education & Research
                                if (prefabBase.TryGet(out School school))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        (int)(
                                            Mod.m_Setting.Student
                                            * (1 + ((int)school.m_Level / 10))
                                            * school.m_StudentCapacity
                                        ),
                                        $"Student_{school.m_Level}"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.StudentGraduation
                                            * school.m_GraduationModifier
                                            * 100f,
                                        "StudentGraduation"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.StudentWellbeing * school.m_StudentWellbeing,
                                        "StudentWellbeing"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.StudentHealth * school.m_StudentHealth,
                                        "StudentHealth"
                                    );
                                }

                                if (prefabBase.TryGet(out ResearchFacility researchFacility))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.ResearchFacility,
                                        "ResearchFacility"
                                    );
                                }

                                // Fire & Rescue
                                if (prefabBase.TryGet(out FireStation fireStation))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.FireTruck * fireStation.m_FireEngineCapacity,
                                        "FireTruck"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.FireHelicopter
                                            * fireStation.m_FireHelicopterCapacity,
                                        "FireHelicopter"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.FireDisasterCap
                                            * fireStation.m_DisasterResponseCapacity,
                                        "FireDisasterCap"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.FireVehicleEffi
                                            * fireStation.m_VehicleEfficiency,
                                        "FireVehicleEffi"
                                    );
                                }

                                if (prefabBase.TryGet(out FirewatchTower firewatchTower))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.Firewatch,
                                        "Firewatch"
                                    );
                                }

                                if (
                                    prefabBase.TryGet(
                                        out EarlyDisasterWarningSystem earlyDisasterWarningSystem
                                    )
                                )
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.EarlyDisasterWarningSystem,
                                        "EarlyDisasterWarningSystem"
                                    );
                                }

                                if (prefabBase.TryGet(out DisasterFacility disasterFacility))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.DisasterFacility,
                                        "DisasterFacility"
                                    );
                                }

                                if (prefabBase.TryGet(out EmergencyShelter emergencyShelter))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.EvacuationBus
                                            * emergencyShelter.m_VehicleCapacity,
                                        "EvacuationBus"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.ShelterCap
                                            * emergencyShelter.m_ShelterCapacity
                                            / 1000f,
                                        "ShelterCap"
                                    );
                                }

                                if (prefabBase.TryGet(out EmergencyGenerator emergencyGenerator))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.EmergencyGenerator
                                            * (emergencyGenerator.m_ElectricityProduction / 1000f),
                                        "EmergencyGenerator"
                                    );
                                }

                                // Police & Administration
                                if (prefabBase.TryGet(out PoliceStation policeStation))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.PatrolCar * policeStation.m_PatrolCarCapacity,
                                        "PatrolCar"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.PoliceHelicopter
                                            * policeStation.m_PoliceHelicopterCapacity,
                                        "PoliceHelicopter"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.LocalJail * policeStation.m_JailCapacity,
                                        "LocalJail"
                                    );

                                    if (policeStation.m_Purposes.HasFlag(PolicePurpose.Patrol))
                                    {
                                        Adder(
                                            ref prevUpkeepValue,
                                            ref upkeepValue,
                                            Mod.m_Setting.Patrol,
                                            "Patrol"
                                        );
                                    }
                                    if (policeStation.m_Purposes.HasFlag(PolicePurpose.Emergency))
                                    {
                                        Adder(
                                            ref prevUpkeepValue,
                                            ref upkeepValue,
                                            Mod.m_Setting.EmergencyPolice,
                                            "Emergency"
                                        );
                                    }
                                    if (
                                        policeStation.m_Purposes.HasFlag(PolicePurpose.Intelligence)
                                    )
                                    {
                                        Adder(
                                            ref prevUpkeepValue,
                                            ref upkeepValue,
                                            Mod.m_Setting.Intelligence,
                                            "Intelligence"
                                        );
                                    }
                                }

                                if (prefabBase.TryGet(out Prison prison))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.PrisonVan * prison.m_PrisonVanCapacity,
                                        "PrisonVan"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.PrisonerCap * prison.m_PrisonerCapacity,
                                        "PrisonerCap"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.PrisonerWellbeing
                                            * prison.m_PrisonerWellbeing,
                                        "PrisonerWellbeing"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.PrisonerHealth * prison.m_PrisonerHealth,
                                        "PrisonerHealth"
                                    );
                                }

                                if (prefabBase.TryGet(out WelfareOffice welfareOffice))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.WelfareOffice,
                                        "WelfareOffice"
                                    );
                                }

                                if (
                                    prefabBase.TryGet(
                                        out AdministrationBuilding administrationBuilding
                                    )
                                )
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.AdminBuilding,
                                        "AdminBuilding"
                                    );
                                }

                                // Transportation
                                if (prefabBase.TryGet(out TransportDepot transportDepot))
                                {
                                    TransportType transportType = transportDepot.m_TransportType;
                                    float transportMuliplier = 0f;
                                    switch (transportType)
                                    {
                                        case TransportType.Bus:
                                            transportMuliplier = Mod.m_Setting.Bus;
                                            break;
                                        case TransportType.Train:
                                            transportMuliplier = Mod.m_Setting.Train;
                                            break;
                                        case TransportType.Taxi:
                                            transportMuliplier = Mod.m_Setting.Taxi;
                                            break;
                                        case TransportType.Tram:
                                            transportMuliplier = Mod.m_Setting.Tram;
                                            break;
                                        case TransportType.Ship:
                                            transportMuliplier = Mod.m_Setting.Ship;
                                            break;
                                        //case TransportType.Post:
                                        //    transportMuliplier = Mod.m_Setting.Post;
                                        //    break;
                                        //case TransportType.Helicopter:
                                        //    transportMuliplier = Mod.m_Setting.Helicopter;
                                        //    break;
                                        case TransportType.Airplane:
                                            transportMuliplier = Mod.m_Setting.Airplane;
                                            break;
                                        case TransportType.Subway:
                                            transportMuliplier = Mod.m_Setting.Subway;
                                            break;
                                        case TransportType.Rocket:
                                            transportMuliplier = Mod.m_Setting.Rocket;
                                            break;
                                    }
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        transportMuliplier * transportDepot.m_VehicleCapacity,
                                        $"TransportType_{transportType}"
                                    );

                                    Game.Vehicles.EnergyTypes energyType =
                                        transportDepot.m_EnergyTypes;
                                    float energyValue = 0f;
                                    switch (energyType)
                                    {
                                        case Game.Vehicles.EnergyTypes.Fuel:
                                            energyValue = Mod.m_Setting.EnergyFuel;
                                            break;
                                        case Game.Vehicles.EnergyTypes.Electricity:
                                            energyValue = Mod.m_Setting.EnergyElectricity;
                                            break;
                                        case Game.Vehicles.EnergyTypes.FuelAndElectricity:
                                            energyValue =
                                                Mod.m_Setting.EnergyFuel
                                                + Mod.m_Setting.EnergyElectricity;
                                            break;
                                    }
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        energyValue,
                                        $"EnergyTypes_{energyType}"
                                    );

                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Math.Max(
                                            Mod.m_Setting.MaintenanceBoost,
                                            Mod.m_Setting.MaintenanceBoost
                                                * (transportDepot.m_MaintenanceDuration * 100f)
                                        ),
                                        "MaintenanceBoost"
                                    );

                                    if (transportDepot.m_DispatchCenter)
                                    {
                                        Adder(
                                            ref prevUpkeepValue,
                                            ref upkeepValue,
                                            Mod.m_Setting.DispatchCenter,
                                            "DispatchCenter"
                                        );
                                    }
                                }

                                if (
                                    prefabBase.TryGet(
                                        out CargoTransportStation cargoTransportStation
                                    )
                                )
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.TradedResource
                                            * cargoTransportStation.m_TradedResources.Length,
                                        "TradedResource"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.DeliveryTruck
                                            * cargoTransportStation.transports,
                                        "DeliveryTruck"
                                    );

                                    if (
                                        !cargoTransportStation.m_CarRefuelTypes.HasFlag(
                                            Game.Vehicles.EnergyTypes.None
                                        )
                                    )
                                    {
                                        float energyValue = 0f;
                                        switch (cargoTransportStation.m_CarRefuelTypes)
                                        {
                                            case Game.Vehicles.EnergyTypes.Fuel:
                                                energyValue = Mod.m_Setting.EnergyFuel;
                                                break;
                                            case Game.Vehicles.EnergyTypes.Electricity:
                                                energyValue = Mod.m_Setting.EnergyElectricity;
                                                break;
                                            case Game.Vehicles.EnergyTypes.FuelAndElectricity:
                                                energyValue =
                                                    Mod.m_Setting.EnergyFuel
                                                    + Mod.m_Setting.EnergyElectricity;
                                                break;
                                        }
                                        Adder(
                                            ref prevUpkeepValue,
                                            ref upkeepValue,
                                            energyValue,
                                            $"CarRefuelTypes_{cargoTransportStation.m_CarRefuelTypes}"
                                        );
                                    }

                                    if (
                                        !cargoTransportStation.m_TrainRefuelTypes.HasFlag(
                                            Game.Vehicles.EnergyTypes.None
                                        )
                                    )
                                    {
                                        float energyValue = 0f;
                                        switch (cargoTransportStation.m_TrainRefuelTypes)
                                        {
                                            case Game.Vehicles.EnergyTypes.Fuel:
                                                energyValue = Mod.m_Setting.EnergyFuel;
                                                break;
                                            case Game.Vehicles.EnergyTypes.Electricity:
                                                energyValue = Mod.m_Setting.EnergyElectricity;
                                                break;
                                            case Game.Vehicles.EnergyTypes.FuelAndElectricity:
                                                energyValue =
                                                    Mod.m_Setting.EnergyFuel
                                                    + Mod.m_Setting.EnergyElectricity;
                                                break;
                                        }
                                        Adder(
                                            ref prevUpkeepValue,
                                            ref upkeepValue,
                                            energyValue,
                                            $"TrainRefuelTypes_{cargoTransportStation.m_TrainRefuelTypes}"
                                        );
                                    }

                                    if (
                                        !cargoTransportStation.m_WatercraftRefuelTypes.HasFlag(
                                            Game.Vehicles.EnergyTypes.None
                                        )
                                    )
                                    {
                                        float energyValue = 0f;
                                        switch (cargoTransportStation.m_WatercraftRefuelTypes)
                                        {
                                            case Game.Vehicles.EnergyTypes.Fuel:
                                                energyValue = Mod.m_Setting.EnergyFuel;
                                                break;
                                            case Game.Vehicles.EnergyTypes.Electricity:
                                                energyValue = Mod.m_Setting.EnergyElectricity;
                                                break;
                                            case Game.Vehicles.EnergyTypes.FuelAndElectricity:
                                                energyValue =
                                                    Mod.m_Setting.EnergyFuel
                                                    + Mod.m_Setting.EnergyElectricity;
                                                break;
                                        }
                                        Adder(
                                            ref prevUpkeepValue,
                                            ref upkeepValue,
                                            energyValue,
                                            $"WatercraftRefuel_{cargoTransportStation.m_WatercraftRefuelTypes}"
                                        );
                                    }

                                    if (
                                        !cargoTransportStation.m_AircraftRefuelTypes.HasFlag(
                                            Game.Vehicles.EnergyTypes.None
                                        )
                                    )
                                    {
                                        float energyValue = 0f;
                                        switch (cargoTransportStation.m_AircraftRefuelTypes)
                                        {
                                            case Game.Vehicles.EnergyTypes.Fuel:
                                                energyValue = Mod.m_Setting.EnergyFuel;
                                                break;
                                            case Game.Vehicles.EnergyTypes.Electricity:
                                                energyValue = Mod.m_Setting.EnergyElectricity;
                                                break;
                                            case Game.Vehicles.EnergyTypes.FuelAndElectricity:
                                                energyValue =
                                                    Mod.m_Setting.EnergyFuel
                                                    + Mod.m_Setting.EnergyElectricity;
                                                break;
                                        }
                                        Adder(
                                            ref prevUpkeepValue,
                                            ref upkeepValue,
                                            energyValue,
                                            $"AircraftRefuelTypes_{cargoTransportStation.m_AircraftRefuelTypes}"
                                        );
                                    }
                                }

                                if (prefabBase.TryGet(out TransportStation transportStation))
                                {
                                    if (
                                        !transportStation.m_CarRefuelTypes.HasFlag(
                                            Game.Vehicles.EnergyTypes.None
                                        )
                                    )
                                    {
                                        float energyValue = 0f;
                                        switch (transportStation.m_CarRefuelTypes)
                                        {
                                            case Game.Vehicles.EnergyTypes.Fuel:
                                                energyValue = Mod.m_Setting.EnergyFuel;
                                                break;
                                            case Game.Vehicles.EnergyTypes.Electricity:
                                                energyValue = Mod.m_Setting.EnergyElectricity;
                                                break;
                                            case Game.Vehicles.EnergyTypes.FuelAndElectricity:
                                                energyValue =
                                                    Mod.m_Setting.EnergyFuel
                                                    + Mod.m_Setting.EnergyElectricity;
                                                break;
                                        }
                                        Adder(
                                            ref prevUpkeepValue,
                                            ref upkeepValue,
                                            energyValue,
                                            $"CarRefuelTypes_{transportStation.m_CarRefuelTypes}"
                                        );
                                    }

                                    if (
                                        !transportStation.m_TrainRefuelTypes.HasFlag(
                                            Game.Vehicles.EnergyTypes.None
                                        )
                                    )
                                    {
                                        float energyValue = 0f;
                                        switch (transportStation.m_TrainRefuelTypes)
                                        {
                                            case Game.Vehicles.EnergyTypes.Fuel:
                                                energyValue = Mod.m_Setting.EnergyFuel;
                                                break;
                                            case Game.Vehicles.EnergyTypes.Electricity:
                                                energyValue = Mod.m_Setting.EnergyElectricity;
                                                break;
                                            case Game.Vehicles.EnergyTypes.FuelAndElectricity:
                                                energyValue =
                                                    Mod.m_Setting.EnergyFuel
                                                    + Mod.m_Setting.EnergyElectricity;
                                                break;
                                        }
                                        Adder(
                                            ref prevUpkeepValue,
                                            ref upkeepValue,
                                            energyValue,
                                            $"TrainRefuelTypes_{transportStation.m_TrainRefuelTypes}"
                                        );
                                    }

                                    if (
                                        !transportStation.m_WatercraftRefuelTypes.HasFlag(
                                            Game.Vehicles.EnergyTypes.None
                                        )
                                    )
                                    {
                                        float energyValue = 0f;
                                        switch (transportStation.m_WatercraftRefuelTypes)
                                        {
                                            case Game.Vehicles.EnergyTypes.Fuel:
                                                energyValue = Mod.m_Setting.EnergyFuel;
                                                break;
                                            case Game.Vehicles.EnergyTypes.Electricity:
                                                energyValue = Mod.m_Setting.EnergyElectricity;
                                                break;
                                            case Game.Vehicles.EnergyTypes.FuelAndElectricity:
                                                energyValue =
                                                    Mod.m_Setting.EnergyFuel
                                                    + Mod.m_Setting.EnergyElectricity;
                                                break;
                                        }
                                        Adder(
                                            ref prevUpkeepValue,
                                            ref upkeepValue,
                                            energyValue,
                                            $"WatercraftRefuel_{transportStation.m_WatercraftRefuelTypes}"
                                        );
                                    }

                                    if (
                                        !transportStation.m_AircraftRefuelTypes.HasFlag(
                                            Game.Vehicles.EnergyTypes.None
                                        )
                                    )
                                    {
                                        float energyValue = 0f;
                                        switch (transportStation.m_AircraftRefuelTypes)
                                        {
                                            case Game.Vehicles.EnergyTypes.Fuel:
                                                energyValue = Mod.m_Setting.EnergyFuel;
                                                break;
                                            case Game.Vehicles.EnergyTypes.Electricity:
                                                energyValue = Mod.m_Setting.EnergyElectricity;
                                                break;
                                            case Game.Vehicles.EnergyTypes.FuelAndElectricity:
                                                energyValue =
                                                    Mod.m_Setting.EnergyFuel
                                                    + Mod.m_Setting.EnergyElectricity;
                                                break;
                                        }
                                        Adder(
                                            ref prevUpkeepValue,
                                            ref upkeepValue,
                                            energyValue,
                                            $"AircraftRefuelTypes_{transportStation.m_AircraftRefuelTypes}"
                                        );
                                    }

                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.ComfortFactor
                                            * transportStation.m_ComfortFactor
                                            * 100f,
                                        "ComfortFactor"
                                    );
                                }

                                // Parks & Recreation
                                if (prefabBase.TryGet(out LeisureProvider leisureProvider))
                                {
                                    Game.Agents.LeisureType leisureType =
                                        leisureProvider.m_LeisureType;
                                    float leisureMultiplier = 0f;
                                    switch (leisureType)
                                    {
                                        case Game.Agents.LeisureType.Meals:
                                            leisureMultiplier = Mod.m_Setting.LeisureMeals;
                                            break;
                                        case Game.Agents.LeisureType.Entertainment:
                                            leisureMultiplier = Mod.m_Setting.LeisureEntertainment;
                                            break;
                                        case Game.Agents.LeisureType.Commercial:
                                            leisureMultiplier = Mod.m_Setting.LeisureCommercial;
                                            break;
                                        case Game.Agents.LeisureType.CityIndoors:
                                            leisureMultiplier = Mod.m_Setting.LeisureCityIndoors;
                                            break;
                                        case Game.Agents.LeisureType.Travel:
                                            leisureMultiplier = Mod.m_Setting.LeisureTravel;
                                            break;
                                        case Game.Agents.LeisureType.CityPark:
                                            leisureMultiplier = Mod.m_Setting.LeisureCityPark;
                                            break;
                                        case Game.Agents.LeisureType.CityBeach:
                                            leisureMultiplier = Mod.m_Setting.LeisureCityBeach;
                                            break;
                                        case Game.Agents.LeisureType.Attractions:
                                            leisureMultiplier = Mod.m_Setting.LeisureAttractions;
                                            break;
                                        case Game.Agents.LeisureType.Relaxation:
                                            leisureMultiplier = Mod.m_Setting.LeisureRelaxation;
                                            break;
                                        case Game.Agents.LeisureType.Sightseeing:
                                            leisureMultiplier = Mod.m_Setting.LeisureSightseeing;
                                            break;
                                    }
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        leisureMultiplier
                                            * (
                                                Mod.m_Setting.LeisureEfficieny
                                                * (leisureProvider.m_Efficiency / 100f)
                                            ),
                                        $"LeisureProvider_{leisureProvider.m_LeisureType}"
                                    );
                                }

                                if (prefabBase.TryGet(out Attraction attraction))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.Attraction * attraction.m_Attractiveness,
                                        "Attraction"
                                    );
                                }

                                // Communication
                                if (prefabBase.TryGet(out PostFacility postFacility))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.PostVan * postFacility.m_PostVanCapacity,
                                        "PostVan"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.PostTruck * postFacility.m_PostTruckCapacity,
                                        "PostTruck"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.MailCap
                                            * (
                                                (
                                                    postFacility.m_MailStorageCapacity
                                                    + postFacility.m_MailBoxCapacity
                                                ) / 1000f
                                            ),
                                        "MailCap"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.PostSortingRate
                                            * (postFacility.m_SortingRate / 1000f),
                                        "PostSortingRate"
                                    );
                                }

                                if (prefabBase.TryGet(out TelecomFacility telecomFacility))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.TelecomRange
                                            * (telecomFacility.m_Range / 1000f),
                                        "TelecomRange"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.NetworkCap
                                            * (telecomFacility.m_NetworkCapacity / 1000f),
                                        "NetworkCap"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Unity.Mathematics.math.select(
                                            Mod.m_Setting.Fibre,
                                            Mod.m_Setting.Wireless,
                                            telecomFacility.m_PenetrateTerrain
                                        ),
                                        $"Fibre {telecomFacility.m_PenetrateTerrain}"
                                    );
                                }

                                // Landscaping

                                // General
                                if (prefabBase.TryGet(out ServiceCoverage serviceCoverage))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.ServiceCoverageMultiplier
                                            * serviceCoverage.m_Capacity
                                            * (
                                                serviceCoverage.m_Magnitude
                                                / serviceCoverage.m_Range
                                            ),
                                        "ServiceCoverage"
                                    );
                                }

                                if (prefabBase.TryGet(out Workplace workplace))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.EmployeeUpkeep * workplace.m_Workplaces,
                                        $"EmployeeUpkeep_{workplace.m_Complexity}"
                                    );
                                }

                                if (prefabBase.TryGet(out StorageLimit storageLimit))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.StorageUpkeep
                                            * (storageLimit.storageLimit / 100000f),
                                        "StorageLimit"
                                    );
                                }

                                if (prefabBase.TryGet(out Pollution pollution))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.GroundPollution
                                            * Math.Abs(pollution.m_GroundPollution / 1000f),
                                        "GroundPollution"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.AirPollution
                                            * Math.Abs(pollution.m_AirPollution / 1000f),
                                        "AirPollution"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.NoisePollution
                                            * Math.Abs(pollution.m_NoisePollution / 1000f),
                                        "NoisePollution"
                                    );
                                }

                                if (prefabBase.TryGet(out PollutionModifier pollutionModifier))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.GroundPollution
                                            * Math.Abs(
                                                pollutionModifier.m_GroundPollutionMultiplier * 10f
                                            ),
                                        "GroundPollutionMultiplier"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.AirPollution
                                            * Math.Abs(
                                                pollutionModifier.m_AirPollutionMultiplier * 10f
                                            ),
                                        "AirPollutionMultiplier"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        Mod.m_Setting.NoisePollution
                                            * Math.Abs(
                                                pollutionModifier.m_NoisePollutionMultiplier * 10f
                                            ),
                                        "NoisePollutionMultiplier"
                                    );
                                }

                                if (prefabBase.TryGet(out UniqueObject uniqueObject))
                                {
                                    Multiplier(
                                        ref prevMultiplierValue,
                                        ref multiplier,
                                        Mod.m_Setting.Uniqueness / 100f,
                                        "Uniqueness"
                                    );
                                }

                                //if (prefabBase.TryGet(out ServiceObject serviceObject))
                                //{
                                //    foreach (var data in serviceBudgetData)
                                //    {
                                //        if (data.m_Service == serviceDict[serviceObject.m_Service])
                                //        {
                                //            Multiplier(
                                //                ref prevMultiplierValue,
                                //                ref multiplier,
                                //                data.m_Budget / 100f,
                                //                "ServiceObject"
                                //            );
                                //            break;
                                //        }
                                //    }
                                //}

                                if (prefabBase.TryGet(out ServiceUpgrade serviceUpgrade))
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        serviceUpgrade.m_UpgradeCost * 0.05f,
                                        "ServiceUpgrade"
                                    );
                                }

                                //CityEffects CityEffects = prefabBase.GetComponent<CityEffects>();
                                //LogHelper.SendLog("CityEffects",LogLevel.DEV);
                                //if (CityEffects != null && Mod.m_Setting.CityBonusMultiplier)
                                //{
                                //    foreach (var data in CityEffects.m_Effects)
                                //    {
                                //        if (data.m_Mode.HasFlag(ModifierValueMode.Relative) || data.m_Mode.HasFlag(ModifierValueMode.InverseRelative))
                                //        {
                                //            Multiplier(ref prevMultiplierValue, ref multiplier,  Math.Abs(data.m_Delta), $"CityEffects_{data.m_Mode}");
                                //        }
                                //    }
                                //}

                                //LocalEffects LocalEffects = prefabBase.GetComponent<LocalEffects>();
                                //LogHelper.SendLog("LocalEffects",LogLevel.DEV);
                                //if (LocalEffects != null && Mod.m_Setting.CityBonusMultiplier)
                                //{
                                //    foreach (var data in LocalEffects.m_Effects)
                                //    {
                                //        if (data.m_Mode.HasFlag(ModifierValueMode.Relative) || data.m_Mode.HasFlag(ModifierValueMode.InverseRelative))
                                //        {
                                //            Multiplier(ref prevMultiplierValue, ref multiplier,  Math.Abs(data.m_Delta), $"LocalEffects_{data.m_Mode}");
                                //        }
                                //        //if (data.m_Mode == ModifierValueMode.Absolute)
                                //        //{
                                //        //    Multiplier(ref prevMultiplierValue, ref multiplier,  Math.Abs(data.m_Delta - 100f), "LocalEffects.Absolute");
                                //        //}
                                //    }
                                //}

                                float height = 5;
                                if (
                                    EntityManager.TryGetComponent(
                                        entity,
                                        out ObjectGeometryData objectGeometryData
                                    )
                                )
                                {
                                    if (objectGeometryData.m_Size.y >= 1f)
                                        height = objectGeometryData.m_Size.y;
                                }

                                if (
                                    prefabSystem.TryGetPrefab(
                                        prefabData,
                                        out BuildingExtensionPrefab buildingExtension
                                    ) && buildingExtension is not null
                                )
                                {
                                    int plotSize =
                                        buildingExtension.m_OverrideLotSize.x
                                        * buildingExtension.m_OverrideLotSize.y;
                                    if (plotSize > 0)
                                    {
                                        Adder(
                                            ref prevUpkeepValue,
                                            ref upkeepValue,
                                            (float)(
                                                Mod.m_Setting.PlotPrice
                                                * Math.Round(Math.Log(plotSize + 1) * height)
                                            ),
                                            "PlotPrice"
                                        );
                                    }
                                }

                                if (
                                    prefabSystem.TryGetPrefab(
                                        prefabData,
                                        out BuildingPrefab building
                                    ) && building is not null
                                )
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        (float)(
                                            Mod.m_Setting.PlotPrice
                                            * Math.Round(Math.Log(building.lotSize + 1) * height)
                                        ),
                                        "PlotPrice"
                                    );
                                }

                                multiplier = Math.Max(multiplier, 0.1f);
                                int upkeepValueInt =
                                    (int)Math.Round((upkeepValue * multiplier) / 100) * 100;
                                if (upkeepValueInt >= 0)
                                {
                                    consumptionData.m_Upkeep = upkeepValueInt;
                                    EntityManager.SetComponentData(entity, consumptionData);
                                    RefreshBuffer(entity, upkeepValueInt);
#if DEBUG

                                    string logText = $"{name};{ogUpkeep[name]};{upkeepValueInt}";
                                    logText = $"{logText};multiplier={multiplier}";
                                    foreach (var vals in prevVal)
                                    {
                                        logText = $"{logText};{vals.Key}={vals.Value}";
                                    }
                                    sb.AppendLine(logText);
#endif
                                }
                                else
                                {
                                    LogHelper.SendLog(
                                        $"Something went wrong: {name}'s upkeep is ${upkeepValue}?",
                                        LogLevel.Error
                                    );
                                    string logText = $"{name};{ogUpkeep[name]};{upkeepValueInt}";
                                    logText = $"{logText};multiplier={multiplier}";
                                    foreach (var vals in prevVal)
                                    {
                                        logText = $"{logText};{vals.Key}={vals.Value}";
                                    }
                                    LogHelper.SendLog(logText, LogLevel.Error);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    LogHelper.SendLog(e, LogLevel.DEV);
                }
            }

            LogHelper.SendLog(sb, LogLevel.DEVD);
        }

        public void ResetToVanilla()
        {
            foreach (var item in buildingDict)
            {
                try
                {
                    string name = item.Key;
                    Entity entity = item.Value;

                    if (prefabSystem.TryGetPrefab(entity, out PrefabBase prefabBase))
                    {
                        if (
                            EntityManager.TryGetComponent(
                                entity,
                                out ConsumptionData consumptionData
                            )
                        )
                        {
                            if (ogUpkeep.ContainsKey(name) && ogUpkeep[name] != 0)
                            {
                                consumptionData.m_Upkeep = ogUpkeep[name];
                                EntityManager.SetComponentData(entity, consumptionData);
                                RefreshBuffer(entity, ogUpkeep[name]);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    LogHelper.SendLog(e, LogLevel.Error);
                }
            }
        }

        public void RefreshBuffer(Entity entity, int amount)
        {
            try
            {
                if (
                    !EntityManager.TryGetBuffer<ServiceUpkeepData>(
                        entity,
                        false,
                        out DynamicBuffer<ServiceUpkeepData> sudBuffer
                    )
                )
                    return;

                for (int i = 0; i < sudBuffer.Length; i++)
                {
                    ServiceUpkeepData uge = sudBuffer[i];
                    if (uge.m_Upkeep.m_Resource == Game.Economy.Resource.Money)
                    {
                        uge.m_Upkeep.m_Amount = amount;
                        sudBuffer[i] = uge;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.SendLog(e, LogLevel.Error);
            }
        }

        float SegmentedWeight(float input, float N)
        {
            input = Math.Clamp(input, 0f, 100f);
            int segment = (int)(input / 25f);
            float fraction = (input % 25f) / 25f;
            float baseWeight = 0f;

            for (int i = 0; i < segment; i++)
                baseWeight += (i + 1) * N;

            float currentSegmentWeight = (segment + 1) * N * fraction;
            return baseWeight + currentSegmentWeight;

            //if (input < 25)
            //    return (input / 25) * N;
            //else if (input < 50)
            //    return N + (((input - 25) / 25) * 2f * N);
            //else if (input < 75)
            //    return N + (2f * N) + (((input - 50) / 25) * 3f * N);
            //else
            //    return N + (2f * N) + (3f * N) + (((input - 75) / 25) * 4f * N);
        }

        void Adder(ref float prevUpkeepValue, ref float upkeepValue, float adder, string text)
        {
            prevUpkeepValue = upkeepValue;
            upkeepValue += (float)adder;
            var diff = upkeepValue - prevUpkeepValue;
            if (!prevVal.ContainsKey(text) && diff != 0f)
                prevVal[text] = diff;
        }

        void Multiplier(
            ref float prevMultiplierValue,
            ref float multipler,
            float adder,
            string text
        )
        {
            prevMultiplierValue = multipler;
            multipler += (float)adder;
            var diff = multipler - prevMultiplierValue;
            if (!prevVal.ContainsKey($"{text}_x") && diff != 0f)
                prevVal[$"{text}_x"] = diff;
        }
    }
}
