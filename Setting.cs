using Colossal.IO.AssetDatabase;
using Colossal.Json;
using Game.Modding;
using Game.Settings;
using Game.UI;
using StarQ.Shared.Extensions;

namespace SmartUpkeepManager
{
    [FileLocation("ModsSettings\\StarQ\\" + nameof(SmartUpkeepManager))]
    [SettingsUITabOrder(Page1, Page2, Page3, AboutTab)]
    [SettingsUIGroupOrder(
        Buttons,
        GeneralGroup,
        Roads,
        Electricity,
        Water,
        Health,
        Garbage,
        Education,
        Fire,
        Police,
        Transportation,
        Parks,
        Communication,
        InfoGroup
    )]
    [SettingsUIShowGroupName(
        GeneralGroup,
        Roads,
        Electricity,
        Water,
        Health,
        Garbage,
        Education,
        Fire,
        Police,
        Transportation,
        Parks,
        Communication
    )]
    public partial class Setting : ModSetting
    {
        public const string Page1 = "Page 1";
        public const string Page2 = "Page 2";
        public const string Page3 = "Page 3";
        public const string Buttons = "Save Changes";
        public const string GeneralGroup = "GeneralGroup";
        public const string Roads = "Roads";
        public const string Electricity = "Electricity";
        public const string Water = "Water";
        public const string Health = "Healthcare & Deathcare";
        public const string Garbage = "Garbage Management";
        public const string Education = "Education & Research";
        public const string Fire = "Fire & Rescue";
        public const string Police = "Police & Administration";
        public const string Transportation = "Transportation";
        public const string Parks = "Parks & Recreation";
        public const string Communication = "Communication";

        public const string AboutTab = "AboutTab";
        public const string InfoGroup = "InfoGroup";

        public const string LogTab = "LogTab";

        [SettingsUISection(Page1, Buttons)]
        public bool Disable { get; set; }

        //{
        //    get => GetValue(nameof(Disable), false);
        //    set => SetValue(nameof(Disable), value, Save);
        //}

        //[SettingsUIButtonGroup("Options")]
        //[SettingsUISection(Page1, Buttons)]
        //public bool SaveButton
        //{
        //    set { Save(); }
        //}

        [SettingsUIButtonGroup("Options")]
        [SettingsUISection(Page1, Buttons)]
        public bool ModDefault
        {
            set { SetDefaults(); }
        }

        [SettingsUIButtonGroup("Options")]
        [SettingsUISection(Page1, Buttons)]
        public bool MakeFree
        {
            set { SetFree(); }
        }

        //private readonly Dictionary<string, object> _values = new();
        //private bool _dontSave = false;

        //private T GetValue<T>(string propertyName, T defaultValue = default)
        //{
        //    if (_values.TryGetValue(propertyName, out var value))
        //    {
        //        try
        //        {
        //            return (T)Convert.ChangeType(value, typeof(T));
        //        }
        //        catch (InvalidCastException)
        //        {
        //            Mod.log.Info(
        //                $"Warning: Unable to cast setting '{propertyName}' to {typeof(T)}. Returning default."
        //            );
        //        }
        //    }
        //    return defaultValue;
        //}

        //private void SetValue<T>(string propertyName, T value, Action onChanged = null)
        //{
        //    _values[propertyName] = value;
        //    if (!_dontSave)
        //        onChanged?.Invoke();
        //}

        //public void Save()
        //{
        //    if (Disable)
        //    {
        //        suS.ResetToVanilla();
        //        return;
        //    }

        //    if (SmartUpkeepSystem.systemActive && !Disable && SmartUpkeepSystem.inGame)
        //        suS.SetUpkeep();
        //}

        // Roads
        [SettingsUISection(Page1, Roads)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int RoadMaintenance { get; set; }

        //{
        //    get => GetValue(nameof(RoadMaintenance), Defaults.RoadMaintenance);
        //    set => SetValue(nameof(RoadMaintenance), value, Save);
        //}

        [SettingsUISection(Page1, Roads)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int SnowPloughing { get; set; }

        //{
        //    get => GetValue(nameof(SnowPloughing), Defaults.SnowPloughing);
        //    set => SetValue(nameof(SnowPloughing), value, Save);
        //}

        [SettingsUISection(Page1, Roads)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Towing { get; set; }

        //{
        //    get => GetValue(nameof(Towing), Defaults.Towing);
        //    set => SetValue(nameof(Towing), value, Save);
        //}

        [SettingsUISection(Page1, Roads)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int RoadMaintenanceVehicle { get; set; }

        //{
        //    get => GetValue(nameof(RoadMaintenanceVehicle), Defaults.RoadMaintenanceVehicle);
        //    set => SetValue(nameof(RoadMaintenanceVehicle), value, Save);
        //}

        // Electricity
        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 50,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int SolarPowered { get; set; }

        //{
        //            get => GetValue(nameof(SolarPowered), Defaults.SolarPowered);
        //            set => SetValue(nameof(SolarPowered), value, Save);
        //        }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 50,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int GroundWaterPowered { get; set; }

        //{
        //            get => GetValue(nameof(GroundWaterPowered), Defaults.GroundWaterPowered);
        //            set => SetValue(nameof(GroundWaterPowered), value, Save);
        //        }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 50,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int WaterPowered { get; set; }

        //{
        //            get => GetValue(nameof(WaterPowered), Defaults.WaterPowered);
        //            set => SetValue(nameof(WaterPowered), value, Save);
        //        }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 50,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int WindPowered { get; set; }

        //{
        //            get => GetValue(nameof(WindPowered), Defaults.WindPowered);
        //            set => SetValue(nameof(WindPowered), value, Save);
        //        }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 50,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int GarbagePowered { get; set; }

        //{
        //            get => GetValue(nameof(GarbagePowered), Defaults.GarbagePowered);
        //            set => SetValue(nameof(GarbagePowered), value, Save);
        //        }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 50,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int ElectricityProduction { get; set; }

        //{
        //            get => GetValue(nameof(ElectricityProduction), Defaults.ElectricityProduction);
        //            set => SetValue(nameof(ElectricityProduction), value, Save);
        //        }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 50,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int BatteryOut { get; set; }

        //{
        //            get => GetValue(nameof(BatteryOut), Defaults.BatteryOut);
        //            set => SetValue(nameof(BatteryOut), value, Save);
        //        }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 50,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int BatteryCap { get; set; }

        //{
        //            get => GetValue(nameof(BatteryCap), Defaults.BatteryCap);
        //            set => SetValue(nameof(BatteryCap), value, Save);
        //        }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 5000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Transformer { get; set; }

        //{
        //            get => GetValue(nameof(Transformer), Defaults.Transformer);
        //            set => SetValue(nameof(Transformer), value, Save);
        //        }

        //Water & Sewage
        [SettingsUISection(Page1, Water)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 500,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int WaterPumpCap { get; set; }

        //{
        //            get => GetValue(nameof(WaterPumpCap), Defaults.WaterPumpCap);
        //            set => SetValue(nameof(WaterPumpCap), value, Save);
        //        }

        [SettingsUISection(Page1, Water)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 500,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int SewageOutCap { get; set; }

        //{
        //            get => GetValue(nameof(SewageOutCap), Defaults.SewageOutCap);
        //            set => SetValue(nameof(SewageOutCap), value, Save);
        //        }

        [SettingsUISection(Page1, Water)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 30000,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Purification { get; set; }

        //{
        //            get => GetValue(nameof(Purification), Defaults.Purification);
        //            set => SetValue(nameof(Purification), value, Save);
        //        }

        //Healthcare & Deathcare
        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Ambulance { get; set; }

        //{
        //            get => GetValue(nameof(Ambulance), Defaults.Ambulance);
        //            set => SetValue(nameof(Ambulance), value, Save);
        //        }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 50000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int MedicalHelicopter { get; set; }

        //{
        //            get => GetValue(nameof(MedicalHelicopter), Defaults.MedicalHelicopter);
        //            set => SetValue(nameof(MedicalHelicopter), value, Save);
        //        }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Patient { get; set; }

        //{
        //            get => GetValue(nameof(Patient), Defaults.Patient);
        //            set => SetValue(nameof(Patient), value, Save);
        //        }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 3000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int HealthBonus { get; set; }

        //{
        //            get => GetValue(nameof(HealthBonus), Defaults.HealthBonus);
        //            set => SetValue(nameof(HealthBonus), value, Save);
        //        }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 5000,
            step = 250,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int HealthRange { get; set; }

        //{
        //            get => GetValue(nameof(HealthRange), Defaults.HealthRange);
        //            set => SetValue(nameof(HealthRange), value, Save);
        //        }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 100000,
            step = 10000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Treatment { get; set; }

        //{
        //            get => GetValue(nameof(Treatment), Defaults.Treatment);
        //            set => SetValue(nameof(Treatment), value, Save);
        //        }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Hearse { get; set; }

        //{
        //            get => GetValue(nameof(Hearse), Defaults.Hearse);
        //            set => SetValue(nameof(Hearse), value, Save);
        //        }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 2000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int BodyStorage { get; set; }

        //{
        //            get => GetValue(nameof(BodyStorage), Defaults.BodyStorage);
        //            set => SetValue(nameof(BodyStorage), value, Save);
        //        }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 2000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int BodyProcessing { get; set; }

        //{
        //            get => GetValue(nameof(BodyProcessing), Defaults.BodyProcessing);
        //            set => SetValue(nameof(BodyProcessing), value, Save);
        //        }

        // Garbage Management
        [SettingsUISection(Page1, Garbage)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 500,
            step = 10,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int GarbageCap { get; set; }

        //{
        //            get => GetValue(nameof(GarbageCap), Defaults.GarbageCap);
        //            set => SetValue(nameof(GarbageCap), value, Save);
        //        }

        [SettingsUISection(Page1, Garbage)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int GarbageTruck { get; set; }

        //{
        //            get => GetValue(nameof(GarbageTruck), Defaults.GarbageTruck);
        //            set => SetValue(nameof(GarbageTruck), value, Save);
        //        }

        [SettingsUISection(Page1, Garbage)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int DumpTruck { get; set; }

        //{
        //            get => GetValue(nameof(DumpTruck), Defaults.DumpTruck);
        //            set => SetValue(nameof(DumpTruck), value, Save);
        //        }

        [SettingsUISection(Page1, Garbage)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 10,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int GarbageProcessing { get; set; }

        //{
        //            get => GetValue(nameof(GarbageProcessing), Defaults.GarbageProcessing);
        //            set => SetValue(nameof(GarbageProcessing), value, Save);
        //        }

        // Education & Research
        //[SettingsUISection(Page3, Education)]
        //[SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        //[SettingsUISlider(min = 0, max = 5000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        //public int StudentPrimary
        //{
        //    get => GetValue(nameof(StudentPrimary), Defaults.StudentPrimary);
        //    set => SetValue(nameof(StudentPrimary), value, Save);
        //}

        //[SettingsUISection(Page3, Education)]
        //[SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        //[SettingsUISlider(min = 0, max = 2500, step = 10, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        //public int StudentSecondary
        //{
        //    get => GetValue(nameof(StudentSecondary), Defaults.StudentSecondary);
        //    set => SetValue(nameof(StudentSecondary), value, Save);
        //}

        //[SettingsUISection(Page3, Education)]
        //[SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        //[SettingsUISlider(min = 0, max = 1000, step = 10, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        //public int StudentTertiary
        //{
        //    get => GetValue(nameof(StudentTertiary), Defaults.StudentTertiary);
        //    set => SetValue(nameof(StudentTertiary), value, Save);
        //}

        [SettingsUISection(Page3, Education)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 100,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Student { get; set; }

        //{
        //            get => GetValue(nameof(Student), Defaults.Student);
        //            set => SetValue(nameof(Student), value, Save);
        //        }

        [SettingsUISection(Page3, Education)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int StudentGraduation { get; set; }

        //{
        //            get => GetValue(nameof(StudentGraduation), Defaults.StudentGraduation);
        //            set => SetValue(nameof(StudentGraduation), value, Save);
        //        }

        [SettingsUISection(Page3, Education)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int StudentWellbeing { get; set; }

        //{
        //            get => GetValue(nameof(StudentWellbeing), Defaults.StudentWellbeing);
        //            set => SetValue(nameof(StudentWellbeing), value, Save);
        //        }

        [SettingsUISection(Page3, Education)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int StudentHealth { get; set; }

        //{
        //            get => GetValue(nameof(StudentHealth), Defaults.StudentHealth);
        //            set => SetValue(nameof(StudentHealth), value, Save);
        //        }

        [SettingsUISection(Page3, Education)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 200000,
            step = 5000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int ResearchFacility { get; set; }

        //{
        //            get => GetValue(nameof(ResearchFacility), Defaults.ResearchFacility);
        //            set => SetValue(nameof(ResearchFacility), value, Save);
        //        }

        // Fire & Rescue
        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int FireTruck { get; set; }

        //{
        //            get => GetValue(nameof(FireTruck), Defaults.FireTruck);
        //            set => SetValue(nameof(FireTruck), value, Save);
        //        }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 50000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int FireHelicopter { get; set; }

        //{
        //            get => GetValue(nameof(FireHelicopter), Defaults.FireHelicopter);
        //            set => SetValue(nameof(FireHelicopter), value, Save);
        //        }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 50000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int FireDisasterCap { get; set; }

        //{
        //            get => GetValue(nameof(FireDisasterCap), Defaults.FireDisasterCap);
        //            set => SetValue(nameof(FireDisasterCap), value, Save);
        //        }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 500,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int FireVehicleEffi { get; set; }

        //{
        //            get => GetValue(nameof(FireVehicleEffi), Defaults.FireVehicleEffi);
        //            set => SetValue(nameof(FireVehicleEffi), value, Save);
        //        }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 20000,
            step = 500,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Firewatch { get; set; }

        //{
        //            get => GetValue(nameof(Firewatch), Defaults.Firewatch);
        //            set => SetValue(nameof(Firewatch), value, Save);
        //        }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 30000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int EarlyDisasterWarningSystem { get; set; }

        //{
        //            get =>
        //                GetValue(nameof(EarlyDisasterWarningSystem), Defaults.EarlyDisasterWarningSystem);
        //            set => SetValue(nameof(EarlyDisasterWarningSystem), value, Save);
        //        }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 50000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int DisasterFacility { get; set; }

        //{
        //            get => GetValue(nameof(DisasterFacility), Defaults.DisasterFacility);
        //            set => SetValue(nameof(DisasterFacility), value, Save);
        //        }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 5000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int ShelterCap { get; set; }

        //{
        //            get => GetValue(nameof(ShelterCap), Defaults.ShelterCap);
        //            set => SetValue(nameof(ShelterCap), value, Save);
        //        }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int EvacuationBus { get; set; }

        //{
        //            get => GetValue(nameof(EvacuationBus), Defaults.EvacuationBus);
        //            set => SetValue(nameof(EvacuationBus), value, Save);
        //        }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 5000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int EmergencyGenerator { get; set; }

        //{
        //            get => GetValue(nameof(EmergencyGenerator), Defaults.EmergencyGenerator);
        //            set => SetValue(nameof(EmergencyGenerator), value, Save);
        //        }

        // Police & Administration
        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int PatrolCar { get; set; }

        //{
        //            get => GetValue(nameof(PatrolCar), Defaults.PatrolCar);
        //            set => SetValue(nameof(PatrolCar), value, Save);
        //        }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 50000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int PoliceHelicopter { get; set; }

        //{
        //            get => GetValue(nameof(PoliceHelicopter), Defaults.PoliceHelicopter);
        //            set => SetValue(nameof(PoliceHelicopter), value, Save);
        //        }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 5000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LocalJail { get; set; }

        //{
        //            get => GetValue(nameof(LocalJail), Defaults.LocalJail);
        //            set => SetValue(nameof(LocalJail), value, Save);
        //        }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 200000,
            step = 5000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Patrol { get; set; }

        //{
        //            get => GetValue(nameof(Patrol), Defaults.Patrol);
        //            set => SetValue(nameof(Patrol), value, Save);
        //        }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 200000,
            step = 5000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int EmergencyPolice { get; set; }

        //{
        //            get => GetValue(nameof(EmergencyPolice), Defaults.EmergencyPolice);
        //            set => SetValue(nameof(EmergencyPolice), value, Save);
        //        }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 200000,
            step = 5000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Intelligence { get; set; }

        //{
        //            get => GetValue(nameof(Intelligence), Defaults.Intelligence);
        //            set => SetValue(nameof(Intelligence), value, Save);
        //        }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int PrisonVan { get; set; }

        //{
        //            get => GetValue(nameof(PrisonVan), Defaults.PrisonVan);
        //            set => SetValue(nameof(PrisonVan), value, Save);
        //        }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 5000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int PrisonerCap { get; set; }

        //{
        //            get => GetValue(nameof(PrisonerCap), Defaults.PrisonerCap);
        //            set => SetValue(nameof(PrisonerCap), value, Save);
        //        }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int PrisonerWellbeing { get; set; }

        //{
        //            get => GetValue(nameof(PrisonerWellbeing), Defaults.PrisonerWellbeing);
        //            set => SetValue(nameof(PrisonerWellbeing), value, Save);
        //        }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int PrisonerHealth { get; set; }

        //{
        //            get => GetValue(nameof(PrisonerHealth), Defaults.PrisonerHealth);
        //            set => SetValue(nameof(PrisonerHealth), value, Save);
        //        }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 200000,
            step = 5000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int WelfareOffice { get; set; }

        //{
        //            get => GetValue(nameof(WelfareOffice), Defaults.WelfareOffice);
        //            set => SetValue(nameof(WelfareOffice), value, Save);
        //        }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 200000,
            step = 5000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int AdminBuilding { get; set; }

        //{
        //            get => GetValue(nameof(AdminBuilding), Defaults.AdminBuilding);
        //            set => SetValue(nameof(AdminBuilding), value, Save);
        //        }

        // Transportation
        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 5000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int PlatformMaintenance { get; set; }

        //{
        //            get => GetValue(nameof(PlatformMaintenance), Defaults.PlatformMaintenance);
        //            set => SetValue(nameof(PlatformMaintenance), value, Save);
        //        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Bus { get; set; }

        //{
        //            get => GetValue(nameof(Bus), Defaults.Bus);
        //            set => SetValue(nameof(Bus), value, Save);
        //        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 20000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Train { get; set; }

        //{
        //            get => GetValue(nameof(Train), Defaults.Train);
        //            set => SetValue(nameof(Train), value, Save);
        //        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Taxi { get; set; }

        //{
        //            get => GetValue(nameof(Taxi), Defaults.Taxi);
        //            set => SetValue(nameof(Taxi), value, Save);
        //        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 20000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Tram { get; set; }

        //{
        //            get => GetValue(nameof(Tram), Defaults.Tram);
        //            set => SetValue(nameof(Tram), value, Save);
        //        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 100000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Ship { get; set; }

        //{
        //            get => GetValue(nameof(Ship), Defaults.Ship);
        //            set => SetValue(nameof(Ship), value, Save);
        //        }

        //[SettingsUISection(Page3, Transportation)]
        //[SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        //[SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        //public int Post
        //{
        //    get => GetValue(nameof(Post), Defaults.Post);
        //    set => SetValue(nameof(Post), value, Save);
        //}

        //[SettingsUIHidden]
        //[SettingsUISection(Page3, Transportation)]
        //[SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        //[SettingsUISlider(min = 0, max = 50000, step = 1000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        //public int Helicopter
        //{
        //    get => GetValue(nameof(Helicopter), Defaults.Helicopter);
        //    set => SetValue(nameof(Helicopter), value, Save);
        //}

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 50000,
            step = 5000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Airplane { get; set; }

        //{
        //            get => GetValue(nameof(Airplane), Defaults.Airplane);
        //            set => SetValue(nameof(Airplane), value, Save);
        //        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 20000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Subway { get; set; }

        //{
        //            get => GetValue(nameof(Subway), Defaults.Subway);
        //            set => SetValue(nameof(Subway), value, Save);
        //        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 100000,
            step = 5000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Rocket { get; set; }

        //{
        //            get => GetValue(nameof(Rocket), Defaults.Rocket);
        //            set => SetValue(nameof(Rocket), value, Save);
        //        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 500,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int EnergyFuel { get; set; }

        //{
        //            get => GetValue(nameof(EnergyFuel), Defaults.EnergyFuel);
        //            set => SetValue(nameof(EnergyFuel), value, Save);
        //        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 500,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int EnergyElectricity { get; set; }

        //{
        //            get => GetValue(nameof(EnergyElectricity), Defaults.EnergyElectricity);
        //            set => SetValue(nameof(EnergyElectricity), value, Save);
        //        }

        //[SettingsUIHidden]
        //[SettingsUISection(Page3, Transportation)]
        //[SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        //[SettingsUISlider(min = 0, max = 200000, step = 5000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        //public int ProductionBoost
        //{
        //    get => GetValue(nameof(ProductionBoost), Defaults.ProductionBoost);
        //    set => SetValue(nameof(ProductionBoost), value, Save);
        //}

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 50000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int MaintenanceBoost { get; set; }

        //{
        //            get => GetValue(nameof(MaintenanceBoost), Defaults.MaintenanceBoost);
        //            set => SetValue(nameof(MaintenanceBoost), value, Save);
        //        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int DispatchCenter { get; set; }

        //{
        //            get => GetValue(nameof(DispatchCenter), Defaults.DispatchCenter);
        //            set => SetValue(nameof(DispatchCenter), value, Save);
        //        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int TradedResource { get; set; }

        //{
        //            get => GetValue(nameof(TradedResource), Defaults.TradedResource);
        //            set => SetValue(nameof(TradedResource), value, Save);
        //        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int DeliveryTruck { get; set; }

        //{
        //            get => GetValue(nameof(DeliveryTruck), Defaults.DeliveryTruck);
        //            set => SetValue(nameof(DeliveryTruck), value, Save);
        //        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 10,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int ComfortFactor { get; set; }

        //{
        //            get => GetValue(nameof(ComfortFactor), Defaults.ComfortFactor);
        //            set => SetValue(nameof(ComfortFactor), value, Save);
        //        }

        // Parks & Recreation
        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 30000,
            step = 500,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int ParkMaintenance { get; set; }

        //{
        //            get => GetValue(nameof(ParkMaintenance), Defaults.ParkMaintenance);
        //            set => SetValue(nameof(ParkMaintenance), value, Save);
        //        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int ParkMaintenanceVehicle { get; set; }

        //{
        //            get => GetValue(nameof(ParkMaintenanceVehicle), Defaults.ParkMaintenanceVehicle);
        //            set => SetValue(nameof(ParkMaintenanceVehicle), value, Save);
        //        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 5000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LeisureEfficieny { get; set; }

        //{
        //            get => GetValue(nameof(LeisureEfficieny), Defaults.LeisureEfficieny);
        //            set => SetValue(nameof(LeisureEfficieny), value, Save);
        //        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LeisureMeals { get; set; }

        //{
        //            get => GetValue(nameof(LeisureMeals), Defaults.LeisureMeals);
        //            set => SetValue(nameof(LeisureMeals), value, Save);
        //        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LeisureEntertainment { get; set; }

        //{
        //            get => GetValue(nameof(LeisureEntertainment), Defaults.LeisureEntertainment);
        //            set => SetValue(nameof(LeisureEntertainment), value, Save);
        //        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LeisureCommercial { get; set; }

        //{
        //            get => GetValue(nameof(LeisureCommercial), Defaults.LeisureCommercial);
        //            set => SetValue(nameof(LeisureCommercial), value, Save);
        //        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LeisureCityIndoors { get; set; }

        //{
        //            get => GetValue(nameof(LeisureCityIndoors), Defaults.LeisureCityIndoors);
        //            set => SetValue(nameof(LeisureCityIndoors), value, Save);
        //        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LeisureTravel { get; set; }

        //{
        //            get => GetValue(nameof(LeisureTravel), Defaults.LeisureTravel);
        //            set => SetValue(nameof(LeisureTravel), value, Save);
        //        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LeisureCityPark { get; set; }

        //{
        //            get => GetValue(nameof(LeisureCityPark), Defaults.LeisureCityPark);
        //            set => SetValue(nameof(LeisureCityPark), value, Save);
        //        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LeisureCityBeach { get; set; }

        //{
        //            get => GetValue(nameof(LeisureCityBeach), Defaults.LeisureCityBeach);
        //            set => SetValue(nameof(LeisureCityBeach), value, Save);
        //        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LeisureAttractions { get; set; }

        //{
        //            get => GetValue(nameof(LeisureAttractions), Defaults.LeisureAttractions);
        //            set => SetValue(nameof(LeisureAttractions), value, Save);
        //        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LeisureRelaxation { get; set; }

        //{
        //            get => GetValue(nameof(LeisureRelaxation), Defaults.LeisureRelaxation);
        //            set => SetValue(nameof(LeisureRelaxation), value, Save);
        //        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LeisureSightseeing { get; set; }

        //{
        //            get => GetValue(nameof(LeisureSightseeing), Defaults.LeisureSightseeing);
        //            set => SetValue(nameof(LeisureSightseeing), value, Save);
        //        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Attraction { get; set; }

        //{
        //            get => GetValue(nameof(Attraction), Defaults.Attraction);
        //            set => SetValue(nameof(Attraction), value, Save);
        //        }

        // Communication
        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int PostVan { get; set; }

        //{
        //            get => GetValue(nameof(PostVan), Defaults.PostVan);
        //            set => SetValue(nameof(PostVan), value, Save);
        //        }

        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int PostTruck { get; set; }

        //{
        //            get => GetValue(nameof(PostTruck), Defaults.PostTruck);
        //            set => SetValue(nameof(PostTruck), value, Save);
        //        }

        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 200,
            step = 10,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int MailCap { get; set; }

        //{
        //            get => GetValue(nameof(MailCap), Defaults.MailCap);
        //            set => SetValue(nameof(MailCap), value, Save);
        //        }

        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 5000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int PostSortingRate { get; set; }

        //{
        //            get => GetValue(nameof(PostSortingRate), Defaults.PostSortingRate);
        //            set => SetValue(nameof(PostSortingRate), value, Save);
        //        }

        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 15000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int TelecomRange { get; set; }

        //{
        //            get => GetValue(nameof(TelecomRange), Defaults.TelecomRange);
        //            set => SetValue(nameof(TelecomRange), value, Save);
        //        }

        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 2000,
            step = 50,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int NetworkCap { get; set; }

        //{
        //            get => GetValue(nameof(NetworkCap), Defaults.NetworkCap);
        //            set => SetValue(nameof(NetworkCap), value, Save);
        //        }

        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 20000,
            step = 500,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Wireless { get; set; }

        //{
        //            get => GetValue(nameof(Wireless), Defaults.Wireless);
        //            set => SetValue(nameof(Wireless), value, Save);
        //        }

        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 50000,
            step = 500,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Fibre { get; set; }

        //{
        //            get => GetValue(nameof(Fibre), Defaults.Fibre);
        //            set => SetValue(nameof(Fibre), value, Save);
        //        }

        // Landscaping

        // General

        [SettingsUISection(Page1, GeneralGroup)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 100,
            step = 2,
            scalarMultiplier = 1,
            unit = Unit.kPercentage
        )]
        public bool ServiceBudgetMultiplier { get; set; }

        //{
        //            get => GetValue(nameof(ServiceBudgetMultiplier), true);
        //            set => SetValue(nameof(ServiceBudgetMultiplier), value, Save);
        //        }

        [SettingsUISection(Page1, GeneralGroup)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 100,
            step = 2,
            scalarMultiplier = 1,
            unit = Unit.kPercentage
        )]
        public bool CityBonusMultiplier { get; set; }

        //{
        //            get => GetValue(nameof(CityBonusMultiplier), true);
        //            set => SetValue(nameof(CityBonusMultiplier), value, Save);
        //        }

        [SettingsUISection(Page1, GeneralGroup)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kPercentage
        )]
        public int ServiceCoverageMultiplier { get; set; }

        //{
        //            get => GetValue(nameof(ServiceCoverageMultiplier), Defaults.ServiceCoverageMultiplier);
        //            set => SetValue(nameof(ServiceCoverageMultiplier), value, Save);
        //        }

        [SettingsUISection(Page1, GeneralGroup)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 10,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerCell
        )]
        public int PlotPrice { get; set; }

        //{
        //            get => GetValue(nameof(PlotPrice), Defaults.PlotPrice);
        //            set => SetValue(nameof(PlotPrice), value, Save);
        //        }

        [SettingsUISection(Page1, Roads)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 500,
            step = 10,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int ParkingSpots { get; set; }

        //{
        //            get => GetValue(nameof(ParkingSpots), Defaults.ParkingSpots);
        //            set => SetValue(nameof(ParkingSpots), value, Save);
        //        }

        [SettingsUISection(Page1, GeneralGroup)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int GroundPollution { get; set; }

        //{
        //            get => GetValue(nameof(GroundPollution), Defaults.GroundPollution);
        //            set => SetValue(nameof(GroundPollution), value, Save);
        //        }

        [SettingsUISection(Page1, GeneralGroup)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int AirPollution { get; set; }

        //{
        //            get => GetValue(nameof(AirPollution), Defaults.AirPollution);
        //            set => SetValue(nameof(AirPollution), value, Save);
        //        }

        [SettingsUISection(Page1, GeneralGroup)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int NoisePollution { get; set; }

        //{
        //            get => GetValue(nameof(NoisePollution), Defaults.NoisePollution);
        //            set => SetValue(nameof(NoisePollution), value, Save);
        //        }

        [SettingsUISection(Page1, GeneralGroup)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 10,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int EmployeeUpkeep { get; set; }

        //{
        //            get => GetValue(nameof(EmployeeUpkeep), Defaults.EmployeeUpkeep);
        //            set => SetValue(nameof(EmployeeUpkeep), value, Save);
        //        }

        [SettingsUISection(Page1, GeneralGroup)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 0,
            max = 100,
            step = 5,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int StorageUpkeep { get; set; }

        //{
        //            get => GetValue(nameof(StorageUpkeep), Defaults.StorageUpkeep);
        //            set => SetValue(nameof(StorageUpkeep), value, Save);
        //        }

        [SettingsUISection(Page1, GeneralGroup)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(
            min = 100,
            max = 5000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kPercentage
        )]
        public int Uniqueness { get; set; }

        //{
        //            get => GetValue(nameof(Uniqueness), Defaults.Uniqueness);
        //            set => SetValue(nameof(Uniqueness), value, Save);
        //        }

        //[SettingsUISection(Page1, InfoGroup)]
        //public bool VerboseLogging { get; set; } = false;

        [SettingsUISection(AboutTab, InfoGroup)]
        public string NameText => Mod.Name;

        [SettingsUISection(AboutTab, InfoGroup)]
        public string VersionText => VariableHelper.AddDevSuffix(Mod.Version);

        [SettingsUISection(AboutTab, InfoGroup)]
        public string AuthorText => VariableHelper.StarQ;

        [SettingsUIButton]
        [SettingsUIButtonGroup("Social")]
        [SettingsUISection(AboutTab, InfoGroup)]
        public bool BMaCLink
        {
            set => VariableHelper.OpenBMAC();
        }

        [SettingsUIMultilineText]
        [SettingsUIDisplayName(typeof(LogHelper), nameof(LogHelper.LogText))]
        [SettingsUISection(LogTab, "")]
        public string LogText => string.Empty;

        [Exclude]
        [SettingsUIHidden]
        public bool IsLogMissing
        {
            get => VariableHelper.CheckLog(Mod.Id);
        }

        [SettingsUIButton]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(IsLogMissing))]
        [SettingsUISection(LogTab, "")]
        public bool OpenLog
        {
            set => VariableHelper.OpenLog(Mod.Id);
        }
    }
}
