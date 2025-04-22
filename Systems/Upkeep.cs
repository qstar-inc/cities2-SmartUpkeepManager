using System;
using System.Collections.Generic;
using System.Linq;
using Colossal.Entities;
using Colossal.Serialization.Entities;
using Game;
using Game.Prefabs;
using Unity.Collections;
using Unity.Entities;

namespace SmartUpkeepManager.Systems
{
    public partial class SmartUpkeepSystem : GameSystemBase
    {
        private PrefabSystem prefabSystem;
        private EntityQuery buildingQuery;
        private static bool log;
        public static readonly Dictionary<string, Entity> buildingDict = new();
        public static readonly Dictionary<ServicePrefab, Entity> serviceDict = new();
        public static readonly Dictionary<string, int> ogUpkeep = new();
        public static readonly Dictionary<string, float> prevVal = new();

        //public static List<string> comps = new();
        //public static List<string> toRemove = new();
        public static bool systemActive = false;
        public static bool inGame = false;

        protected override void OnCreate()
        {
            base.OnCreate();
            prefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();
            //buildingQuery = GetEntityQuery(new EntityQueryDesc()
            //{
            //    //All = new[] {
            //    //ComponentType.ReadWrite<ConsumptionData>()
            //    //},
            //    Any = new[] {
            //        ComponentType.ReadWrite<BuildingData>(),
            //        ComponentType.ReadWrite<BuildingExtensionData>(),
            //        //ComponentType.ReadWrite<AttractionData>(),
            //        //ComponentType.ReadWrite<BatteryData>(),
            //        //ComponentType.ReadWrite<FireStationData>(),
            //        //ComponentType.ReadWrite<FirewatchTowerData>(),
            //        //ComponentType.ReadWrite<HospitalData>(),
            //    },
            //    None = new[] {
            //        ComponentType.ReadOnly<SpawnableBuildingData>()
            //    }
            //});
        }

        public void CollectDataForSUM()
        {
            //Mod.log.Info("Prefab Data collection started");
            if (systemActive && buildingDict.Count != 0)
                return;
            try
            {
                buildingQuery = SystemAPI
                    .QueryBuilder()
                    //.WithAny<SolarPoweredData, GroundWaterPoweredData, WaterPoweredData, WindPoweredData, PowerPlantData, BatteryData, TransformerData>()
                    //.WithAny<HospitalData>()
                    //.WithAny<FireStationData, FirewatchTowerData>()
                    //.WithAny<AdminBuildingData>()
                    //.WithAny<MaintenanceDepotData>()
                    //.WithAny<CoverageData, WorkplaceData, StorageLimitData, PollutionData, UniqueObjectData, ServiceObjectData>()
                    .WithAny<BuildingData, BuildingExtensionData>()
                    .WithNone<
                        SpawnableBuildingData,
                        SignatureBuildingData,
                        ExtractorFacilityData,
                        PlaceholderBuildingData
                    >()
                    .Build();
                var entities = buildingQuery.ToEntityArray(Allocator.Temp);
                foreach (Entity entity in entities)
                {
                    string prefabName = prefabSystem.GetPrefabName(entity);
                    if (!buildingDict.ContainsKey(prefabName))
                    {
                        if (
                            EntityManager.TryGetComponent(entity, out PrefabData prefabData)
                            && prefabSystem.TryGetPrefab(prefabData, out PrefabBase prefabBase)
                        )
                        {
                            ServiceConsumption serviceConsumption =
                                prefabBase.GetComponent<ServiceConsumption>();
                            if (serviceConsumption != null)
                            {
                                buildingDict.Add(prefabName, entity);
                                if (
                                    !ogUpkeep.ContainsKey(prefabName)
                                    && serviceConsumption.m_Upkeep > 0
                                )
                                    ogUpkeep.Add(prefabName, serviceConsumption.m_Upkeep);
                                //Mod.log.Info($"Collecting {prefabName}");
                            }

                            //foreach (var component in prefabBase.components)
                            //{
                            //    string xxx = $"{component.GetType()}".Replace("Game.Prefabs.", "");
                            //    if (!comps.Contains(xxx))
                            //    {
                            //        comps.Add(xxx);
                            //    }
                            //}
                        }
                    }
                }

                EntityQuery serviceQuery = SystemAPI.QueryBuilder().WithAny<ServiceData>().Build();
                var serviceEntities = serviceQuery.ToEntityArray(Allocator.Temp);
                foreach (Entity entity in serviceEntities)
                {
                    prefabSystem.TryGetPrefab(entity, out PrefabBase prefab);
                    ServicePrefab ServicePrefab = (ServicePrefab)prefab;
                    if (!serviceDict.ContainsKey(ServicePrefab))
                    {
                        serviceDict.Add(ServicePrefab, entity);
                    }
                }

                systemActive = true;
            }
            catch (Exception e)
            {
                Mod.log.Error(e);
            }
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            //Mod.log.Info("OnGameLoadingComplete Start");
            base.OnGameLoadingComplete(purpose, mode);
            if (buildingDict.Count == 0)
                CollectDataForSUM();
            if (GameModeExtensions.IsGame(mode))
            {
                inGame = true;
                if (!Mod.m_Setting.Disable)
                {
                    SetUpkeep();
                }
                else
                {
                    ResetToVanilla();
                }
            }
            else
            {
                inGame = false;
            }
            //Mod.log.Info("OnGameLoadingComplete End");
        }

        protected override void OnUpdate() { }

        public void SetUpkeep()
        {
            if (Mod.m_Setting != null)
            {
                log = Mod.m_Setting.VerboseLogging;
            }
            else
            {
                log = false;
            }

            try
            {
                CalculateUpkeep();
            }
            catch (Exception ex)
            {
                Mod.log.Error(ex);
            }
            if (log)
                Mod.log.Info("Update Complete!");
        }

        public void RefreshBuffer(Entity entity, int amount)
        {
            try
            {
                DynamicBuffer<ServiceUpkeepData> sudBuffer =
                    EntityManager.GetBuffer<ServiceUpkeepData>(entity);

                for (int i = 0; i < sudBuffer.Length; i++)
                {
                    var uge = sudBuffer[i];
                    if (uge.m_Upkeep.m_Resource == Game.Economy.Resource.Money)
                    {
                        uge.m_Upkeep.m_Amount = amount;
                        sudBuffer[i] = uge;
                    }
                }
            }
            catch (Exception) { }
        }

        //        protected override void OnDestroy()
        //        {
        //            base.OnDestroy();
        //#if DEBUG
        //            string[] toRxemove = {

        //                // General
        //                //"ServiceCoverage",
        //                //"Workplace",
        //                //"Pollution",
        //                //"StorageLimit",
        //                //"UniqueObject",
        //                //"ServiceObject",

        //                //// Mixed
        //                //"MaintenanceDepot",
        //                //"ObjectSubObjects",

        //                //// Roads
        //                //"ParkingFacility",

        //                //// Electricity
        //                //"SolarPowered",
        //                //"GroundWaterPowered",
        //                //"WaterPowered",
        //                //"WindPowered",
        //                //"GarbagePowered",
        //                //"PowerPlant",
        //                //"Battery",
        //                //"Transformer",

        //                ////Water & Sewage
        //                //"WaterPumpingStation",
        //                //"SewageOutlet",

        //                ////Healthcare & Deathcare
        //                //"Hospital",
        //                //"DeathcareFacility",

        //                //// Garbage Management
        //                //"GarbageFacility",

        //                //// Education & Research
        //                //"School",
        //                //"ResearchFacility",

        //                //// Fire & Rescue
        //                //"FireStation",
        //                //"FirewatchTower",
        //                ////EarlyDisasterWarningSystem
        //                ////DisasterFacility
        //                ////EmergencyGenerator
        //                ////EmergencyShelter

        //                //// Police & Administration
        //                ////PoliceStation
        //                ////Prison
        //                ////WelfareOffice
        //                //"AdministrationBuilding",

        //                //// Transportation
        //                ////CargoTransportStation
        //                ////TransportDepot
        //                ////TransportStation

        //                //// Parks & Recreation
        //                ////Park
        //                ////LeisureProvider
        //                //"Attraction",

        //                //// Communication
        //                ////PostFacility
        //                ////TelecomFacility

        //                // Ignore
        //                "AchievementFilter",
        //                "ActivityLocation",
        //                "AssetPackItem",
        //                "BuildingTerraformOverride",
        //                "ContentPrerequisite",
        //                "DefaultPolicies",
        //                "DestructibleObject",
        //                "EditorAssetCategoryOverride",
        //                "EffectSource",
        //                "FloatingObject",
        //                "InitialResources",
        //                "NetObject",
        //                "ObjectAchievementComponent",
        //                "ObjectSubAreas",
        //                "ObjectSubLanes",
        //                "ObjectSubNets",
        //                "ObsoleteIdentifiers",
        //                "PlaceableObject",
        //                "PlaceholderObject",
        //                "ResourceConsumer",
        //                "ResourceProducer",
        //                "ServiceFeeCollector",
        //                "ServiceUpgrade",
        //                "ShorelineObject",
        //                "SignatureBuilding",
        //                "SpawnableBuilding",
        //                "SpawnableObject",
        //                "StandingObject",
        //                "SubObjectDefaultProbability",
        //                "ThemeObject",
        //                "TrafficSpawner",
        //                "UIObject",
        //                "Unlockable",
        //                "UnlockOnBuild",
        //            };
        //            var x = toRemove.Union(toRxemove);
        //            comps.RemoveAll(x.Contains);
        //            comps.Sort();
        //            Mod.log.Info($"-------------");
        //            foreach (var item in comps)
        //            {
        //                Mod.log.Info(item.ToString());
        //            }
        //#endif
        //        }
    }
}
