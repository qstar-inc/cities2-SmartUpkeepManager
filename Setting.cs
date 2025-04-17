using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI;
using SmartUpkeepManager.Systems;
using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine.Device;

namespace SmartUpkeepManager
{
    [FileLocation("ModsSettings\\StarQ\\" + nameof(SmartUpkeepManager))]
    [SettingsUITabOrder(Page1, Page2, Page3, AboutTab)]
    [SettingsUIGroupOrder(Buttons, General, Roads, Electricity, Water, Health, Garbage, Education, Fire, Police, Transportation, Parks, Communication, InfoGroup)]
    [SettingsUIShowGroupName(General, Roads, Electricity, Water, Health, Garbage, Education, Fire, Police, Transportation, Parks, Communication)]
    public partial class Setting : ModSetting
    {
        private static readonly SmartUpkeepSystem suS = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<SmartUpkeepSystem>();

        public const string Page1 = "Page 1";
        public const string Page2 = "Page 2";
        public const string Page3 = "Page 3";
        public const string Buttons = "Save Changes";
        public const string General = "General";
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

        public const string AboutTab = "About";
        public const string InfoGroup = "Info";

        [SettingsUISection(Page1, Buttons)]
        public bool Disable
        {
            get => GetValue(nameof(Disable), false);
            set => SetValue(nameof(Disable), value, Save);
        }

        [SettingsUIButtonGroup("Options")]
        [SettingsUISection(Page1, Buttons)]
        public bool SaveButton
        {
            set
            {
                Save();
            }
        }

        [SettingsUIButtonGroup("Options")]
        [SettingsUISection(Page1, Buttons)]
        public bool ModDefault
        {
            set
            {
                SetDefaults();
            }
        }

        [SettingsUIButtonGroup("Options")]
        [SettingsUISection(Page1, Buttons)]
        public bool MakeFree
        {
            set
            {
                SetFree();
            }
        }

        private readonly Dictionary<string, object> _values = new();
        private bool _dontSave = false;

        private T GetValue<T>(string propertyName, T defaultValue = default)
        {
            if (_values.TryGetValue(propertyName, out var value))
            {
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch (InvalidCastException)
                {
                    Mod.log.Info($"Warning: Unable to cast setting '{propertyName}' to {typeof(T)}. Returning default.");
                }
            }
            return defaultValue;
        }

        private void SetValue<T>(string propertyName, T value, Action onChanged = null)
        {
            _values[propertyName] = value;
            if (!_dontSave)
                onChanged?.Invoke();
        }

        public void Save()
        {
            if (Disable)
            {
                suS.ResetToVanilla();
                return;
            }

            if (SmartUpkeepSystem.systemActive && !Disable && SmartUpkeepSystem.inGame)
                suS.SetUpkeep();
        }

        // Roads
        [SettingsUISection(Page1, Roads)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int RoadMaintenance
        {
            get => GetValue(nameof(RoadMaintenance), Defaults.RoadMaintenance);
            set => SetValue(nameof(RoadMaintenance), value, Save);
        }

        [SettingsUISection(Page1, Roads)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int SnowPloughing
        {
            get => GetValue(nameof(SnowPloughing), Defaults.SnowPloughing);
            set => SetValue(nameof(SnowPloughing), value, Save);
        }

        [SettingsUISection(Page1, Roads)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int Towing
        {
            get => GetValue(nameof(Towing), Defaults.Towing);
            set => SetValue(nameof(Towing), value, Save);
        }

        [SettingsUISection(Page1, Roads)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int RoadMaintenanceVehicle
        {
            get => GetValue(nameof(RoadMaintenanceVehicle), Defaults.RoadMaintenanceVehicle);
            set => SetValue(nameof(RoadMaintenanceVehicle), value, Save);
        }

        // Electricity
        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 50, step = 1, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int SolarPowered
        {
            get => GetValue(nameof(SolarPowered), Defaults.SolarPowered);
            set => SetValue(nameof(SolarPowered), value, Save);
        }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 50, step = 1, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int GroundWaterPowered
        {
            get => GetValue(nameof(GroundWaterPowered), Defaults.GroundWaterPowered);
            set => SetValue(nameof(GroundWaterPowered), value, Save);
        }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 50, step = 1, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int WaterPowered
        {
            get => GetValue(nameof(WaterPowered), Defaults.WaterPowered);
            set => SetValue(nameof(WaterPowered), value, Save);
        }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 50, step = 1, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int WindPowered
        {
            get => GetValue(nameof(WindPowered), Defaults.WindPowered);
            set => SetValue(nameof(WindPowered), value, Save);
        }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 50, step = 1, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int GarbagePowered
        {
            get => GetValue(nameof(GarbagePowered), Defaults.GarbagePowered);
            set => SetValue(nameof(GarbagePowered), value, Save);
        }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 50, step = 1, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int ElectricityProduction
        {
            get => GetValue(nameof(ElectricityProduction), Defaults.ElectricityProduction);
            set => SetValue(nameof(ElectricityProduction), value, Save);
        }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 50, step = 1, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int BatteryOut
        {
            get => GetValue(nameof(BatteryOut), Defaults.BatteryOut);
            set => SetValue(nameof(BatteryOut), value, Save);
        }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 50, step = 1, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int BatteryCap
        {
            get => GetValue(nameof(BatteryCap), Defaults.BatteryCap);
            set => SetValue(nameof(BatteryCap), value, Save);
        }

        [SettingsUISection(Page1, Electricity)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 5000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int Transformer
        {
            get => GetValue(nameof(Transformer), Defaults.Transformer);
            set => SetValue(nameof(Transformer), value, Save);
        }

        //Water & Sewage
        [SettingsUISection(Page1, Water)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 500, step = 1, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int WaterPumpCap
        {
            get => GetValue(nameof(WaterPumpCap), Defaults.WaterPumpCap);
            set => SetValue(nameof(WaterPumpCap), value, Save);
        }

        [SettingsUISection(Page1, Water)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 500, step = 1, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int SewageOutCap
        {
            get => GetValue(nameof(SewageOutCap), Defaults.SewageOutCap);
            set => SetValue(nameof(SewageOutCap), value, Save);
        }

        [SettingsUISection(Page1, Water)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 30000, step = 1, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int Purification
        {
            get => GetValue(nameof(Purification), Defaults.Purification);
            set => SetValue(nameof(Purification), value, Save);
        }

        //Healthcare & Deathcare
        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int Ambulance
        {
            get => GetValue(nameof(Ambulance), Defaults.Ambulance);
            set => SetValue(nameof(Ambulance), value, Save);
        }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 50000, step = 1000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int MedicalHelicopter
        {
            get => GetValue(nameof(MedicalHelicopter), Defaults.MedicalHelicopter);
            set => SetValue(nameof(MedicalHelicopter), value, Save);
        }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 5000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int Patient
        {
            get => GetValue(nameof(Patient), Defaults.Patient);
            set => SetValue(nameof(Patient), value, Save);
        }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int HealthBonus
        {
            get => GetValue(nameof(HealthBonus), Defaults.HealthBonus);
            set => SetValue(nameof(HealthBonus), value, Save);
        }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 5000, step = 250, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int HealthRange
        {
            get => GetValue(nameof(HealthRange), Defaults.HealthRange);
            set => SetValue(nameof(HealthRange), value, Save);
        }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 100000, step = 10000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int Treatment
        {
            get => GetValue(nameof(Treatment), Defaults.Treatment);
            set => SetValue(nameof(Treatment), value, Save);
        }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int Hearse
        {
            get => GetValue(nameof(Hearse), Defaults.Hearse);
            set => SetValue(nameof(Hearse), value, Save);
        }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 5000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int BodyStorage
        {
            get => GetValue(nameof(BodyStorage), Defaults.BodyStorage);
            set => SetValue(nameof(BodyStorage), value, Save);
        }

        [SettingsUISection(Page2, Health)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 5000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int BodyProcessing
        {
            get => GetValue(nameof(BodyProcessing), Defaults.BodyProcessing);
            set => SetValue(nameof(BodyProcessing), value, Save);
        }

        // Garbage Management
        [SettingsUISection(Page1, Garbage)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 500, step = 10, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int GarbageCap
        {
            get => GetValue(nameof(GarbageCap), Defaults.GarbageCap);
            set => SetValue(nameof(GarbageCap), value, Save);
        }

        [SettingsUISection(Page1, Garbage)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int GarbageTruck
        {
            get => GetValue(nameof(GarbageTruck), Defaults.GarbageTruck);
            set => SetValue(nameof(GarbageTruck), value, Save);
        }

        [SettingsUISection(Page1, Garbage)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int DumpTruck
        {
            get => GetValue(nameof(DumpTruck), Defaults.DumpTruck);
            set => SetValue(nameof(DumpTruck), value, Save);
        }

        [SettingsUISection(Page1, Garbage)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 1000, step = 10, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int GarbageProcessing
        {
            get => GetValue(nameof(GarbageProcessing), Defaults.GarbageProcessing);
            set => SetValue(nameof(GarbageProcessing), value, Save);
        }

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
        [SettingsUISlider(min = 0, max = 100, step = 1, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int Student
        {
            get => GetValue(nameof(Student), Defaults.Student);
            set => SetValue(nameof(Student), value, Save);
        }

        [SettingsUISection(Page3, Education)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int StudentGraduation
        {
            get => GetValue(nameof(StudentGraduation), Defaults.StudentGraduation);
            set => SetValue(nameof(StudentGraduation), value, Save);
        }

        [SettingsUISection(Page3, Education)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int StudentWellbeing
        {
            get => GetValue(nameof(StudentWellbeing), Defaults.StudentWellbeing);
            set => SetValue(nameof(StudentWellbeing), value, Save);
        }

        [SettingsUISection(Page3, Education)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int StudentHealth
        {
            get => GetValue(nameof(StudentHealth), Defaults.StudentHealth);
            set => SetValue(nameof(StudentHealth), value, Save);
        }

        [SettingsUISection(Page3, Education)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 200000, step = 5000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int ResearchFacility
        {
            get => GetValue(nameof(ResearchFacility), Defaults.ResearchFacility);
            set => SetValue(nameof(ResearchFacility), value, Save);
        }

        // Fire & Rescue
        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int FireTruck
        {
            get => GetValue(nameof(FireTruck), Defaults.FireTruck);
            set => SetValue(nameof(FireTruck), value, Save);
        }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 50000, step = 1000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int FireHelicopter
        {
            get => GetValue(nameof(FireHelicopter), Defaults.FireHelicopter);
            set => SetValue(nameof(FireHelicopter), value, Save);
        }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 50000, step = 1000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int FireDisasterCap
        {
            get => GetValue(nameof(FireDisasterCap), Defaults.FireDisasterCap);
            set => SetValue(nameof(FireDisasterCap), value, Save);
        }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 500, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int FireVehicleEffi
        {
            get => GetValue(nameof(FireVehicleEffi), Defaults.FireVehicleEffi);
            set => SetValue(nameof(FireVehicleEffi), value, Save);
        }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 20000, step = 500, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int Firewatch
        {
            get => GetValue(nameof(Firewatch), Defaults.Firewatch);
            set => SetValue(nameof(Firewatch), value, Save);
        }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 30000, step = 1000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int EarlyDisasterWarningSystem
        {
            get => GetValue(nameof(EarlyDisasterWarningSystem), Defaults.EarlyDisasterWarningSystem);
            set => SetValue(nameof(EarlyDisasterWarningSystem), value, Save);
        }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 50000, step = 1000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int DisasterFacility
        {
            get => GetValue(nameof(DisasterFacility), Defaults.DisasterFacility);
            set => SetValue(nameof(DisasterFacility), value, Save);
        }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 5000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int ShelterCap
        {
            get => GetValue(nameof(ShelterCap), Defaults.ShelterCap);
            set => SetValue(nameof(ShelterCap), value, Save);
        }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int EvacuationBus
        {
            get => GetValue(nameof(EvacuationBus), Defaults.EvacuationBus);
            set => SetValue(nameof(EvacuationBus), value, Save);
        }

        [SettingsUISection(Page2, Fire)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 50000, step = 1000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int EmergencyGenerator
        {
            get => GetValue(nameof(EmergencyGenerator), Defaults.EmergencyGenerator);
            set => SetValue(nameof(EmergencyGenerator), value, Save);
        }

        // Police & Administration
        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int PatrolCar
        {
            get => GetValue(nameof(PatrolCar), Defaults.PatrolCar);
            set => SetValue(nameof(PatrolCar), value, Save);
        }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 50000, step = 1000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int PoliceHelicopter
        {
            get => GetValue(nameof(PoliceHelicopter), Defaults.PoliceHelicopter);
            set => SetValue(nameof(PoliceHelicopter), value, Save);
        }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 20000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int LocalJail
        {
            get => GetValue(nameof(LocalJail), Defaults.LocalJail);
            set => SetValue(nameof(LocalJail), value, Save);
        }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 200000, step = 5000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int Patrol
        {
            get => GetValue(nameof(Patrol), Defaults.Patrol);
            set => SetValue(nameof(Patrol), value, Save);
        }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 200000, step = 5000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int EmergencyPolice
        {
            get => GetValue(nameof(EmergencyPolice), Defaults.EmergencyPolice);
            set => SetValue(nameof(EmergencyPolice), value, Save);
        }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 200000, step = 5000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int Intelligence
        {
            get => GetValue(nameof(Intelligence), Defaults.Intelligence);
            set => SetValue(nameof(Intelligence), value, Save);
        }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int PrisonVan
        {
            get => GetValue(nameof(PrisonVan), Defaults.PrisonVan);
            set => SetValue(nameof(PrisonVan), value, Save);
        }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 20000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int PrisonerCap
        {
            get => GetValue(nameof(PrisonerCap), Defaults.PrisonerCap);
            set => SetValue(nameof(PrisonerCap), value, Save);
        }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int PrisonerWellbeing
        {
            get => GetValue(nameof(PrisonerWellbeing), Defaults.PrisonerWellbeing);
            set => SetValue(nameof(PrisonerWellbeing), value, Save);
        }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int PrisonerHealth
        {
            get => GetValue(nameof(PrisonerHealth), Defaults.PrisonerHealth);
            set => SetValue(nameof(PrisonerHealth), value, Save);
        }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 200000, step = 5000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int WelfareOffice
        {
            get => GetValue(nameof(WelfareOffice), Defaults.WelfareOffice);
            set => SetValue(nameof(WelfareOffice), value, Save);
        }

        [SettingsUISection(Page2, Police)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 200000, step = 5000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int AdminBuilding
        {
            get => GetValue(nameof(AdminBuilding), Defaults.AdminBuilding);
            set => SetValue(nameof(AdminBuilding), value, Save);
        }

        // Transportation
        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 5000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int PlatformMaintenance
        {
            get => GetValue(nameof(PlatformMaintenance), Defaults.PlatformMaintenance);
            set => SetValue(nameof(PlatformMaintenance), value, Save);
        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int Bus
        {
            get => GetValue(nameof(Bus), Defaults.Bus);
            set => SetValue(nameof(Bus), value, Save);
        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 20000, step = 1000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int Train
        {
            get => GetValue(nameof(Train), Defaults.Train);
            set => SetValue(nameof(Train), value, Save);
        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int Taxi
        {
            get => GetValue(nameof(Taxi), Defaults.Taxi);
            set => SetValue(nameof(Taxi), value, Save);
        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 20000, step = 1000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int Tram
        {
            get => GetValue(nameof(Tram), Defaults.Tram);
            set => SetValue(nameof(Tram), value, Save);
        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 100000, step = 1000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int Ship
        {
            get => GetValue(nameof(Ship), Defaults.Ship);
            set => SetValue(nameof(Ship), value, Save);
        }

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
        [SettingsUISlider(min = 0, max = 50000, step = 5000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int Airplane
        {
            get => GetValue(nameof(Airplane), Defaults.Airplane);
            set => SetValue(nameof(Airplane), value, Save);
        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 20000, step = 1000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int Subway
        {
            get => GetValue(nameof(Subway), Defaults.Subway);
            set => SetValue(nameof(Subway), value, Save);
        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 100000, step = 5000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int Rocket
        {
            get => GetValue(nameof(Rocket), Defaults.Rocket);
            set => SetValue(nameof(Rocket), value, Save);
        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 50000, step = 1000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int EnergyFuel
        {
            get => GetValue(nameof(EnergyFuel), Defaults.EnergyFuel);
            set => SetValue(nameof(EnergyFuel), value, Save);
        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 50000, step = 1000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int EnergyElectricity
        {
            get => GetValue(nameof(EnergyElectricity), Defaults.EnergyElectricity);
            set => SetValue(nameof(EnergyElectricity), value, Save);
        }

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
        [SettingsUISlider(min = 0, max = 50000, step = 1000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int MaintenanceBoost
        {
            get => GetValue(nameof(MaintenanceBoost), Defaults.MaintenanceBoost);
            set => SetValue(nameof(MaintenanceBoost), value, Save);
        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int DispatchCenter
        {
            get => GetValue(nameof(DispatchCenter), Defaults.DispatchCenter);
            set => SetValue(nameof(DispatchCenter), value, Save);
        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int TradedResource
        {
            get => GetValue(nameof(TradedResource), Defaults.TradedResource);
            set => SetValue(nameof(TradedResource), value, Save);
        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int DeliveryTruck
        {
            get => GetValue(nameof(DeliveryTruck), Defaults.DeliveryTruck);
            set => SetValue(nameof(DeliveryTruck), value, Save);
        }

        [SettingsUISection(Page3, Transportation)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 100, step = 1, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int ComfortFactor
        {
            get => GetValue(nameof(ComfortFactor), Defaults.ComfortFactor);
            set => SetValue(nameof(ComfortFactor), value, Save);
        }

        // Parks & Recreation
        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 30000, step = 500, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int ParkMaintenance
        {
            get => GetValue(nameof(ParkMaintenance), Defaults.ParkMaintenance);
            set => SetValue(nameof(ParkMaintenance), value, Save);
        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int ParkMaintenanceVehicle
        {
            get => GetValue(nameof(ParkMaintenanceVehicle), Defaults.ParkMaintenanceVehicle);
            set => SetValue(nameof(ParkMaintenanceVehicle), value, Save);
        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 5000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int LeisureEfficieny
        {
            get => GetValue(nameof(LeisureEfficieny), Defaults.LeisureEfficieny);
            set => SetValue(nameof(LeisureEfficieny), value, Save);
        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 1000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int LeisureMeals
        {
            get => GetValue(nameof(LeisureMeals), Defaults.LeisureMeals);
            set => SetValue(nameof(LeisureMeals), value, Save);
        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 1000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int LeisureEntertainment
        {
            get => GetValue(nameof(LeisureEntertainment), Defaults.LeisureEntertainment);
            set => SetValue(nameof(LeisureEntertainment), value, Save);
        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 1000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int LeisureCommercial
        {
            get => GetValue(nameof(LeisureCommercial), Defaults.LeisureCommercial);
            set => SetValue(nameof(LeisureCommercial), value, Save);
        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 1000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int LeisureCityIndoors
        {
            get => GetValue(nameof(LeisureCityIndoors), Defaults.LeisureCityIndoors);
            set => SetValue(nameof(LeisureCityIndoors), value, Save);
        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 1000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int LeisureTravel
        {
            get => GetValue(nameof(LeisureTravel), Defaults.LeisureTravel);
            set => SetValue(nameof(LeisureTravel), value, Save);
        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 1000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int LeisureCityPark
        {
            get => GetValue(nameof(LeisureCityPark), Defaults.LeisureCityPark);
            set => SetValue(nameof(LeisureCityPark), value, Save);
        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 1000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int LeisureCityBeach
        {
            get => GetValue(nameof(LeisureCityBeach), Defaults.LeisureCityBeach);
            set => SetValue(nameof(LeisureCityBeach), value, Save);
        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 1000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int LeisureAttractions
        {
            get => GetValue(nameof(LeisureAttractions), Defaults.LeisureAttractions);
            set => SetValue(nameof(LeisureAttractions), value, Save);
        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 1000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int LeisureRelaxation
        {
            get => GetValue(nameof(LeisureRelaxation), Defaults.LeisureRelaxation);
            set => SetValue(nameof(LeisureRelaxation), value, Save);
        }

        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 1000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int LeisureSightseeing
        {
            get => GetValue(nameof(LeisureSightseeing), Defaults.LeisureSightseeing);
            set => SetValue(nameof(LeisureSightseeing), value, Save);
        }
        
        [SettingsUISection(Page3, Parks)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 1000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int Attraction
        {
            get => GetValue(nameof(Attraction), Defaults.Attraction);
            set => SetValue(nameof(Attraction), value, Save);
        }

        // Communication
        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int PostVan
        {
            get => GetValue(nameof(PostVan), Defaults.PostVan);
            set => SetValue(nameof(PostVan), value, Save);
        }

        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int PostTruck
        {
            get => GetValue(nameof(PostTruck), Defaults.PostTruck);
            set => SetValue(nameof(PostTruck), value, Save);
        }

        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 200, step = 10, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int MailCap
        {
            get => GetValue(nameof(MailCap), Defaults.MailCap);
            set => SetValue(nameof(MailCap), value, Save);
        }

        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 5000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int PostSortingRate
        {
            get => GetValue(nameof(PostSortingRate), Defaults.PostSortingRate);
            set => SetValue(nameof(PostSortingRate), value, Save);
        }

        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 15000, step = 1000, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int TelecomRange
        {
            get => GetValue(nameof(TelecomRange), Defaults.TelecomRange);
            set => SetValue(nameof(TelecomRange), value, Save);
        }

        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 2000, step = 50, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int NetworkCap
        {
            get => GetValue(nameof(NetworkCap), Defaults.NetworkCap);
            set => SetValue(nameof(NetworkCap), value, Save);
        }

        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 20000, step = 500, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int Wireless
        {
            get => GetValue(nameof(Wireless), Defaults.Wireless);
            set => SetValue(nameof(Wireless), value, Save);
        }

        [SettingsUISection(Page1, Communication)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 50000, step = 500, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int Fibre
        {
            get => GetValue(nameof(Fibre), Defaults.Fibre);
            set => SetValue(nameof(Fibre), value, Save);
        }

        // Landscaping

        // General

        [SettingsUISection(Page1, General)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 100, step = 2, scalarMultiplier = 1, unit = Unit.kPercentage)]
        public bool ServiceBudgetMultiplier
        {
            get => GetValue(nameof(ServiceBudgetMultiplier), true);
            set => SetValue(nameof(ServiceBudgetMultiplier), value, Save);
        }

        [SettingsUISection(Page1, General)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 100, step = 2, scalarMultiplier = 1, unit = Unit.kPercentage)]
        public bool CityBonusMultiplier
        {
            get => GetValue(nameof(CityBonusMultiplier), true);
            set => SetValue(nameof(CityBonusMultiplier), value, Save);
        }

        [SettingsUISection(Page1, General)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 1000, step = 100, scalarMultiplier = 1, unit = Unit.kPercentage)]
        public int ServiceCoverageMultiplier
        {
            get => GetValue(nameof(ServiceCoverageMultiplier), Defaults.ServiceCoverageMultiplier);
            set => SetValue(nameof(ServiceCoverageMultiplier), value, Save);
        }

        [SettingsUISection(Page1, General)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 1000, step = 10, scalarMultiplier = 1, unit = Unit.kMoneyPerCell)]
        public int PlotPrice
        {
            get => GetValue(nameof(PlotPrice), Defaults.PlotPrice);
            set => SetValue(nameof(PlotPrice), value, Save);
        }

        [SettingsUISection(Page1, Roads)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 500, step = 10, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int ParkingSpots
        {
            get => GetValue(nameof(ParkingSpots), Defaults.ParkingSpots);
            set => SetValue(nameof(ParkingSpots), value, Save);
        }

        [SettingsUISection(Page1, General)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int GroundPollution
        {
            get => GetValue(nameof(GroundPollution), Defaults.GroundPollution);
            set => SetValue(nameof(GroundPollution), value, Save);
        }

        [SettingsUISection(Page1, General)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int AirPollution
        {
            get => GetValue(nameof(AirPollution), Defaults.AirPollution);
            set => SetValue(nameof(AirPollution), value, Save);
        }

        [SettingsUISection(Page1, General)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 10000, step = 100, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int NoisePollution
        {
            get => GetValue(nameof(NoisePollution), Defaults.NoisePollution);
            set => SetValue(nameof(NoisePollution), value, Save);
        }

        [SettingsUISection(Page1, General)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 1000, step = 10, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int EmployeeUpkeep
        {
            get => GetValue(nameof(EmployeeUpkeep), Defaults.EmployeeUpkeep);
            set => SetValue(nameof(EmployeeUpkeep), value, Save);
        }

        [SettingsUISection(Page1, General)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 0, max = 100, step = 5, scalarMultiplier = 1, unit = Unit.kMoneyPerMonth)]
        public int StorageUpkeep
        {
            get => GetValue(nameof(StorageUpkeep), Defaults.StorageUpkeep);
            set => SetValue(nameof(StorageUpkeep), value, Save);
        }

        [SettingsUISection(Page1, General)]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(Disable), false)]
        [SettingsUISlider(min = 100, max = 5000, step = 100, scalarMultiplier = 1, unit = Unit.kPercentage)]
        public int Uniqueness
        {
            get => GetValue(nameof(Uniqueness), Defaults.Uniqueness);
            set => SetValue(nameof(Uniqueness), value, Save);
        }

        [SettingsUISection(Page1, InfoGroup)]
        public bool VerboseLogging { get; set; } = false;

        [SettingsUISection(AboutTab, InfoGroup)]
        public string NameText => Mod.Name;

        [SettingsUISection(AboutTab, InfoGroup)]
        public string VersionText =>
# if DEBUG
            $"{Mod.Version} - DEV";
#else
            Mod.Version;
#endif

        [SettingsUISection(AboutTab, InfoGroup)]
        public string AuthorText => "StarQ";

        [SettingsUIButtonGroup("Social")]
        [SettingsUIButton]
        [SettingsUISection(AboutTab, InfoGroup)]
        public bool BMaCLink
        {
            set
            {
                try
                {
                    Application.OpenURL($"https://buymeacoffee.com/starq");
                }
                catch (Exception e)
                {
                    Mod.log.Info(e);
                }
            }
        }
    }
}
