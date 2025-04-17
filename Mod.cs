using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using SmartUpkeepManager.Systems;
using System.Reflection;
using Unity.Entities;

namespace SmartUpkeepManager
{
    public class Mod : IMod
    {
        public static string Name = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;
        public static string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);

        public static ILog log = LogManager.GetLogger($"{nameof(SmartUpkeepManager)}").SetShowsErrorsInUI(false);
        public static Setting m_Setting;

        public void OnLoad(UpdateSystem updateSystem)
        {
#if DEBUG
            log.Info($"{Name} {Version}: DEV Build");
#endif
            m_Setting = new Setting(this);
            m_Setting.RegisterInOptionsUI();

            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));
            AssetDatabase.global.LoadSettings(nameof(SmartUpkeepManager), m_Setting, new Setting(this));
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<SmartUpkeepSystem>();
        }

        public void OnDispose()
        {
            if (m_Setting != null)
            {
                m_Setting.UnregisterInOptionsUI();
                m_Setting = null;
            }
        }
    }
}