using System.Collections.Generic;
using System.Reflection;
using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using SmartUpkeepManager.Systems;
using StarQ.Shared.Extensions;

namespace SmartUpkeepManager
{
    public class Mod : IMod
    {
        public static string Id = nameof(SmartUpkeepManager);
        public static string Name = Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyTitleAttribute>()
            .Title;
        public static string Version = Assembly
            .GetExecutingAssembly()
            .GetName()
            .Version.ToString(3);

        public static ILog log = LogManager.GetLogger($"{Id}").SetShowsErrorsInUI(false);
        public static Setting m_Setting;

        public void OnLoad(UpdateSystem updateSystem)
        {
            LogHelper.Init(Id, log);
            LocaleHelper.Init(Id, Name, GetReplacements);

            m_Setting = new Setting(this);
            m_Setting.RegisterInOptionsUI();

            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));
            AssetDatabase.global.LoadSettings(
                nameof(SmartUpkeepManager),
                m_Setting,
                new Setting(this)
            );
            WorldHelper.GetSystem<SmartUpkeepSystem>();
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
