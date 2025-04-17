using Game.Modding;
using Game.Vehicles;
using UnityEngine;

namespace SmartUpkeepManager
{
    public class Defaults
    {
        // Roads
        public static readonly int ParkingSpots = 50;
        public static readonly int RoadMaintenance = 5000;
        public static readonly int SnowPloughing = 2000;
        public static readonly int Towing = 3000;
        public static readonly int RoadMaintenanceVehicle = 800;
        
        // Electricity
        public static readonly int SolarPowered = 5;
        public static readonly int GroundWaterPowered = 10;
        public static readonly int WaterPowered = 10;
        public static readonly int WindPowered = 5;
        public static readonly int GarbagePowered = 15;
        public static readonly int ElectricityProduction = 15;
        public static readonly int BatteryOut = 20;
        public static readonly int BatteryCap = 20;
        public static readonly int Transformer = 500;

        //Water & Sewage
        public static readonly int WaterPumpCap = 20;
        public static readonly int SewageOutCap = 50;
        public static readonly int Purification = 5000;

        //Healthcare & Deathcare
        public static readonly int Ambulance = 1000;
        public static readonly int MedicalHelicopter = 5000;
        public static readonly int Patient = 100;
        public static readonly int HealthBonus = 2000;
        public static readonly int HealthRange = 250;
        public static readonly int Treatment = 30000;
        public static readonly int Hearse = 1000;
        public static readonly int BodyStorage = 500;
        public static readonly int BodyProcessing = 500;

        // Garbage Management
        public static readonly int GarbageCap = 20;
        public static readonly int GarbageTruck = 1000;
        public static readonly int DumpTruck = 1000;
        public static readonly int GarbageProcessing = 100;

        // Education & Research
        public static readonly int StudentPrimary = 300;
        public static readonly int StudentSecondary = 500;
        public static readonly int StudentTertiary = 800;
        public static readonly int StudentUniversity = 1000;
        public static readonly int StudentGraduation = 5000;
        public static readonly int StudentWellbeing = 5000;
        public static readonly int StudentHealth = 5000;
        public static readonly int ResearchFacility = 50000;

        // Fire & Rescue
        public static readonly int FireTruck = 1000;
        public static readonly int FireHelicopter = 5000;
        public static readonly int FireDisasterCap = 5000;
        public static readonly int FireVehicleEffi = 2000;
        public static readonly int Firewatch = 3000;
        public static readonly int EarlyDisasterWarningSystem = 10000;
        public static readonly int DisasterFacility = 10000;
        public static readonly int ShelterCap = 500;
        public static readonly int EvacuationBus = 1000;
        public static readonly int EmergencyGenerator = 10000;

        // Police & Administration
        public static readonly int PatrolCar = 600;
        public static readonly int PoliceHelicopter = 5000;
        public static readonly int LocalJail = 100;
        public static readonly int Patrol = 5000;
        public static readonly int EmergencyPolice = 10000;
        public static readonly int Intelligence = 50000;
        public static readonly int PrisonVan = 600;
        public static readonly int PrisonerCap = 100;
        public static readonly int PrisonerWellbeing = 5000;
        public static readonly int PrisonerHealth = 5000;
        public static readonly int WelfareOffice = 15000;
        public static readonly int AdminBuilding = 50000;

        // Transportation
        public static readonly int PlatformMaintenance = 500;
        public static readonly int Bus = 1000;
        public static readonly int Train = 5000;
        public static readonly int Taxi = 500;
        public static readonly int Tram = 1000;
        public static readonly int Ship = 20000;
        //public static readonly int Post = 500;
        //public static readonly int Helicopter = 10000;
        public static readonly int Airplane = 15000;
        public static readonly int Subway = 3000;
        public static readonly int Rocket = 50000;
        public static readonly int EnergyFuel = 5000;
        public static readonly int EnergyElectricity = 8000;
        //public static readonly int ProductionBoost = 1000;
        public static readonly int MaintenanceBoost = 20000;
        public static readonly int DispatchCenter = 500;
        public static readonly int TradedResource = 1000;
        public static readonly int DeliveryTruck = 1000;
        public static readonly int ComfortFactor = 5;

        // Parks & Recreation
        public static readonly int ParkMaintenance = 7500;
        public static readonly int ParkMaintenanceVehicle = 1000;
        public static readonly int LeisureEfficieny = 100;
        public static readonly int LeisureMeals = 50;
        public static readonly int LeisureEntertainment = 80;
        public static readonly int LeisureCommercial = 100;
        public static readonly int LeisureCityIndoors = 75;
        public static readonly int LeisureTravel = 300;
        public static readonly int LeisureCityPark = 50;
        public static readonly int LeisureCityBeach = 100;
        public static readonly int LeisureAttractions = 200;
        public static readonly int LeisureRelaxation = 200;
        public static readonly int LeisureSightseeing = 300;
        public static readonly int Attraction = 100;

        // Communication
        public static readonly int PostVan = 500;
        public static readonly int PostTruck = 1000;
        public static readonly int MailCap = 80;
        public static readonly int PostSortingRate = 600;
        public static readonly int TelecomRange = 1000;
        public static readonly int NetworkCap = 500;
        public static readonly int Wireless = 2500;
        public static readonly int Fibre = 5000;

        // Landscaping

        // General
        public static readonly int ServiceCoverageMultiplier = 100;
        public static readonly int PlotPrice = 50;
        public static readonly int Uniqueness = 300;
        public static readonly int GroundPollution = 2000;
        public static readonly int AirPollution = 2000;
        public static readonly int NoisePollution = 1000;
        public static readonly int EmployeeUpkeep = 100;
        public static readonly int StorageUpkeep = 10;
    }

    public partial class Setting : ModSetting
    {
        public Setting(IMod mod) : base(mod)
        {
            SetDefaults();
        }

        public override void SetDefaults()
        {
            _dontSave = true;
            Disable = false;
#if DEBUG
            VerboseLogging = true;
#else
            VerboseLogging = false;
#endif

            ParkingSpots = Defaults.ParkingSpots;
            RoadMaintenance = Defaults.RoadMaintenance;
            SnowPloughing = Defaults.SnowPloughing;
            Towing = Defaults.Towing;
            RoadMaintenanceVehicle = Defaults.RoadMaintenanceVehicle;
            SolarPowered = Defaults.SolarPowered;
            GroundWaterPowered = Defaults.GroundWaterPowered;
            WaterPowered = Defaults.WaterPowered;
            WindPowered = Defaults.WindPowered;
            GarbagePowered = Defaults.GarbagePowered;
            ElectricityProduction = Defaults.ElectricityProduction;
            BatteryOut = Defaults.BatteryOut;
            BatteryCap = Defaults.BatteryCap;
            Transformer = Defaults.Transformer;
            WaterPumpCap = Defaults.WaterPumpCap;
            SewageOutCap = Defaults.SewageOutCap;
            Purification = Defaults.Purification;
            Ambulance = Defaults.Ambulance;
            MedicalHelicopter = Defaults.MedicalHelicopter;
            Patient = Defaults.Patient;
            HealthBonus = Defaults.HealthBonus;
            HealthRange = Defaults.HealthRange;
            Treatment = Defaults.Treatment;
            Hearse = Defaults.Hearse;
            BodyStorage = Defaults.BodyStorage;
            BodyProcessing = Defaults.BodyProcessing;
            GarbageCap = Defaults.GarbageCap;
            GarbageTruck = Defaults.GarbageTruck;
            DumpTruck = Defaults.DumpTruck;
            GarbageProcessing = Defaults.GarbageProcessing;
            StudentPrimary = Defaults.StudentPrimary;
            StudentSecondary = Defaults.StudentSecondary;
            StudentTertiary = Defaults.StudentTertiary;
            StudentUniversity = Defaults.StudentUniversity;
            StudentGraduation = Defaults.StudentGraduation;
            StudentWellbeing = Defaults.StudentWellbeing;
            StudentHealth = Defaults.StudentHealth;
            ResearchFacility = Defaults.ResearchFacility;
            FireTruck = Defaults.FireTruck;
            FireHelicopter = Defaults.FireHelicopter;
            FireDisasterCap = Defaults.FireDisasterCap;
            FireVehicleEffi = Defaults.FireVehicleEffi;
            Firewatch = Defaults.Firewatch;
            EarlyDisasterWarningSystem = Defaults.EarlyDisasterWarningSystem;
            DisasterFacility = Defaults.DisasterFacility;
            ShelterCap = Defaults.ShelterCap;
            EvacuationBus = Defaults.EvacuationBus;
            EmergencyGenerator = Defaults.EmergencyGenerator;
            PatrolCar = Defaults.PatrolCar;
            PoliceHelicopter = Defaults.PoliceHelicopter;
            LocalJail = Defaults.LocalJail;
            Patrol = Defaults.Patrol;
            EmergencyPolice = Defaults.EmergencyPolice;
            Intelligence = Defaults.Intelligence;
            PrisonVan = Defaults.PrisonVan;
            PrisonerCap = Defaults.PrisonerCap;
            PrisonerWellbeing = Defaults.PrisonerWellbeing;
            PrisonerHealth = Defaults.PrisonerHealth;
            WelfareOffice = Defaults.WelfareOffice;
            AdminBuilding = Defaults.AdminBuilding;
            Bus = Defaults.Bus;
            Train = Defaults.Train;
            Taxi = Defaults.Taxi;
            Tram = Defaults.Tram;
            Ship = Defaults.Ship;
            //Post = Defaults.Post;
            //Helicopter = Defaults.Helicopter;
            Airplane = Defaults.Airplane;
            Subway = Defaults.Subway;
            Rocket = Defaults.Rocket;
            EnergyFuel = Defaults.EnergyFuel;
            EnergyElectricity = Defaults.EnergyElectricity;
            //ProductionBoost = Defaults.ProductionBoost;
            MaintenanceBoost = Defaults.MaintenanceBoost;
            DispatchCenter = Defaults.DispatchCenter;
            TradedResource = Defaults.TradedResource;
            DeliveryTruck = Defaults.DeliveryTruck;
            ComfortFactor = Defaults.ComfortFactor;
            ParkMaintenance = Defaults.ParkMaintenance;
            ParkMaintenanceVehicle = Defaults.ParkMaintenanceVehicle;
            LeisureEfficieny = Defaults.LeisureEfficieny;
            LeisureMeals = Defaults.LeisureMeals;
            LeisureEntertainment = Defaults.LeisureEntertainment;
            LeisureCommercial = Defaults.LeisureCommercial;
            LeisureCityIndoors = Defaults.LeisureCityIndoors;
            LeisureTravel = Defaults.LeisureTravel;
            LeisureCityPark = Defaults.LeisureCityPark;
            LeisureCityBeach = Defaults.LeisureCityBeach;
            LeisureAttractions = Defaults.LeisureAttractions;
            LeisureRelaxation = Defaults.LeisureRelaxation;
            LeisureSightseeing = Defaults.LeisureSightseeing;
            Attraction = Defaults.Attraction;
            PostVan = Defaults.PostVan;
            PostTruck = Defaults.PostTruck;
            MailCap = Defaults.MailCap;
            PostSortingRate = Defaults.PostSortingRate;
            TelecomRange = Defaults.TelecomRange;
            NetworkCap = Defaults.NetworkCap;
            Wireless = Defaults.Wireless;
            Fibre = Defaults.Fibre;
            ServiceCoverageMultiplier = Defaults.ServiceCoverageMultiplier;
            PlotPrice = Defaults.PlotPrice;
            Uniqueness = Defaults.Uniqueness;
            GroundPollution = Defaults.GroundPollution;
            AirPollution = Defaults.AirPollution;
            NoisePollution = Defaults.NoisePollution;
            EmployeeUpkeep = Defaults.EmployeeUpkeep;
            StorageUpkeep = Defaults.StorageUpkeep;
            ServiceBudgetMultiplier = true;

            _dontSave = false;
            Save();
        }

        public void SetFree()
        {
            _dontSave = true;
            Disable = false;
#if DEBUG
            VerboseLogging = true;
#else
            VerboseLogging = false;
#endif      
            ParkingSpots = 0;
            RoadMaintenance = 0;
            SnowPloughing = 0;
            Towing = 0;
            RoadMaintenanceVehicle = 0;
            SolarPowered = 0;
            GroundWaterPowered = 0;
            WaterPowered = 0;
            WindPowered = 0;
            GarbagePowered = 0;
            ElectricityProduction = 0;
            BatteryOut = 0;
            BatteryCap = 0;
            Transformer = 0;
            WaterPumpCap = 0;
            SewageOutCap = 0;
            Purification = 0;
            Ambulance = 0;
            MedicalHelicopter = 0;
            Patient = 0;
            HealthBonus = 0;
            HealthRange = 0;
            Treatment = 0;
            Hearse = 0;
            BodyStorage = 0;
            BodyProcessing = 0;
            GarbageCap = 0;
            GarbageTruck = 0;
            DumpTruck = 0;
            GarbageProcessing = 0;
            StudentPrimary = 0;
            StudentSecondary = 0;
            StudentTertiary = 0;
            StudentUniversity = 0;
            StudentGraduation = 0;
            StudentWellbeing = 0;
            StudentHealth = 0;
            ResearchFacility = 0;
            FireTruck = 0;
            FireHelicopter = 0;
            FireDisasterCap = 0;
            FireVehicleEffi = 0;
            Firewatch = 0;
            EarlyDisasterWarningSystem = 0;
            DisasterFacility = 0;
            ShelterCap = 0;
            EvacuationBus = 0;
            EmergencyGenerator = 0;
            PatrolCar = 0;
            PoliceHelicopter = 0;
            LocalJail = 0;
            Patrol = 0;
            EmergencyPolice = 0;
            Intelligence = 0;
            PrisonVan = 0;
            PrisonerCap = 0;
            PrisonerWellbeing = 0;
            PrisonerHealth = 0;
            WelfareOffice = 0;
            AdminBuilding = 0;
            Bus = 0;
            Train = 0;
            Taxi = 0;
            Tram = 0;
            Ship = 0;
            //Post = 0;
            //Helicopter = 0;
            Airplane = 0;
            Subway = 0;
            Rocket = 0;
            EnergyFuel = 0;
            EnergyElectricity = 0;
            //ProductionBoost = 0;
            MaintenanceBoost = 0;
            DispatchCenter = 0;
            TradedResource = 0;
            DeliveryTruck = 0;
            ComfortFactor = 0;
            ParkMaintenance = 0;
            ParkMaintenanceVehicle = 0;
            LeisureEfficieny = 0;
            LeisureMeals = 0;
            LeisureEntertainment = 0;
            LeisureCommercial = 0;
            LeisureCityIndoors = 0;
            LeisureTravel = 0;
            LeisureCityPark = 0;
            LeisureCityBeach = 0;
            LeisureAttractions = 0;
            LeisureRelaxation = 0;
            LeisureSightseeing = 0;
            Attraction = 0;
            PostVan = 0;
            PostTruck = 0;
            MailCap = 0;
            PostSortingRate = 0;
            TelecomRange = 0;
            NetworkCap = 0;
            Wireless = 0;
            Fibre = 0;
            ServiceCoverageMultiplier = 0;
            PlotPrice = 0;
            Uniqueness = 0;
            GroundPollution = 0;
            AirPollution = 0;
            NoisePollution = 0;
            EmployeeUpkeep = 0;
            StorageUpkeep = 0;
            ServiceBudgetMultiplier = false;

            _dontSave = false;
            Save();
        }
    }
}
