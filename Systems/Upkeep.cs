using System;
using System.Collections.Generic;
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

        public static bool NeedUpdate = false;

        protected override void OnCreate()
        {
            base.OnCreate();
            prefabSystem = WorldHelper.PrefabSystem;

            Mod.m_Setting.onSettingsApplied += OnSettingsChanged;
            NeedUpdate = true;
        }

        public void CollectDataForSUM()
        {
            LogHelper.SendLog("Prefab Data collection started", LogLevel.DEVD);
            buildingDict.Clear();
            try
            {
                EntityQuery buildingQuery = SystemAPI
                    .QueryBuilder()
                    .WithAny<BuildingData, BuildingExtensionData>()
                    .WithNone<
                        SpawnableBuildingData,
                        SignatureBuildingData,
                        ExtractorFacilityData,
                        PlaceholderBuildingData
                    >()
                    .Build();
                NativeArray<Entity> entities = buildingQuery.ToEntityArray(Allocator.Temp);
                foreach (Entity entity in entities)
                {
                    string prefabName = prefabSystem.GetPrefabName(entity);
                    if (!buildingDict.ContainsKey(prefabName))
                        if (prefabSystem.TryGetPrefab(entity, out PrefabBase prefabBase))
                            if (prefabBase.TryGet(out ServiceConsumption serviceConsumption))
                            {
                                buildingDict.Add(prefabName, entity);
                                if (
                                    !ogUpkeep.ContainsKey(prefabName)
                                    && serviceConsumption.m_Upkeep > 0
                                )
                                    ogUpkeep.Add(prefabName, serviceConsumption.m_Upkeep);
                                LogHelper.SendLog($"Collecting {prefabName}", LogLevel.DEVD);
                            }
                }

                EntityQuery serviceQuery = SystemAPI.QueryBuilder().WithAny<ServiceData>().Build();
                NativeArray<Entity> serviceEntities = serviceQuery.ToEntityArray(Allocator.Temp);
                foreach (Entity entity in serviceEntities)
                {
                    prefabSystem.TryGetPrefab(entity, out PrefabBase prefab);
                    ServicePrefab ServicePrefab = (ServicePrefab)prefab;
                    if (!serviceDict.ContainsKey(ServicePrefab))
                        serviceDict.Add(ServicePrefab, entity);
                }
            }
            catch (Exception e)
            {
                LogHelper.SendLog(e, LogLevel.Error);
            }
            LogHelper.SendLog("Prefab Data collection ended", LogLevel.DEVD);
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);
            LogHelper.SendLog("OnGameLoadingComplete started", LogLevel.DEVD);
            if (!WorldHelper.IsGame)
                return;

            if (buildingDict.Count == 0)
                CollectDataForSUM();
            if (Mod.m_Setting.EnableMod)
                SetUpkeep();
            else
                ResetToVanilla();
            LogHelper.SendLog("OnGameLoadingComplete ended", LogLevel.DEVD);
        }

        protected override void OnUpdate() { }

        private void OnSettingsChanged(Game.Settings.Setting setting)
        {
            LogHelper.SendLog("OnSettingsChanged started", LogLevel.DEVD);
            NeedUpdate = true;
            DoUpdate();
            LogHelper.SendLog("OnSettingsChanged ended", LogLevel.DEVD);
        }

        public void DoUpdate()
        {
            if (!NeedUpdate || !Mod.m_Setting.EnableMod)
                return;

            SetUpkeep();
            NeedUpdate = false;
        }

        public void SetUpkeep()
        {
            try
            {
                CalculateUpkeep();
                LogHelper.SendLog("Upkeeps set successful!", LogLevel.DEVD);
            }
            catch (Exception ex)
            {
                LogHelper.SendLog(ex, LogLevel.Error);
            }
        }
    }
}
