using System;
using System.Collections.Generic;
using Colossal.Entities;
using Colossal.Serialization.Entities;
using Game;
using Game.Prefabs;
using StarQ.Shared.Extensions;
using Unity.Collections;
using Unity.Entities;

namespace SmartUpkeepManager.Systems
{
    public partial class SmartUpkeepSystem : GameSystemBase
    {
        private PrefabSystem prefabSystem;
        public static readonly Dictionary<string, Entity> buildingDict = new();
        public static readonly Dictionary<ServicePrefab, Entity> serviceDict = new();
        public static readonly Dictionary<string, int> ogUpkeep = new();
        public static readonly Dictionary<string, float> prevVal = new();

        public bool NeedUpdate = false;

        //public static List<string> comps = new();
        //public static List<string> toRemove = new();
        //public static bool systemActive = false;
        //public static bool inGame = false;

        protected override void OnCreate()
        {
            base.OnCreate();
            prefabSystem =
                World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<PrefabSystem>();

            Mod.m_Setting.onSettingsApplied += OnSettingsChanged;
            CollectDataForSUM();
            NeedUpdate = true;
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
            //if (buildingDict.Count != 0)
            //    return;
            buildingDict.Clear();
            try
            {
                EntityQuery buildingQuery = SystemAPI
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
                                LogHelper.SendLog($"Collecting {prefabName}", LogLevel.DEV);
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
            }
            catch (Exception e)
            {
                LogHelper.SendLog(e, LogLevel.Error);
            }
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);
            if (buildingDict.Count == 0)
                CollectDataForSUM();
            if (mode.IsGame() && !Mod.m_Setting.Disable)
                SetUpkeep();
            else
                ResetToVanilla();
        }

        protected override void OnUpdate()
        {
            if (!NeedUpdate || Mod.m_Setting.Disable)
                return;

            SetUpkeep();
            NeedUpdate = false;
        }

        private void OnSettingsChanged(Game.Settings.Setting setting)
        {
            NeedUpdate = true;
            Update();
        }

        public void SetUpkeep()
        {
            try
            {
                CalculateUpkeep();
            }
            catch (Exception ex)
            {
                LogHelper.SendLog(ex, LogLevel.Error);
            }
            LogHelper.SendLog("Update set successful!");
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
