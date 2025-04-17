using Colossal.Entities;
using Game.Prefabs;
using Game;
using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using System.Runtime.Remoting.Lifetime;

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
            EntityQuery serviceBudgetQuery = SystemAPI.QueryBuilder().WithAll<Game.Simulation.ServiceBudgetData>().Build();
            Entity serviceBudgetEntity = serviceBudgetQuery.ToEntityArray(Allocator.Temp)[0];
            EntityManager.TryGetBuffer(serviceBudgetEntity, true, out DynamicBuffer<Game.Simulation.ServiceBudgetData> serviceBudgetData);

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

                    if (EntityManager.TryGetComponent(entity, out PrefabData prefabData) && prefabSystem.TryGetPrefab(prefabData, out PrefabBase prefabBase))
                    {

                        if (EntityManager.TryGetComponent(entity, out ConsumptionData consumptionData))
                        {

                            if (ogUpkeep.ContainsKey(name) && ogUpkeep[name] != 0)
                            {
                                // Mixed
                                MaintenanceDepot MaintenanceDepot = prefabBase.GetComponent<MaintenanceDepot>();
                                CrossExamine("MaintenanceDepot");
                                if (MaintenanceDepot != null)
                                {
                                    if (MaintenanceDepot.m_MaintenanceType.HasFlag(Game.Simulation.MaintenanceType.Road))
                                    {
                                        Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.RoadMaintenance, "RoadMaintenance");
                                    }
                                    if (MaintenanceDepot.m_MaintenanceType.HasFlag(Game.Simulation.MaintenanceType.Snow))
                                    {
                                        Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.SnowPloughing, "SnowPloughing");
                                    }
                                    if (MaintenanceDepot.m_MaintenanceType.HasFlag(Game.Simulation.MaintenanceType.Park))
                                    {
                                        Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.ParkMaintenance, "ParkMaintenance");
                                        Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.ParkMaintenanceVehicle * MaintenanceDepot.m_VehicleCapacity, "ParkMaintenanceVehicle");
                                    }
                                    if (MaintenanceDepot.m_MaintenanceType.HasFlag(Game.Simulation.MaintenanceType.Vehicle))
                                    {
                                        Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.Towing, "Towing");
                                    }
                                    if (MaintenanceDepot.m_MaintenanceType.HasFlag(Game.Simulation.MaintenanceType.Snow) || MaintenanceDepot.m_MaintenanceType.HasFlag(Game.Simulation.MaintenanceType.Road) || MaintenanceDepot.m_MaintenanceType.HasFlag(Game.Simulation.MaintenanceType.Vehicle))
                                    {
                                        Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.RoadMaintenanceVehicle * MaintenanceDepot.m_VehicleCapacity, "RoadMaintenanceVehicle");
                                    }
                                }

                                ObjectSubObjects ObjectSubObjects = prefabBase.GetComponent<ObjectSubObjects>();
                                CrossExamine("ObjectSubObjects");
                                if (ObjectSubObjects != null)
                                {
                                    int parkingSpaces = 0;
                                    int platforms = 0;
                                    for (int i = 0; i < ObjectSubObjects.m_SubObjects.Length; i++)
                                    {
                                        ObjectSubObjectInfo ObjectSubObjectInfo = ObjectSubObjects.m_SubObjects[i];
                                        string objName = ObjectSubObjectInfo.m_Object.name;
                                        if (!objName.StartsWith("ParkingLot") && !objName.Contains("Decal0")) continue;
                                        if (objName == "ParkingLotDecal01" || objName == "ParkingLotDiagonalDecal01" || objName == "ParkingLotDisabledDecal01" || objName == "ParkingLotElectricDecal01" || objName == "ParkingLotServiceDecal01") { parkingSpaces++; }
                                        else if (objName == "ParkingLotDoubleDecal01" || objName == "ParkingLotDoubleDecal02" || objName == "ParkingLotDoubleServiceDecal01" || objName == "ParkingLotDoubleServiceDecal02") { parkingSpaces += 2; }
                                        else if (objName == "ParkingLotDecal02" || objName == "ParkingLotDiagonalDecal02" || objName == "ParkingLotDisabledDecal02" || objName == "ParkingLotElectricDecal02" || objName == "ParkingLotServiceDecal02") { parkingSpaces += 3; }
                                        else if (objName == "ParkingLotDecal03" || objName == "ParkingLotDiagonalDecal03" || objName == "ParkingLotServiceDecal03") { parkingSpaces += 5; }
                                        else if (objName == "ParkingLotDecal04" || objName == "ParkingLotServiceDecal04") { parkingSpaces += 10; }
                                        else if (objName.StartsWith("ParkingLot") && objName.Contains("Decal0"))
                                        {
                                            parkingSpaces++;
                                        }
                                        else if (objName.StartsWith("Integrated"))
                                        {
                                            platforms++;
                                        }
                                    }

                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.ParkingSpots * parkingSpaces, "ParkingSpots");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.PlatformMaintenance * platforms, "Platforms");
                                }

                                // Roads
                                ParkingFacility ParkingFacility = prefabBase.GetComponent<ParkingFacility>();
                                CrossExamine("ParkingFacility");
                                if (ParkingFacility != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.ParkingSpots * ParkingFacility.m_GarageMarkerCapacity, "GarageMarker");
                                }

                                // Electricity
                                SolarPowered SolarPowered = prefabBase.GetComponent<SolarPowered>();
                                CrossExamine("SolarPowered");
                                if (SolarPowered != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.SolarPowered * (SolarPowered.m_Production / 1000f), "SolarPowered");
                                }

                                GroundWaterPowered GroundWaterPowered = prefabBase.GetComponent<GroundWaterPowered>();
                                CrossExamine("GroundWaterPowered");
                                if (GroundWaterPowered != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.GroundWaterPowered * (GroundWaterPowered.m_Production / 1000f), "GroundWaterPowered");
                                }

                                WaterPowered WaterPowered = prefabBase.GetComponent<WaterPowered>();
                                CrossExamine("WaterPowered");
                                if (WaterPowered != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.WaterPowered * (WaterPowered.m_CapacityFactor * WaterPowered.m_ProductionFactor) * 10, "WaterPowered");
                                }

                                WindPowered WindPowered = prefabBase.GetComponent<WindPowered>();
                                CrossExamine("WindPowered");
                                if (WindPowered != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.WindPowered * (WindPowered.m_Production / 1000f), "WindPowered");
                                }

                                GarbagePowered GarbagePowered = prefabBase.GetComponent<GarbagePowered>();
                                CrossExamine("GarbagePowered");
                                if (GarbagePowered != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.GarbagePowered * GarbagePowered.m_ProductionPerUnit * (GarbagePowered.m_Capacity / 1000f), "GarbagePowered");
                                }

                                PowerPlant PowerPlant = prefabBase.GetComponent<PowerPlant>();
                                CrossExamine("PowerPlant");
                                if (PowerPlant != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.ElectricityProduction * (PowerPlant.m_ElectricityProduction / 1000f), "ElectricityProduction");
                                }

                                Battery Battery = prefabBase.GetComponent<Battery>();
                                CrossExamine("Battery");
                                if (Battery != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.BatteryOut * (Battery.m_PowerOutput / 1000f), "BatteryOut");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.BatteryCap * (Battery.m_Capacity / 1000f), "BatteryCap");
                                }

                                Transformer Transformer = prefabBase.GetComponent<Transformer>();
                                CrossExamine("Transformer");
                                if (Transformer != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.Transformer, "Transformer");
                                }

                                //Water & Sewage

                                WaterPumpingStation WaterPumpingStation = prefabBase.GetComponent<WaterPumpingStation>();
                                CrossExamine("WaterPumpingStation");
                                if (WaterPumpingStation != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.WaterPumpCap * (WaterPumpingStation.m_Capacity / 1000f), "WaterPumpCap");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.Purification * WaterPumpingStation.m_Purification, "WaterPurification");
                                }

                                SewageOutlet SewageOutlet = prefabBase.GetComponent<SewageOutlet>();
                                CrossExamine("SewageOutlet");
                                if (SewageOutlet != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.SewageOutCap * (SewageOutlet.m_Capacity / 1000f), "SewageOutCap");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.Purification * SewageOutlet.m_Purification, "SewagePurification");
                                }

                                //Healthcare & Deathcare
                                Hospital Hospital = prefabBase.GetComponent<Hospital>();
                                CrossExamine("Hospital");
                                if (Hospital != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.Ambulance * Hospital.m_AmbulanceCapacity, "Ambulance");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.MedicalHelicopter * Hospital.m_MedicalHelicopterCapacity, "MedicalHelicopter");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.Patient * Hospital.m_PatientCapacity, "Patient");
                                    //Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.HealthBonus * Hospital.m_TreatmentBonus, "HealthBonus");

                                    float weightedX = SegmentedWeight(Hospital.m_HealthRange.x, Mod.m_Setting.HealthRange);
                                    float weightedY = SegmentedWeight(Hospital.m_HealthRange.y, Mod.m_Setting.HealthRange);

                                    float extra = 0;
                                    if (Hospital.m_TreatDiseases || Hospital.m_TreatInjuries)
                                    {
                                        if (Hospital.m_PatientCapacity > 0)
                                        {
                                            extra = (Mod.m_Setting.Treatment / Hospital.m_PatientCapacity) * (Hospital.m_TreatmentBonus / Hospital.m_PatientCapacity);
                                        } else
                                        {
                                            extra = Mod.m_Setting.Treatment;
                                        }
                                    } else
                                    {
                                        if (Hospital.m_TreatmentBonus > 0)
                                        {
                                            extra = Mod.m_Setting.HealthBonus;
                                        }
                                    }
                                    var valx = ((Math.Abs(weightedX - weightedY) / Mod.m_Setting.HealthRange) / 100f) * extra;
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, valx, "HealthBonusRange");
                                    //Adder(ref prevUpkeepValue, ref upkeepValue, logX, Hospital.m_TreatDiseases ? Mod.m_Setting.Treatment : 0f, "TreatDiseases");
                                    //Adder(ref prevUpkeepValue, ref upkeepValue, logX, Hospital.m_TreatInjuries ? Mod.m_Setting.Treatment : 0f, "TreatInjuries");
                                }

                                DeathcareFacility DeathcareFacility = prefabBase.GetComponent<DeathcareFacility>();
                                CrossExamine("DeathcareFacility");
                                if (DeathcareFacility != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.Hearse * DeathcareFacility.m_HearseCapacity, "Hearse");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, (Mod.m_Setting.BodyStorage / 1000f) * DeathcareFacility.m_StorageCapacity * (DeathcareFacility.m_LongTermStorage ? 1f : 0.3f), "BodyStorage");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.BodyProcessing * DeathcareFacility.m_ProcessingRate, "BodyProcessing");
                                }

                                // Garbage Management
                                GarbageFacility GarbageFacility = prefabBase.GetComponent<GarbageFacility>();
                                CrossExamine("GarbageFacility");
                                if (GarbageFacility != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.GarbageTruck * GarbageFacility.m_VehicleCapacity, "GarbageTruck");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.DumpTruck * GarbageFacility.m_TransportCapacity, "DumpTruck");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, (Mod.m_Setting.GarbageCap / 1000f) * GarbageFacility.m_GarbageCapacity * (GarbageFacility.m_LongTermStorage ? 1f : 0.3f) * (GarbageFacility.m_IndustrialWasteOnly ? 0.1f : 1f), "GarbageCap");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, (Mod.m_Setting.GarbageProcessing / 1000f) * (GarbageFacility.m_ProcessingSpeed / 1000f) * (GarbageFacility.m_IndustrialWasteOnly ? 0.1f : 1f), "GarbageProcessing");
                                }

                                // Education & Research
                                School School = prefabBase.GetComponent<School>();
                                CrossExamine("School");
                                if (School != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.Student * School.m_StudentCapacity, $"Student_{School.m_Level}");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.StudentGraduation * School.m_GraduationModifier * 100f, "StudentGraduation");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.StudentWellbeing * School.m_StudentWellbeing, "StudentWellbeing");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.StudentHealth * School.m_StudentHealth, "StudentHealth");
                                }

                                ResearchFacility ResearchFacility = prefabBase.GetComponent<ResearchFacility>();
                                CrossExamine("ResearchFacility");
                                if (ResearchFacility != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.ResearchFacility, "ResearchFacility");
                                }

                                // Fire & Rescue
                                FireStation FireStation = prefabBase.GetComponent<FireStation>();
                                CrossExamine("FireStation");
                                if (FireStation != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.FireTruck * FireStation.m_FireEngineCapacity, "FireTruck");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.FireHelicopter * FireStation.m_FireHelicopterCapacity, "FireHelicopter");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.FireDisasterCap * FireStation.m_DisasterResponseCapacity, "FireDisasterCap");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.FireVehicleEffi * FireStation.m_VehicleEfficiency, "FireVehicleEffi");
                                }

                                FirewatchTower FirewatchTower = prefabBase.GetComponent<FirewatchTower>();
                                CrossExamine("FirewatchTower");
                                if (FirewatchTower != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.Firewatch, "Firewatch");
                                }

                                EarlyDisasterWarningSystem EarlyDisasterWarningSystem = prefabBase.GetComponent<EarlyDisasterWarningSystem>();
                                CrossExamine("EarlyDisasterWarningSystem");
                                if (EarlyDisasterWarningSystem != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.EarlyDisasterWarningSystem, "EarlyDisasterWarningSystem");
                                }

                                DisasterFacility DisasterFacility = prefabBase.GetComponent<DisasterFacility>();
                                CrossExamine("DisasterFacility");
                                if (DisasterFacility != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.DisasterFacility, "DisasterFacility");
                                }

                                EmergencyShelter EmergencyShelter = prefabBase.GetComponent<EmergencyShelter>();
                                CrossExamine("EmergencyShelter");
                                if (EmergencyShelter != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.EvacuationBus * EmergencyShelter.m_VehicleCapacity, "EvacuationBus");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.ShelterCap * EmergencyShelter.m_ShelterCapacity / 1000f, "ShelterCap");
                                }

                                EmergencyGenerator EmergencyGenerator = prefabBase.GetComponent<EmergencyGenerator>();
                                CrossExamine("EmergencyGenerator");
                                if (EmergencyGenerator != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.EmergencyGenerator * (EmergencyGenerator.m_ElectricityProduction / 1000f), "EmergencyGenerator");
                                }

                                // Police & Administration
                                PoliceStation PoliceStation = prefabBase.GetComponent<PoliceStation>();
                                CrossExamine("PoliceStation");
                                if (PoliceStation != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.PatrolCar * PoliceStation.m_PatrolCarCapacity, "PatrolCar");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.PoliceHelicopter * PoliceStation.m_PoliceHelicopterCapacity, "PoliceHelicopter");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.LocalJail * PoliceStation.m_JailCapacity, "LocalJail");

                                    if (PoliceStation.m_Purposes.HasFlag(PolicePurpose.Patrol))
                                    {
                                        Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.Patrol, "Patrol");
                                    }
                                    if (PoliceStation.m_Purposes.HasFlag(PolicePurpose.Emergency))
                                    {
                                        Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.EmergencyPolice, "Emergency");
                                    }
                                    if (PoliceStation.m_Purposes.HasFlag(PolicePurpose.Intelligence))
                                    {
                                        Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.Intelligence, "Intelligence");
                                    }
                                }

                                Prison Prison = prefabBase.GetComponent<Prison>();
                                CrossExamine("Prison");
                                if (Prison != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.PrisonVan * Prison.m_PrisonVanCapacity, "PrisonVan");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.PrisonerCap * Prison.m_PrisonerCapacity, "PrisonerCap");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.PrisonerWellbeing * Prison.m_PrisonerWellbeing, "PrisonerWellbeing");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.PrisonerHealth * Prison.m_PrisonerHealth, "PrisonerHealth");
                                }

                                WelfareOffice WelfareOffice = prefabBase.GetComponent<WelfareOffice>();
                                CrossExamine("WelfareOffice");
                                if (WelfareOffice != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.WelfareOffice, "WelfareOffice");
                                }

                                AdministrationBuilding AdministrationBuilding = prefabBase.GetComponent<AdministrationBuilding>();
                                CrossExamine("AdministrationBuilding");
                                if (AdministrationBuilding != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.AdminBuilding, "AdminBuilding");
                                }

                                // Transportation
                                TransportDepot TransportDepot = prefabBase.GetComponent<TransportDepot>();
                                CrossExamine("TransportDepot");
                                if (TransportDepot != null)
                                {
                                    TransportType transportType = TransportDepot.m_TransportType;
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
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, transportMuliplier * TransportDepot.m_VehicleCapacity, $"TransportType_{transportType}");

                                    Game.Vehicles.EnergyTypes energyType = TransportDepot.m_EnergyTypes;
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
                                            energyValue = Mod.m_Setting.EnergyFuel + Mod.m_Setting.EnergyElectricity;
                                            break;
                                    }
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, energyValue, $"EnergyTypes_{energyType}");

                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Math.Max(Mod.m_Setting.MaintenanceBoost, Mod.m_Setting.MaintenanceBoost * (TransportDepot.m_MaintenanceDuration * 100f)), "MaintenanceBoost");

                                    if (TransportDepot.m_DispatchCenter)
                                    {
                                        Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.DispatchCenter, "DispatchCenter");
                                    }
                                }

                                CargoTransportStation CargoTransportStation = prefabBase.GetComponent<CargoTransportStation>();
                                CrossExamine("CargoTransportStation");
                                if (CargoTransportStation != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.TradedResource * CargoTransportStation.m_TradedResources.Length, "TradedResource");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.DeliveryTruck * CargoTransportStation.transports, "DeliveryTruck");

                                    if (!CargoTransportStation.m_CarRefuelTypes.HasFlag(Game.Vehicles.EnergyTypes.None))
                                    {
                                        float energyValue = 0f;
                                        switch (CargoTransportStation.m_CarRefuelTypes)
                                        {
                                            case Game.Vehicles.EnergyTypes.Fuel:
                                                energyValue = Mod.m_Setting.EnergyFuel;
                                                break;
                                            case Game.Vehicles.EnergyTypes.Electricity:
                                                energyValue = Mod.m_Setting.EnergyElectricity;
                                                break;
                                            case Game.Vehicles.EnergyTypes.FuelAndElectricity:
                                                energyValue = Mod.m_Setting.EnergyFuel + Mod.m_Setting.EnergyElectricity;
                                                break;
                                        }
                                        Adder(ref prevUpkeepValue, ref upkeepValue, logX, energyValue, $"CarRefuelTypes_{CargoTransportStation.m_CarRefuelTypes}");
                                    }

                                    if (!CargoTransportStation.m_TrainRefuelTypes.HasFlag(Game.Vehicles.EnergyTypes.None))
                                    {
                                        float energyValue = 0f;
                                        switch (CargoTransportStation.m_TrainRefuelTypes)
                                        {
                                            case Game.Vehicles.EnergyTypes.Fuel:
                                                energyValue = Mod.m_Setting.EnergyFuel;
                                                break;
                                            case Game.Vehicles.EnergyTypes.Electricity:
                                                energyValue = Mod.m_Setting.EnergyElectricity;
                                                break;
                                            case Game.Vehicles.EnergyTypes.FuelAndElectricity:
                                                energyValue = Mod.m_Setting.EnergyFuel + Mod.m_Setting.EnergyElectricity;
                                                break;
                                        }
                                        Adder(ref prevUpkeepValue, ref upkeepValue, logX, energyValue, $"TrainRefuelTypes_{CargoTransportStation.m_TrainRefuelTypes}");
                                    }

                                    if (!CargoTransportStation.m_WatercraftRefuelTypes.HasFlag(Game.Vehicles.EnergyTypes.None))
                                    {
                                        float energyValue = 0f;
                                        switch (CargoTransportStation.m_WatercraftRefuelTypes)
                                        {
                                            case Game.Vehicles.EnergyTypes.Fuel:
                                                energyValue = Mod.m_Setting.EnergyFuel;
                                                break;
                                            case Game.Vehicles.EnergyTypes.Electricity:
                                                energyValue = Mod.m_Setting.EnergyElectricity;
                                                break;
                                            case Game.Vehicles.EnergyTypes.FuelAndElectricity:
                                                energyValue = Mod.m_Setting.EnergyFuel + Mod.m_Setting.EnergyElectricity;
                                                break;
                                        }
                                        Adder(ref prevUpkeepValue, ref upkeepValue, logX, energyValue, $"WatercraftRefuel_{CargoTransportStation.m_WatercraftRefuelTypes}");
                                    }

                                    if (!CargoTransportStation.m_AircraftRefuelTypes.HasFlag(Game.Vehicles.EnergyTypes.None))
                                    {
                                        float energyValue = 0f;
                                        switch (CargoTransportStation.m_AircraftRefuelTypes)
                                        {
                                            case Game.Vehicles.EnergyTypes.Fuel:
                                                energyValue = Mod.m_Setting.EnergyFuel;
                                                break;
                                            case Game.Vehicles.EnergyTypes.Electricity:
                                                energyValue = Mod.m_Setting.EnergyElectricity;
                                                break;
                                            case Game.Vehicles.EnergyTypes.FuelAndElectricity:
                                                energyValue = Mod.m_Setting.EnergyFuel + Mod.m_Setting.EnergyElectricity;
                                                break;
                                        }
                                        Adder(ref prevUpkeepValue, ref upkeepValue, logX, energyValue, $"AircraftRefuelTypes_{CargoTransportStation.m_AircraftRefuelTypes}");
                                    }
                                }

                                TransportStation TransportStation = prefabBase.GetComponent<TransportStation>();
                                CrossExamine("TransportStation");
                                if (TransportStation != null)
                                {
                                    if (!TransportStation.m_CarRefuelTypes.HasFlag(Game.Vehicles.EnergyTypes.None))
                                    {
                                        float energyValue = 0f;
                                        switch (TransportStation.m_CarRefuelTypes)
                                        {
                                            case Game.Vehicles.EnergyTypes.Fuel:
                                                energyValue = Mod.m_Setting.EnergyFuel;
                                                break;
                                            case Game.Vehicles.EnergyTypes.Electricity:
                                                energyValue = Mod.m_Setting.EnergyElectricity;
                                                break;
                                            case Game.Vehicles.EnergyTypes.FuelAndElectricity:
                                                energyValue = Mod.m_Setting.EnergyFuel + Mod.m_Setting.EnergyElectricity;
                                                break;
                                        }
                                        Adder(ref prevUpkeepValue, ref upkeepValue, logX, energyValue, $"CarRefuelTypes_{TransportStation.m_CarRefuelTypes}");
                                    }

                                    if (!TransportStation.m_TrainRefuelTypes.HasFlag(Game.Vehicles.EnergyTypes.None))
                                    {
                                        float energyValue = 0f;
                                        switch (TransportStation.m_TrainRefuelTypes)
                                        {
                                            case Game.Vehicles.EnergyTypes.Fuel:
                                                energyValue = Mod.m_Setting.EnergyFuel;
                                                break;
                                            case Game.Vehicles.EnergyTypes.Electricity:
                                                energyValue = Mod.m_Setting.EnergyElectricity;
                                                break;
                                            case Game.Vehicles.EnergyTypes.FuelAndElectricity:
                                                energyValue = Mod.m_Setting.EnergyFuel + Mod.m_Setting.EnergyElectricity;
                                                break;
                                        }
                                        Adder(ref prevUpkeepValue, ref upkeepValue, logX, energyValue, $"TrainRefuelTypes_{TransportStation.m_TrainRefuelTypes}");
                                    }

                                    if (!TransportStation.m_WatercraftRefuelTypes.HasFlag(Game.Vehicles.EnergyTypes.None))
                                    {
                                        float energyValue = 0f;
                                        switch (TransportStation.m_WatercraftRefuelTypes)
                                        {
                                            case Game.Vehicles.EnergyTypes.Fuel:
                                                energyValue = Mod.m_Setting.EnergyFuel;
                                                break;
                                            case Game.Vehicles.EnergyTypes.Electricity:
                                                energyValue = Mod.m_Setting.EnergyElectricity;
                                                break;
                                            case Game.Vehicles.EnergyTypes.FuelAndElectricity:
                                                energyValue = Mod.m_Setting.EnergyFuel + Mod.m_Setting.EnergyElectricity;
                                                break;
                                        }
                                        Adder(ref prevUpkeepValue, ref upkeepValue, logX, energyValue, $"WatercraftRefuel_{TransportStation.m_WatercraftRefuelTypes}");
                                    }

                                    if (!TransportStation.m_AircraftRefuelTypes.HasFlag(Game.Vehicles.EnergyTypes.None))
                                    {
                                        float energyValue = 0f;
                                        switch (TransportStation.m_AircraftRefuelTypes)
                                        {
                                            case Game.Vehicles.EnergyTypes.Fuel:
                                                energyValue = Mod.m_Setting.EnergyFuel;
                                                break;
                                            case Game.Vehicles.EnergyTypes.Electricity:
                                                energyValue = Mod.m_Setting.EnergyElectricity;
                                                break;
                                            case Game.Vehicles.EnergyTypes.FuelAndElectricity:
                                                energyValue = Mod.m_Setting.EnergyFuel + Mod.m_Setting.EnergyElectricity;
                                                break;
                                        }
                                        Adder(ref prevUpkeepValue, ref upkeepValue, logX, energyValue, $"AircraftRefuelTypes_{TransportStation.m_AircraftRefuelTypes}");
                                    }

                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.ComfortFactor * TransportStation.m_ComfortFactor * 100f, "ComfortFactor");
                                }

                                // Parks & Recreation
                                LeisureProvider LeisureProvider = prefabBase.GetComponent<LeisureProvider>();
                                CrossExamine("LeisureProvider");
                                if (LeisureProvider != null)
                                {
                                    Game.Agents.LeisureType leisureType = LeisureProvider.m_LeisureType;
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
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, leisureMultiplier * (Mod.m_Setting.LeisureEfficieny * (LeisureProvider.m_Efficiency / 100f)), $"LeisureProvider_{LeisureProvider.m_LeisureType}");
                                }

                                Attraction Attraction = prefabBase.GetComponent<Attraction>();
                                CrossExamine("Attraction");
                                if (Attraction != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.Attraction * Attraction.m_Attractiveness, "Attraction");
                                }

                                // Communication
                                PostFacility PostFacility = prefabBase.GetComponent<PostFacility>();
                                CrossExamine("PostFacility");
                                if (PostFacility != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.PostVan * PostFacility.m_PostVanCapacity, "PostVan");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.PostTruck * PostFacility.m_PostTruckCapacity, "PostTruck");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.MailCap * ((PostFacility.m_MailStorageCapacity + PostFacility.m_MailBoxCapacity) / 1000f), "MailCap");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.PostSortingRate * (PostFacility.m_SortingRate / 1000f), "PostSortingRate");
                                }

                                TelecomFacility TelecomFacility = prefabBase.GetComponent<TelecomFacility>();
                                CrossExamine("TelecomFacility");
                                if (TelecomFacility != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.TelecomRange * (TelecomFacility.m_Range / 1000f), "TelecomRange");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.NetworkCap * (TelecomFacility.m_NetworkCapacity / 1000f), "NetworkCap");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Unity.Mathematics.math.select(Mod.m_Setting.Fibre, Mod.m_Setting.Wireless, TelecomFacility.m_PenetrateTerrain), $"Fibre {TelecomFacility.m_PenetrateTerrain}");
                                }

                                // Landscaping

                                // General

                                ServiceCoverage ServiceCoverage = prefabBase.GetComponent<ServiceCoverage>();
                                CrossExamine("ServiceCoverage");
                                if (ServiceCoverage != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.ServiceCoverageMultiplier * ServiceCoverage.m_Capacity * (ServiceCoverage.m_Magnitude / ServiceCoverage.m_Range), "ServiceCoverage");
                                }

                                Workplace Workplace = prefabBase.GetComponent<Workplace>();
                                CrossExamine("Workplace");
                                if (Workplace != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.EmployeeUpkeep * Workplace.m_Workplaces, $"EmployeeUpkeep_{Workplace.m_Complexity}");
                                }

                                StorageLimit StorageLimit = prefabBase.GetComponent<StorageLimit>();
                                CrossExamine("StorageLimit");
                                if (StorageLimit != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.StorageUpkeep * (StorageLimit.storageLimit / 1000f), "StorageLimit");
                                }

                                Pollution Pollution = prefabBase.GetComponent<Pollution>();
                                CrossExamine("Pollution");
                                if (Pollution != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.GroundPollution * Math.Abs(Pollution.m_GroundPollution / 1000f), "GroundPollution");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.AirPollution * Math.Abs(Pollution.m_AirPollution / 1000f), "AirPollution");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.NoisePollution * Math.Abs(Pollution.m_NoisePollution / 1000f), "NoisePollution");
                                }

                                PollutionModifier PollutionModifier = prefabBase.GetComponent<PollutionModifier>();
                                CrossExamine("PollutionModifier");
                                if (PollutionModifier != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.GroundPollution * Math.Abs(PollutionModifier.m_GroundPollutionMultiplier * 10f), "GroundPollutionMultiplier");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.AirPollution * Math.Abs(PollutionModifier.m_AirPollutionMultiplier * 10f), "AirPollutionMultiplier");
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, Mod.m_Setting.NoisePollution * Math.Abs(PollutionModifier.m_NoisePollutionMultiplier * 10f), "NoisePollutionMultiplier");
                                }

                                UniqueObject UniqueObject = prefabBase.GetComponent<UniqueObject>();
                                CrossExamine("UniqueObject");
                                if (UniqueObject != null)
                                {
                                    Multiplier(ref prevMultiplierValue, ref multiplier, logX, Mod.m_Setting.Uniqueness / 100f, "Uniqueness");
                                }

                                ServiceObject ServiceObject = prefabBase.GetComponent<ServiceObject>();
                                CrossExamine("ServiceObject");
                                if (ServiceObject != null)
                                {
                                    foreach (var data in serviceBudgetData)
                                    {
                                        if (data.m_Service == serviceDict[ServiceObject.m_Service])
                                        {
                                            Multiplier(ref prevMultiplierValue, ref multiplier, logX, data.m_Budget / 100f, "ServiceObject");
                                            break;
                                        }
                                    }
                                }

                                ServiceUpgrade ServiceUpgrade = prefabBase.GetComponent<ServiceUpgrade>();
                                CrossExamine("ServiceUpgrade");
                                if (ServiceUpgrade != null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, ServiceUpgrade.m_UpgradeCost * 0.05f, "ServiceUpgrade");
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

                                if (prefabSystem.TryGetPrefab(prefabData, out BuildingExtensionPrefab BuildingExtension) && BuildingExtension is not null)
                                {
                                    int plotSize = BuildingExtension.m_OverrideLotSize.x * BuildingExtension.m_OverrideLotSize.y;
                                    if (plotSize > 0)
                                    {
                                        Adder(ref prevUpkeepValue, ref upkeepValue, logX, (float)(Mod.m_Setting.PlotPrice * Math.Round(Math.Log(plotSize + 1))), "PlotPrice");
                                    }
                                }

                                if (prefabSystem.TryGetPrefab(prefabData, out BuildingPrefab Building) && Building is not null)
                                {
                                    Adder(ref prevUpkeepValue, ref upkeepValue, logX, (float)(Mod.m_Setting.PlotPrice * Math.Round(Math.Log(Building.lotSize + 1))), "PlotPrice");
                                }

                                multiplier = Math.Max(multiplier, 0.1f);
                                int upkeepValueInt = (int)Math.Round((upkeepValue * multiplier) / 100) * 100;
                                if (upkeepValueInt >= 0)
                                {
                                    consumptionData.m_Upkeep = upkeepValueInt;
                                    EntityManager.SetComponentData(entity, consumptionData);
                                    RefreshBuffer(entity, upkeepValueInt);
                                    if (log)
                                    {
                                        string logText = $"{name};{ogUpkeep[name]};{upkeepValueInt}";
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
                                    Mod.log.Info($"Something went wrong: {name}'s upkeep is ${upkeepValue}?");
                                    if (log)
                                    {
                                        string logText = $"{name};{ogUpkeep[name]};{upkeepValueInt}";
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
                catch (Exception e) { Mod.log.Error(e); }
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

                    if (EntityManager.TryGetComponent(entity, out PrefabData prefabData) && prefabSystem.TryGetPrefab(prefabData, out PrefabBase prefabBase))
                    {

                        if (EntityManager.TryGetComponent(entity, out ConsumptionData consumptionData))
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
                catch (Exception e) { Mod.log.Error(e); }
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
            return ((Math.Abs(weightedX - weightedY)/N)/100f) + extra;
        }

        void Adder(ref float prevUpkeepValue, ref float upkeepValue, bool logX, float adder, string text)
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

        void Multiplier(ref float prevMultiplierValue, ref float multipler, bool logX, float adder, string text)
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