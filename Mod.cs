using System.Collections.Generic;
using Colossal.IO.AssetDatabase;
using Game;
using Game.Modding;
using Game.SceneFlow;
using SmartUpkeepManager.Systems;
using StarQ.Shared.Extensions;
using StarQ.Shared.Generators;

namespace SmartUpkeepManager
{
    [GenerateModInfo]
    public partial class Mod : IMod
    {
        public void OnLoad(UpdateSystem updateSystem)
        {
            LogHelper.Init(Id, log);
            LocaleHelper.Init(Id, Name, GetReplacements, AddLocale);

            m_Setting = new Setting(this);
            m_Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));
            AssetDatabase.global.LoadSettings(Id, m_Setting, new Setting(this));

            WorldHelper.GetSystem<SmartUpkeepSystem>();
            //WorldHelper.GetSystem<DataRetriever>();
            //updateSystem.UpdateAt<SUM_UI>(SystemUpdatePhase.UIUpdate);
        }

        public void OnDispose()
        {
            LocaleHelper.Dispose();
            m_Setting?.UnregisterInOptionsUI();
            m_Setting = null;
        }

        public static Dictionary<string, string> GetReplacements()
        {
            return new() { };
        }
    }
}
