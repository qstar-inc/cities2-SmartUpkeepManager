using Game.Modding;
using Game.Settings;
using Game.UI;
using StarQ.Shared.Generators;

namespace SmartUpkeepManager
{
    [GenerateSettingCommonAttribute]
    [SettingsUITabOrder(Page1, Page2, Page3, AboutTab)]
    [SettingsUIGroupOrder(
        Buttons,
        Generals,
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
        Generals,
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
        public const string Buttons = "Buttons";
        public const string Generals = "Generals";
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

        [SettingsUISection(Page1, Buttons)]
        public bool EnableMod { get; set; }

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

        // Roads
        [SettingsUISection(Page1, Roads)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int RoadMaintenance { get; set; }

        [SettingsUISection(Page1, Roads)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int SnowPloughing { get; set; }

        [SettingsUISection(Page1, Roads)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Towing { get; set; }

        [SettingsUISection(Page1, Roads)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int RoadMaintenanceVehicle { get; set; }

        // Electricity
        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 50,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int SolarPowered { get; set; }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 50,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int GroundWaterPowered { get; set; }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 50,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int WaterPowered { get; set; }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 50,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int WindPowered { get; set; }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 50,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int GarbagePowered { get; set; }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 50,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int ElectricityProduction { get; set; }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 50,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int BatteryOut { get; set; }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 50,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int BatteryCap { get; set; }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 5000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Transformer { get; set; }

        //Water & Sewage
        [SettingsUISection(Page1, Water)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 500,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int WaterPumpCap { get; set; }

        [SettingsUISection(Page1, Water)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 500,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int SewageOutCap { get; set; }

        [SettingsUISection(Page1, Water)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 30000,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Purification { get; set; }

        //Healthcare & Deathcare
        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Ambulance { get; set; }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 50000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int MedicalHelicopter { get; set; }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Patient { get; set; }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 3000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int HealthBonus { get; set; }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 5000,
            step = 250,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int HealthRange { get; set; }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 100000,
            step = 10000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Treatment { get; set; }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Hearse { get; set; }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 2000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int BodyStorage { get; set; }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 2000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int BodyProcessing { get; set; }

        // Garbage Management
        [SettingsUISection(Page1, Garbage)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 500,
            step = 10,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int GarbageCap { get; set; }

        [SettingsUISection(Page1, Garbage)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int GarbageTruck { get; set; }

        [SettingsUISection(Page1, Garbage)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int DumpTruck { get; set; }

        [SettingsUISection(Page1, Garbage)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 10,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int GarbageProcessing { get; set; }

        // Education & Research
        [SettingsUISection(Page3, Education)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 100,
            step = 1,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Student { get; set; }

        [SettingsUISection(Page3, Education)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int StudentGraduation { get; set; }

        [SettingsUISection(Page3, Education)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int StudentWellbeing { get; set; }

        [SettingsUISection(Page3, Education)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int StudentHealth { get; set; }

        [SettingsUISection(Page3, Education)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 200000,
            step = 5000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int ResearchFacility { get; set; }

        // Fire & Rescue
        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int FireTruck { get; set; }

        
        [SettingsUIDescription()]
        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 50000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int FireHelicopter { get; set; }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 50000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int FireDisasterCap { get; set; }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 500,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int FireVehicleEffi { get; set; }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 20000,
            step = 500,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Firewatch { get; set; }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 30000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int EarlyDisasterWarningSystem { get; set; }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 50000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int DisasterFacility { get; set; }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 5000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int ShelterCap { get; set; }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int EvacuationBus { get; set; }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 5000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int EmergencyGenerator { get; set; }

        // Police & Administration
        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int PatrolCar { get; set; }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 50000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int PoliceHelicopter { get; set; }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 5000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LocalJail { get; set; }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 200000,
            step = 5000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Patrol { get; set; }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 200000,
            step = 5000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int EmergencyPolice { get; set; }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 200000,
            step = 5000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Intelligence { get; set; }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int PrisonVan { get; set; }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 5000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int PrisonerCap { get; set; }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int PrisonerWellbeing { get; set; }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int PrisonerHealth { get; set; }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 200000,
            step = 5000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int WelfareOffice { get; set; }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 200000,
            step = 5000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int AdminBuilding { get; set; }

        // Transportation
        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 5000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int PlatformMaintenance { get; set; }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Bus { get; set; }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 20000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Train { get; set; }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Taxi { get; set; }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 20000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Tram { get; set; }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 100000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Ship { get; set; }

        //[SettingsUISection(Page3, Transportation)]
        //[SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        //[SettingsUISlider(
        //    min = 0,
        //    max = 100000,
        //    step = 1000,
        //    scalarMultiplier = 1,
        //    unit = Unit.kMoneyPerMonth
        //)]
        //public int Post { get; set; }

        //[SettingsUISection(Page3, Transportation)]
        //[SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        //[SettingsUISlider(
        //    min = 0,
        //    max = 100000,
        //    step = 1000,
        //    scalarMultiplier = 1,
        //    unit = Unit.kMoneyPerMonth
        //)]
        //public int Helicopter { get; set; }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 50000,
            step = 5000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Airplane { get; set; }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 20000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Subway { get; set; }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 100000,
            step = 5000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Rocket { get; set; }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 500,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int EnergyFuel { get; set; }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 500,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int EnergyElectricity { get; set; }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 50000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int MaintenanceBoost { get; set; }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int DispatchCenter { get; set; }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int TradedResource { get; set; }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int DeliveryTruck { get; set; }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 10,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int ComfortFactor { get; set; }

        // Parks & Recreation
        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 30000,
            step = 500,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int ParkMaintenance { get; set; }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int ParkMaintenanceVehicle { get; set; }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 5000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LeisureEfficieny { get; set; }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LeisureMeals { get; set; }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LeisureEntertainment { get; set; }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LeisureCommercial { get; set; }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LeisureCityIndoors { get; set; }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LeisureTravel { get; set; }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LeisureCityPark { get; set; }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LeisureCityBeach { get; set; }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LeisureAttractions { get; set; }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LeisureRelaxation { get; set; }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int LeisureSightseeing { get; set; }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Attraction { get; set; }

        // Communication
        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int PostVan { get; set; }

        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int PostTruck { get; set; }

        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 200,
            step = 10,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int MailCap { get; set; }

        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 5000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int PostSortingRate { get; set; }

        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 15000,
            step = 1000,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int TelecomRange { get; set; }

        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 2000,
            step = 50,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int NetworkCap { get; set; }

        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 20000,
            step = 500,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Wireless { get; set; }

        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 50000,
            step = 500,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int Fibre { get; set; }

        // Landscaping

        // General

        [SettingsUISection(Page1, Generals)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 100,
            step = 2,
            scalarMultiplier = 1,
            unit = Unit.kPercentage
        )]
        public bool ServiceBudgetMultiplier { get; set; }

        [SettingsUISection(Page1, Generals)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 100,
            step = 2,
            scalarMultiplier = 1,
            unit = Unit.kPercentage
        )]
        public bool CityBonusMultiplier { get; set; }

        [SettingsUISection(Page1, Generals)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kPercentage
        )]
        public int ServiceCoverageMultiplier { get; set; }

        [SettingsUISection(Page1, Generals)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 10,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerCell
        )]
        public int PlotPrice { get; set; }

        [SettingsUISection(Page1, Roads)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 500,
            step = 10,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int ParkingSpots { get; set; }

        [SettingsUISection(Page1, Generals)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int GroundPollution { get; set; }

        [SettingsUISection(Page1, Generals)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int AirPollution { get; set; }

        [SettingsUISection(Page1, Generals)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 10000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int NoisePollution { get; set; }

        [SettingsUISection(Page1, Generals)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 1000,
            step = 10,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int EmployeeUpkeep { get; set; }

        [SettingsUISection(Page1, Generals)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 0,
            max = 100,
            step = 5,
            scalarMultiplier = 1,
            unit = Unit.kMoneyPerMonth
        )]
        public int StorageUpkeep { get; set; }

        [SettingsUISection(Page1, Generals)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(EnableMod), true)]
        [SettingsUISlider(
            min = 100,
            max = 5000,
            step = 100,
            scalarMultiplier = 1,
            unit = Unit.kPercentage
        )]
        public int Uniqueness { get; set; }
    }
}
