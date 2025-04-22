using System;
using System.Runtime.Remoting.Lifetime;
using Colossal.Entities;
using Game;
using Game.Prefabs;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace SmartUpkeepManager.Systems
{
    public partial class SmartUpkeepSystem : GameSystemBase
    {
        public void CalculateUpkeep()
        {
            bool logX = false;
#if DEBUG
            logX = true;
#endif
            EntityQuery serviceBudgetQuery = SystemAPI
                .QueryBuilder()
                .WithAll<Game.Simulation.ServiceBudgetData>()
                .Build();
            Entity serviceBudgetEntity = serviceBudgetQuery.ToEntityArray(Allocator.Temp)[0];
            EntityManager.TryGetBuffer(
                serviceBudgetEntity,
                true,
                out DynamicBuffer<Game.Simulation.ServiceBudgetData> serviceBudgetData
            );

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

                    //Mod.log.Info("============");

                    if (
                        EntityManager.TryGetComponent(entity, out PrefabData prefabData)
                        && prefabSystem.TryGetPrefab(prefabData, out PrefabBase prefabBase)
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
                                MaintenanceDepot maintenanceDepot =
                                    prefabBase.GetComponent<MaintenanceDepot>();
                                CrossExamine("MaintenanceDepot");
                                if (maintenanceDepot != null)
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
                                            logX,
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
                                            logX,
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
                                            logX,
                                            Mod.m_Setting.ParkMaintenance,
                                            "ParkMaintenance"
                                        );
                                        Adder(
                                            ref prevUpkeepValue,
                                            ref upkeepValue,
                                            logX,
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
                                            logX,
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
                                            logX,
                                            Mod.m_Setting.RoadMaintenanceVehicle
                                                * maintenanceDepot.m_VehicleCapacity,
                                            "RoadMaintenanceVehicle"
                                        );
                                    }
                                }

                                ObjectSubObjects objectSubObjects =
                                    prefabBase.GetComponent<ObjectSubObjects>();
                                CrossExamine("ObjectSubObjects");
                                if (objectSubObjects != null)
                                {
                                    int parkingSpaces = 0;
                                    int platforms = 0;
                                    for (int i = 0; i < objectSubObjects.m_SubObjects.Length; i++)
                                    {
                                        ObjectSubObjectInfo objectSubObjectInfo =
                                            objectSubObjects.m_SubObjects[i];
                                        if (
                                            objectSubObjectInfo == null
                                            || objectSubObjectInfo.m_Object == null
                                        )
                                            continue;
                                        string objName = objectSubObjectInfo.m_Object.name;
                                        if (
                                            !objName.StartsWith("ParkingLot")
                                            && !objName.Contains("Decal0")
                                            && !objName.StartsWith("Integrated")
                                        )
                                            continue;
                                        if (
                                            objName == "ParkingLotDecal01"
                                            || objName == "ParkingLotDiagonalDecal01"
                                            || objName == "ParkingLotDisabledDecal01"
                                            || objName == "ParkingLotElectricDecal01"
                                            || objName == "ParkingLotServiceDecal01"
                                        )
                                        {
                                            parkingSpaces++;
                                        }
                                        else if (
                                            objName == "ParkingLotDoubleDecal01"
                                            || objName == "ParkingLotDoubleDecal02"
                                            || objName == "ParkingLotDoubleServiceDecal01"
                                            || objName == "ParkingLotDoubleServiceDecal02"
                                        )
                                        {
                                            parkingSpaces += 2;
                                        }
                                        else if (
                                            objName == "ParkingLotDecal02"
                                            || objName == "ParkingLotDiagonalDecal02"
                                            || objName == "ParkingLotDisabledDecal02"
                                            || objName == "ParkingLotElectricDecal02"
                                            || objName == "ParkingLotServiceDecal02"
                                        )
                                        {
                                            parkingSpaces += 3;
                                        }
                                        else if (
                                            objName == "ParkingLotDecal03"
                                            || objName == "ParkingLotDiagonalDecal03"
                                            || objName == "ParkingLotServiceDecal03"
                                        )
                                        {
                                            parkingSpaces += 5;
                                        }
                                        else if (
                                            objName == "ParkingLotDecal04"
                                            || objName == "ParkingLotServiceDecal04"
                                        )
                                        {
                                            parkingSpaces += 10;
                                        }
                                        else if (
                                            objName.StartsWith("ParkingLot")
                                            && objName.Contains("Decal0")
                                        )
                                        {
                                            parkingSpaces++;
                                        }
                                        else if (objName.StartsWith("Integrated"))
                                        {
                                            platforms++;
                                        }
                                    }

                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.ParkingSpots * parkingSpaces,
                                        "ParkingSpots"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.PlatformMaintenance * platforms,
                                        "Platforms"
                                    );
                                }

                                // Roads
                                ParkingFacility parkingFacility =
                                    prefabBase.GetComponent<ParkingFacility>();
                                CrossExamine("ParkingFacility");
                                if (parkingFacility != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.ParkingSpots
                                            * parkingFacility.m_GarageMarkerCapacity,
                                        "GarageMarker"
                                    );
                                }

                                // Electricity
                                SolarPowered solarPowered = prefabBase.GetComponent<SolarPowered>();
                                CrossExamine("SolarPowered");
                                if (solarPowered != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.SolarPowered
                                            * (solarPowered.m_Production / 1000f),
                                        "SolarPowered"
                                    );
                                }

                                GroundWaterPowered groundWaterPowered =
                                    prefabBase.GetComponent<GroundWaterPowered>();
                                CrossExamine("GroundWaterPowered");
                                if (groundWaterPowered != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.GroundWaterPowered
                                            * (groundWaterPowered.m_Production / 1000f),
                                        "GroundWaterPowered"
                                    );
                                }

                                WaterPowered waterPowered = prefabBase.GetComponent<WaterPowered>();
                                CrossExamine("WaterPowered");
                                if (waterPowered != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.WaterPowered
                                            * (
                                                waterPowered.m_CapacityFactor
                                                * waterPowered.m_ProductionFactor
                                            )
                                            * 10,
                                        "WaterPowered"
                                    );
                                }

                                WindPowered windPowered = prefabBase.GetComponent<WindPowered>();
                                CrossExamine("WindPowered");
                                if (windPowered != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.WindPowered
                                            * (windPowered.m_Production / 1000f),
                                        "WindPowered"
                                    );
                                }

                                GarbagePowered gabagePowered =
                                    prefabBase.GetComponent<GarbagePowered>();
                                CrossExamine("GarbagePowered");
                                if (gabagePowered != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.GarbagePowered
                                            * gabagePowered.m_ProductionPerUnit
                                            * (gabagePowered.m_Capacity / 1000f),
                                        "GarbagePowered"
                                    );
                                }

                                PowerPlant powerPlant = prefabBase.GetComponent<PowerPlant>();
                                CrossExamine("PowerPlant");
                                if (powerPlant != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.ElectricityProduction
                                            * (powerPlant.m_ElectricityProduction / 1000f),
                                        "ElectricityProduction"
                                    );
                                }

                                Battery battery = prefabBase.GetComponent<Battery>();
                                CrossExamine("Battery");
                                if (battery != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.BatteryOut * (battery.m_PowerOutput / 1000f),
                                        "BatteryOut"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.BatteryCap * (battery.m_Capacity / 1000f),
                                        "BatteryCap"
                                    );
                                }

                                Transformer transformer = prefabBase.GetComponent<Transformer>();
                                CrossExamine("Transformer");
                                if (transformer != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.Transformer,
                                        "Transformer"
                                    );
                                }

                                //Water & Sewage

                                WaterPumpingStation waterPumpingStation =
                                    prefabBase.GetComponent<WaterPumpingStation>();
                                CrossExamine("WaterPumpingStation");
                                if (waterPumpingStation != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.WaterPumpCap
                                            * (waterPumpingStation.m_Capacity / 1000f),
                                        "WaterPumpCap"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.Purification
                                            * waterPumpingStation.m_Purification,
                                        "WaterPurification"
                                    );
                                }

                                SewageOutlet sewageOutlet = prefabBase.GetComponent<SewageOutlet>();
                                CrossExamine("SewageOutlet");
                                if (sewageOutlet != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.SewageOutCap
                                            * (sewageOutlet.m_Capacity / 1000f),
                                        "SewageOutCap"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.Purification * sewageOutlet.m_Purification,
                                        "SewagePurification"
                                    );
                                }

                                //Healthcare & Deathcare
                                Hospital hospital = prefabBase.GetComponent<Hospital>();
                                CrossExamine("Hospital");
                                if (hospital != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.Ambulance * hospital.m_AmbulanceCapacity,
                                        "Ambulance"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.MedicalHelicopter
                                            * hospital.m_MedicalHelicopterCapacity,
                                        "MedicalHelicopter"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.Patient * hospital.m_PatientCapacity,
                                        "Patient"
                                    );
                                    //Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.HealthBonus * Hospital.m_TreatmentBonus, "HealthBonus");

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
                                        logX,
                                        valx,
                                        "HealthBonusRange"
                                    );
                                    //Adder(ref prevUpkeepValue, ref upkeepValue, logX, Hospital.m_TreatDiseases ? Mod.m_Setting.Treatment : 0f, "TreatDiseases");
                                    //Adder(ref prevUpkeepValue, ref upkeepValue, logX, Hospital.m_TreatInjuries ? Mod.m_Setting.Treatment : 0f, "TreatInjuries");
                                }

                                DeathcareFacility deathcareFacility =
                                    prefabBase.GetComponent<DeathcareFacility>();
                                CrossExamine("DeathcareFacility");
                                if (deathcareFacility != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.Hearse * deathcareFacility.m_HearseCapacity,
                                        "Hearse"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        (Mod.m_Setting.BodyStorage / 100f)
                                            * deathcareFacility.m_StorageCapacity
                                            * (deathcareFacility.m_LongTermStorage ? 1f : 0.3f),
                                        "BodyStorage"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.BodyProcessing
                                            * deathcareFacility.m_ProcessingRate,
                                        "BodyProcessing"
                                    );
                                }

                                // Garbage Management
                                GarbageFacility garbageFacility =
                                    prefabBase.GetComponent<GarbageFacility>();
                                CrossExamine("GarbageFacility");
                                if (garbageFacility != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.GarbageTruck
                                            * garbageFacility.m_VehicleCapacity,
                                        "GarbageTruck"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.DumpTruck
                                            * garbageFacility.m_TransportCapacity,
                                        "DumpTruck"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        (Mod.m_Setting.GarbageCap / 1000f)
                                            * garbageFacility.m_GarbageCapacity
                                            * (garbageFacility.m_LongTermStorage ? 1f : 0.3f)
                                            * (garbageFacility.m_IndustrialWasteOnly ? 0.1f : 1f),
                                        "GarbageCap"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        (Mod.m_Setting.GarbageProcessing / 1000f)
                                            * (garbageFacility.m_ProcessingSpeed / 1000f)
                                            * (garbageFacility.m_IndustrialWasteOnly ? 0.1f : 1f),
                                        "GarbageProcessing"
                                    );
                                }

                                // Education & Research
                                School school = prefabBase.GetComponent<School>();
                                CrossExamine("School");
                                if (school != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.Student * school.m_StudentCapacity,
                                        $"Student_{school.m_Level}"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.StudentGraduation
                                            * school.m_GraduationModifier
                                            * 100f,
                                        "StudentGraduation"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.StudentWellbeing * school.m_StudentWellbeing,
                                        "StudentWellbeing"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.StudentHealth * school.m_StudentHealth,
                                        "StudentHealth"
                                    );
                                }

                                ResearchFacility researchFacility =
                                    prefabBase.GetComponent<ResearchFacility>();
                                CrossExamine("ResearchFacility");
                                if (researchFacility != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.ResearchFacility,
                                        "ResearchFacility"
                                    );
                                }

                                // Fire & Rescue
                                FireStation fireStation = prefabBase.GetComponent<FireStation>();
                                CrossExamine("FireStation");
                                if (fireStation != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.FireTruck * fireStation.m_FireEngineCapacity,
                                        "FireTruck"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.FireHelicopter
                                            * fireStation.m_FireHelicopterCapacity,
                                        "FireHelicopter"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.FireDisasterCap
                                            * fireStation.m_DisasterResponseCapacity,
                                        "FireDisasterCap"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.FireVehicleEffi
                                            * fireStation.m_VehicleEfficiency,
                                        "FireVehicleEffi"
                                    );
                                }

                                FirewatchTower firewatchTower =
                                    prefabBase.GetComponent<FirewatchTower>();
                                CrossExamine("FirewatchTower");
                                if (firewatchTower != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.Firewatch,
                                        "Firewatch"
                                    );
                                }

                                EarlyDisasterWarningSystem earlyDisasterWarningSystem =
                                    prefabBase.GetComponent<EarlyDisasterWarningSystem>();
                                CrossExamine("EarlyDisasterWarningSystem");
                                if (earlyDisasterWarningSystem != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.EarlyDisasterWarningSystem,
                                        "EarlyDisasterWarningSystem"
                                    );
                                }

                                DisasterFacility disasterFacility =
                                    prefabBase.GetComponent<DisasterFacility>();
                                CrossExamine("DisasterFacility");
                                if (disasterFacility != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.DisasterFacility,
                                        "DisasterFacility"
                                    );
                                }

                                EmergencyShelter emergencyShelter =
                                    prefabBase.GetComponent<EmergencyShelter>();
                                CrossExamine("EmergencyShelter");
                                if (emergencyShelter != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.EvacuationBus
                                            * emergencyShelter.m_VehicleCapacity,
                                        "EvacuationBus"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.ShelterCap
                                            * emergencyShelter.m_ShelterCapacity
                                            / 1000f,
                                        "ShelterCap"
                                    );
                                }

                                EmergencyGenerator emergencyGenerator =
                                    prefabBase.GetComponent<EmergencyGenerator>();
                                CrossExamine("EmergencyGenerator");
                                if (emergencyGenerator != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.EmergencyGenerator
                                            * (emergencyGenerator.m_ElectricityProduction / 1000f),
                                        "EmergencyGenerator"
                                    );
                                }

                                // Police & Administration
                                PoliceStation policeStation =
                                    prefabBase.GetComponent<PoliceStation>();
                                CrossExamine("PoliceStation");
                                if (policeStation != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.PatrolCar * policeStation.m_PatrolCarCapacity,
                                        "PatrolCar"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.PoliceHelicopter
                                            * policeStation.m_PoliceHelicopterCapacity,
                                        "PoliceHelicopter"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.LocalJail * policeStation.m_JailCapacity,
                                        "LocalJail"
                                    );

                                    if (policeStation.m_Purposes.HasFlag(PolicePurpose.Patrol))
                                    {
                                        Adder(
                                            ref prevUpkeepValue,
                                            ref upkeepValue,
                                            logX,
                                            Mod.m_Setting.Patrol,
                                            "Patrol"
                                        );
                                    }
                                    if (policeStation.m_Purposes.HasFlag(PolicePurpose.Emergency))
                                    {
                                        Adder(
                                            ref prevUpkeepValue,
                                            ref upkeepValue,
                                            logX,
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
                                            logX,
                                            Mod.m_Setting.Intelligence,
                                            "Intelligence"
                                        );
                                    }
                                }

                                Prison prison = prefabBase.GetComponent<Prison>();
                                CrossExamine("Prison");
                                if (prison != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.PrisonVan * prison.m_PrisonVanCapacity,
                                        "PrisonVan"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.PrisonerCap * prison.m_PrisonerCapacity,
                                        "PrisonerCap"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.PrisonerWellbeing
                                            * prison.m_PrisonerWellbeing,
                                        "PrisonerWellbeing"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.PrisonerHealth * prison.m_PrisonerHealth,
                                        "PrisonerHealth"
                                    );
                                }

                                WelfareOffice welfareOffice =
                                    prefabBase.GetComponent<WelfareOffice>();
                                CrossExamine("WelfareOffice");
                                if (welfareOffice != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.WelfareOffice,
                                        "WelfareOffice"
                                    );
                                }

                                AdministrationBuilding administrationBuilding =
                                    prefabBase.GetComponent<AdministrationBuilding>();
                                CrossExamine("AdministrationBuilding");
                                if (administrationBuilding != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.AdminBuilding,
                                        "AdminBuilding"
                                    );
                                }

                                // Transportation
                                TransportDepot transportDepot =
                                    prefabBase.GetComponent<TransportDepot>();
                                CrossExamine("TransportDepot");
                                if (transportDepot != null)
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
                                        logX,
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
                                        logX,
                                        energyValue,
                                        $"EnergyTypes_{energyType}"
                                    );

                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
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
                                            logX,
                                            Mod.m_Setting.DispatchCenter,
                                            "DispatchCenter"
                                        );
                                    }
                                }

                                CargoTransportStation cargoTransportStation =
                                    prefabBase.GetComponent<CargoTransportStation>();
                                CrossExamine("CargoTransportStation");
                                if (cargoTransportStation != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.TradedResource
                                            * cargoTransportStation.m_TradedResources.Length,
                                        "TradedResource"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
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
                                            logX,
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
                                            logX,
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
                                            logX,
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
                                            logX,
                                            energyValue,
                                            $"AircraftRefuelTypes_{cargoTransportStation.m_AircraftRefuelTypes}"
                                        );
                                    }
                                }

                                TransportStation transportStation =
                                    prefabBase.GetComponent<TransportStation>();
                                CrossExamine("TransportStation");
                                if (transportStation != null)
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
                                            logX,
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
                                            logX,
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
                                            logX,
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
                                            logX,
                                            energyValue,
                                            $"AircraftRefuelTypes_{transportStation.m_AircraftRefuelTypes}"
                                        );
                                    }

                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.ComfortFactor
                                            * transportStation.m_ComfortFactor
                                            * 100f,
                                        "ComfortFactor"
                                    );
                                }

                                // Parks & Recreation
                                LeisureProvider leisureProvider =
                                    prefabBase.GetComponent<LeisureProvider>();
                                CrossExamine("LeisureProvider");
                                if (leisureProvider != null)
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
                                        logX,
                                        leisureMultiplier
                                            * (
                                                Mod.m_Setting.LeisureEfficieny
                                                * (leisureProvider.m_Efficiency / 100f)
                                            ),
                                        $"LeisureProvider_{leisureProvider.m_LeisureType}"
                                    );
                                }

                                Attraction attraction = prefabBase.GetComponent<Attraction>();
                                CrossExamine("Attraction");
                                if (attraction != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.Attraction * attraction.m_Attractiveness,
                                        "Attraction"
                                    );
                                }

                                // Communication
                                PostFacility postFacility = prefabBase.GetComponent<PostFacility>();
                                CrossExamine("PostFacility");
                                if (postFacility != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.PostVan * postFacility.m_PostVanCapacity,
                                        "PostVan"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.PostTruck * postFacility.m_PostTruckCapacity,
                                        "PostTruck"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
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
                                        logX,
                                        Mod.m_Setting.PostSortingRate
                                            * (postFacility.m_SortingRate / 1000f),
                                        "PostSortingRate"
                                    );
                                }

                                TelecomFacility telecomFacility =
                                    prefabBase.GetComponent<TelecomFacility>();
                                CrossExamine("TelecomFacility");
                                if (telecomFacility != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.TelecomRange
                                            * (telecomFacility.m_Range / 1000f),
                                        "TelecomRange"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.NetworkCap
                                            * (telecomFacility.m_NetworkCapacity / 1000f),
                                        "NetworkCap"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
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

                                ServiceCoverage serviceCoverage =
                                    prefabBase.GetComponent<ServiceCoverage>();
                                CrossExamine("ServiceCoverage");
                                if (serviceCoverage != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.ServiceCoverageMultiplier
                                            * serviceCoverage.m_Capacity
                                            * (
                                                serviceCoverage.m_Magnitude
                                                / serviceCoverage.m_Range
                                            ),
                                        "ServiceCoverage"
                                    );
                                }

                                Workplace workplace = prefabBase.GetComponent<Workplace>();
                                CrossExamine("Workplace");
                                if (workplace != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.EmployeeUpkeep * workplace.m_Workplaces,
                                        $"EmployeeUpkeep_{workplace.m_Complexity}"
                                    );
                                }

                                StorageLimit storageLimit = prefabBase.GetComponent<StorageLimit>();
                                CrossExamine("StorageLimit");
                                if (storageLimit != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.StorageUpkeep
                                            * (storageLimit.storageLimit / 100000f),
                                        "StorageLimit"
                                    );
                                }

                                Pollution pollution = prefabBase.GetComponent<Pollution>();
                                CrossExamine("Pollution");
                                if (pollution != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.GroundPollution
                                            * Math.Abs(pollution.m_GroundPollution / 1000f),
                                        "GroundPollution"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.AirPollution
                                            * Math.Abs(pollution.m_AirPollution / 1000f),
                                        "AirPollution"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.NoisePollution
                                            * Math.Abs(pollution.m_NoisePollution / 1000f),
                                        "NoisePollution"
                                    );
                                }

                                PollutionModifier pollutionModifier =
                                    prefabBase.GetComponent<PollutionModifier>();
                                CrossExamine("PollutionModifier");
                                if (pollutionModifier != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.GroundPollution
                                            * Math.Abs(
                                                pollutionModifier.m_GroundPollutionMultiplier * 10f
                                            ),
                                        "GroundPollutionMultiplier"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.AirPollution
                                            * Math.Abs(
                                                pollutionModifier.m_AirPollutionMultiplier * 10f
                                            ),
                                        "AirPollutionMultiplier"
                                    );
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        Mod.m_Setting.NoisePollution
                                            * Math.Abs(
                                                pollutionModifier.m_NoisePollutionMultiplier * 10f
                                            ),
                                        "NoisePollutionMultiplier"
                                    );
                                }

                                UniqueObject uniqueObject = prefabBase.GetComponent<UniqueObject>();
                                CrossExamine("UniqueObject");
                                if (uniqueObject != null)
                                {
                                    Multiplier(
                                        ref prevMultiplierValue,
                                        ref multiplier,
                                        logX,
                                        Mod.m_Setting.Uniqueness / 100f,
                                        "Uniqueness"
                                    );
                                }

                                ServiceObject serviceObject =
                                    prefabBase.GetComponent<ServiceObject>();
                                CrossExamine("ServiceObject");
                                if (serviceObject != null)
                                {
                                    foreach (var data in serviceBudgetData)
                                    {
                                        if (data.m_Service == serviceDict[serviceObject.m_Service])
                                        {
                                            Multiplier(
                                                ref prevMultiplierValue,
                                                ref multiplier,
                                                logX,
                                                data.m_Budget / 100f,
                                                "ServiceObject"
                                            );
                                            break;
                                        }
                                    }
                                }

                                ServiceUpgrade serviceUpgrade =
                                    prefabBase.GetComponent<ServiceUpgrade>();
                                CrossExamine("ServiceUpgrade");
                                if (serviceUpgrade != null)
                                {
                                    Adder(
                                        ref prevUpkeepValue,
                                        ref upkeepValue,
                                        logX,
                                        serviceUpgrade.m_UpgradeCost * 0.05f,
                                        "ServiceUpgrade"
                                    );
                                }

                                //CityEffects CityEffects = prefabBase.GetComponent<CityEffects>();
                                //CrossExamine("CityEffects");
                                //if (CityEffects != null && Mod.m_Setting.CityBonusMultiplier)
                                //{
                                //    foreach (var data in CityEffects.m_Effects)
                                //    {
                                //        if (data.m_Mode.HasFlag(ModifierValueMode.Relative) || data.m_Mode.HasFlag(ModifierValueMode.InverseRelative))
                                //        {
                                //            Multiplier(ref prevMultiplierValue, ref multiplier, logX, Math.Abs(data.m_Delta), $"CityEffects_{data.m_Mode}");
                                //        }
                                //    }
                                //}

                                //LocalEffects LocalEffects = prefabBase.GetComponent<LocalEffects>();
                                //CrossExamine("LocalEffects");
                                //if (LocalEffects != null && Mod.m_Setting.CityBonusMultiplier)
                                //{
                                //    foreach (var data in LocalEffects.m_Effects)
                                //    {
                                //        if (data.m_Mode.HasFlag(ModifierValueMode.Relative) || data.m_Mode.HasFlag(ModifierValueMode.InverseRelative))
                                //        {
                                //            Multiplier(ref prevMultiplierValue, ref multiplier, logX, Math.Abs(data.m_Delta), $"LocalEffects_{data.m_Mode}");
                                //        }
                                //        //if (data.m_Mode == ModifierValueMode.Absolute)
                                //        //{
                                //        //    Multiplier(ref prevMultiplierValue, ref multiplier, logX, Math.Abs(data.m_Delta - 100f), "LocalEffects.Absolute");
                                //        //}
                                //    }
                                //}

                                float height = 5;
                                if (
                                    EntityManager.TryGetComponent<ObjectGeometryData>(
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
                                            logX,
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
                                        logX,
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
                                    if (log)
                                    {
                                        string logText =
                                            $"{name};{ogUpkeep[name]};{upkeepValueInt}";
                                        logText = $"{logText};multiplier={multiplier}";
                                        foreach (var vals in prevVal)
                                        {
                                            logText = $"{logText};{vals.Key}={vals.Value}";
                                        }
                                        Mod.log.Info(logText);
                                    }
                                }
                                else
                                {
                                    Mod.log.Info(
                                        $"Something went wrong: {name}'s upkeep is ${upkeepValue}?"
                                    );
                                    if (log)
                                    {
                                        string logText =
                                            $"{name};{ogUpkeep[name]};{upkeepValueInt}";
                                        logText = $"{logText};multiplier={multiplier}";
                                        foreach (var vals in prevVal)
                                        {
                                            logText = $"{logText};{vals.Key}={vals.Value}";
                                        }
                                        Mod.log.Info(logText);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Mod.log.Error(e);
                }
            }
        }

        public void ResetToVanilla()
        {
            foreach (var item in buildingDict)
            {
                try
                {
                    string name = item.Key;
                    Entity entity = item.Value;

                    if (
                        EntityManager.TryGetComponent(entity, out PrefabData prefabData)
                        && prefabSystem.TryGetPrefab(prefabData, out PrefabBase prefabBase)
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
                                consumptionData.m_Upkeep = ogUpkeep[name];
                                EntityManager.SetComponentData(entity, consumptionData);
                                RefreshBuffer(entity, ogUpkeep[name]);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Mod.log.Error(e);
                }
            }
        }

        float SegmentedWeight(float input, float N)
        {
            if (input < 25)
            {
                float t = input / 25;
                return t * N;
            }
            else if (input < 50)
            {
                float t = (input - 25) / 25;
                return N + (t * 2f * N);
            }
            else if (input < 75)
            {
                float t = (input - 50) / 25;
                return N + (2f * N) + (t * 3f * N);
            }
            else
            {
                float t = (input - 75) / 25;
                return N + (2f * N) + (3f * N) + (t * 4f * N);
            }
        }

        float ComputeCombinedValue(float2 healthRange, bool disease, bool injuries)
        {
            float x = healthRange.x;
            float y = healthRange.y;
            float N = Mod.m_Setting.HealthRange;

            float weightedX = SegmentedWeight(x, N);
            float weightedY = SegmentedWeight(y, N);

            float extra = 0;
            if (disease || injuries)
            {
                extra = Mod.m_Setting.Treatment;
            }
            //Mod.log.Info($"HealthRange ({x} to {y} = Math.Abs({weightedX} - {weightedY}))");
            return ((Math.Abs(weightedX - weightedY) / N) / 100f) + extra;
        }

        void Adder(
            ref float prevUpkeepValue,
            ref float upkeepValue,
            bool logX,
            float adder,
            string text
        )
        {
            prevUpkeepValue = upkeepValue;
            upkeepValue += (float)adder;
            var diff = upkeepValue - prevUpkeepValue;
            if (!prevVal.ContainsKey(text) && diff != 0f)
            {
                prevVal[text] = diff;
            }
            //if (logX && diff != 0) Mod.log.Info($"{diff} for {text}");
        }

        void Multiplier(
            ref float prevMultiplierValue,
            ref float multipler,
            bool logX,
            float adder,
            string text
        )
        {
            prevMultiplierValue = multipler;
            multipler += (float)adder;
            var diff = multipler - prevMultiplierValue;
            if (!prevVal.ContainsKey($"{text}_x") && diff != 0f)
            {
                prevVal[$"{text}_x"] = diff;
            }
            //if (logX) Mod.log.Info($"{multipler - prevMultiplierValue}x for {text}");
        }

        void CrossExamine(string text)
        {
            //if (!toRemove.Contains(text)) toRemove.Add(text);
        }
    }
}
