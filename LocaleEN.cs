using Colossal;
using System.Collections.Generic;

namespace SmartUpkeepManager
{
    public class LocaleEN : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleEN(Setting setting)
        {
            m_Setting = setting;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), Mod.Name },
                { m_Setting.GetOptionTabLocaleID(Setting.Page1), Setting.Page1 },
                { m_Setting.GetOptionTabLocaleID(Setting.Page2), Setting.Page2 },
                { m_Setting.GetOptionTabLocaleID(Setting.Page3), Setting.Page3 },
                { m_Setting.GetOptionGroupLocaleID(Setting.Buttons), Setting.Buttons },
                { m_Setting.GetOptionGroupLocaleID(Setting.General), Setting.General },
                { m_Setting.GetOptionGroupLocaleID(Setting.Roads), Setting.Roads },
                { m_Setting.GetOptionGroupLocaleID(Setting.Electricity), Setting.Electricity },
                { m_Setting.GetOptionGroupLocaleID(Setting.Water), Setting.Water },
                { m_Setting.GetOptionGroupLocaleID(Setting.Health), Setting.Health },
                { m_Setting.GetOptionGroupLocaleID(Setting.Garbage), Setting.Garbage },
                { m_Setting.GetOptionGroupLocaleID(Setting.Education), Setting.Education },
                { m_Setting.GetOptionGroupLocaleID(Setting.Fire), Setting.Fire },
                { m_Setting.GetOptionGroupLocaleID(Setting.Police), Setting.Police },
                { m_Setting.GetOptionGroupLocaleID(Setting.Transportation), Setting.Transportation },
                { m_Setting.GetOptionGroupLocaleID(Setting.Parks), Setting.Parks },
                { m_Setting.GetOptionGroupLocaleID(Setting.Communication), Setting.Communication },

                { m_Setting.GetOptionTabLocaleID(Setting.AboutTab), Setting.AboutTab },
                { m_Setting.GetOptionGroupLocaleID(Setting.InfoGroup), Setting.InfoGroup },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Disable)), "Disable mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Disable)), $"The mod will be disabled and all vanilla upkeep values will be restored." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SaveButton)), "Save Changes" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SaveButton)), $"Save all changes set below." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ModDefault)), "Reset to Mod Defaults" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ModDefault)), $"Reset all changes set below to the mod's predefined values. This doesn't revert back to vanilla costs." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.MakeFree)), "Set all values to 0" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.MakeFree)), $"Set all upkeep amount to 0." },
                
                // Roads
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkingSpots)), "Parking Spots" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkingSpots)), $"Specify the upkeep cost for each parking spots.\r\nMod default: {Defaults.ParkingSpots}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenance)), "Road Maintenance" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenance)), $"Specify the upkeep cost for being able to provide road maintenance service.\r\nMod default: {Defaults.RoadMaintenance}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SnowPloughing)), "Snow Ploughing" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SnowPloughing)), $"Specify the upkeep cost for being able to provide snow ploughing service.\r\nMod default: {Defaults.SnowPloughing}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Towing)), "Towing" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Towing)), $"Specify the upkeep cost for being able to provide towing service.\r\nMod default: {Defaults.Towing}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadMaintenanceVehicle)), "Road Maintenance Vehicles" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadMaintenanceVehicle)), $"Specify the upkeep cost for each Road Maintenance vehicles.\r\nMod default: {Defaults.RoadMaintenanceVehicle}" },

                // Electricity
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SolarPowered)), "Solar Powered Electricity Production" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SolarPowered)), $"Specify the upkeep cost for each MW power producable with solar.\r\nMod default: {Defaults.SolarPowered}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.GroundWaterPowered)), "Ground Water Powered Electricity Production" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.GroundWaterPowered)), $"Specify the upkeep cost for each MW power producable with ground water.\r\nMod default: {Defaults.GroundWaterPowered}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WaterPowered)), "Water Powered Electricity Production" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WaterPowered)), $"Specify the upkeep cost for producable power with water.\r\nMod default: {Defaults.WaterPowered}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WindPowered)), "Wind Powered Electricity Production" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WindPowered)), $"Specify the upkeep cost for each MW power producable with wind.\r\nMod default: {Defaults.WindPowered}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.GarbagePowered)), "Garbage Powered Electricity Production" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.GarbagePowered)), $"Specify the upkeep cost for each MW power producable with garbage.\r\nMod default: {Defaults.GarbagePowered}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ElectricityProduction)), "General Electricity Production" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ElectricityProduction)), $"Specify the upkeep cost for each MW general power producable.\r\nMod default: {Defaults.ElectricityProduction}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BatteryOut)), "Battery Output" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BatteryOut)), $"Specify the upkeep cost for each MW power output.\r\nMod default: {Defaults.BatteryOut}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BatteryCap)), "Battery Capacity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BatteryCap)), $"Specify the upkeep cost for each MW power storage.\r\nMod default: {Defaults.BatteryCap}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Transformer)), "Transformer Upkeep" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Transformer)), $"Specify the upkeep cost for each transformer.\r\nMod default: {Defaults.Transformer}" },

                //Water & Sewage
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WaterPumpCap)), "Water Pumping Capacity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WaterPumpCap)), $"Specify the upkeep cost for each 1000 m³ of water pumped.\r\nMod default: {Defaults.WaterPumpCap}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.SewageOutCap)), "Sewage Pumping Capacity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.SewageOutCap)), $"Specify the upkeep cost for each 1000 m³ of sewage pumped.\r\nMod default: {Defaults.SewageOutCap}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Purification)), "Purification" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Purification)), $"Specify the upkeep cost for purification.\r\nMod default: {Defaults.Purification}" },

                //Healthcare & Deathcare
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Ambulance)), "Ambulances" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Ambulance)), $"Specify the upkeep cost for each Ambulances.\r\nMod default: {Defaults.Ambulance}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.MedicalHelicopter)), "Medical Helicopters" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.MedicalHelicopter)), $"Specify the upkeep cost for each Medical Helicopters.\r\nMod default: {Defaults.MedicalHelicopter}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Patient)), "Patients" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Patient)), $"Specify the upkeep cost for each Patients.\r\nMod default: {Defaults.Patient}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HealthBonus)), "Treatment Bonus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HealthBonus)), $"Specify the upkeep cost for each Treatment Bonus.\r\nMod default: {Defaults.HealthBonus}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HealthRange)), "Health Range" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HealthRange)), $"Specify the upkeep cost for maintaining Health Range.\r\nMod default: {Defaults.HealthRange}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Treatment)), "Treatments" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Treatment)), $"Specify the upkeep cost for each type of Treatments.\r\nMod default: {Defaults.Treatment}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Hearse)), "Hearses" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Hearse)), $"Specify the upkeep cost for each Hearses.\r\nMod default: {Defaults.Hearse}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BodyStorage)), "Body Storage" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BodyStorage)), $"Specify the upkeep cost for each bodies stored. 30% for short term storage.\r\nMod default: {Defaults.BodyStorage}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BodyProcessing)), "Body Processing" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BodyProcessing)), $"Specify the upkeep cost for processing each bodies.\r\nMod default: {Defaults.BodyProcessing}" },

                // Garbage Management
                
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.GarbageTruck)), "Garbage Trucks" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.GarbageTruck)), $"Specify the upkeep cost for each Garbage Trucks.\r\nMod default: {Defaults.GarbageTruck}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DumpTruck)), "Dump Trucks" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DumpTruck)), $"Specify the upkeep cost for each Dump Trucks.\r\nMod default: {Defaults.DumpTruck}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.GarbageCap)), "Garbage Storage" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.GarbageCap)), $"Specify the upkeep cost for tonnes of garbage stored. 30% for short term storage + 90% less for industrial wastes only.\r\nMod default: {Defaults.GarbageCap}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.GarbageProcessing)), "Garbage Processing" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.GarbageProcessing)), $"Specify the upkeep cost for processing each tonnes of garbage stored.\r\nMod default: {Defaults.GarbageProcessing}" },

                // Education & Research

                //{ m_Setting.GetOptionLabelLocaleID(nameof(Setting.StudentPrimary)), "Elementary Seats" },
                //{ m_Setting.GetOptionDescLocaleID(nameof(Setting.StudentPrimary)), $"Specify the upkeep cost for having the ability to educated each elementary students. This will be incurring whether someone is admitted to a seat or not.\r\nMod default: {Defaults.StudentPrimary}" },

                //{ m_Setting.GetOptionLabelLocaleID(nameof(Setting.StudentSecondary)), "High School Seats" },
                //{ m_Setting.GetOptionDescLocaleID(nameof(Setting.StudentSecondary)), $"Specify the upkeep cost for having the ability to educated each high school students. This will be incurring whether someone is admitted to a seat or not.\r\nMod default: {Defaults.StudentSecondary}" },

                //{ m_Setting.GetOptionLabelLocaleID(nameof(Setting.StudentTertiary)), "College Seats" },
                //{ m_Setting.GetOptionDescLocaleID(nameof(Setting.StudentTertiary)), $"Specify the upkeep cost for having the ability to educated each college students. This will be incurring whether someone is admitted to a seat or not.\r\nMod default: {Defaults.StudentTertiary}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Student)), "Student Seats" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Student)), $"Specify the upkeep cost for having the ability to educate each students. This will be incurring whether someone is admitted to a seat or not.\r\nMod default: {Defaults.Student}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StudentGraduation)), "Graduation Bonus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StudentGraduation)), $"Specify the upkeep cost for each % of graduation bonus.\r\nMod default: {Defaults.StudentGraduation}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StudentWellbeing)), "Student Wellbeing" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StudentWellbeing)), $"Specify the upkeep cost for each wellbeing point each student receives.\r\nMod default: {Defaults.StudentWellbeing}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StudentHealth)), "Student Health" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StudentHealth)), $"Specify the upkeep cost for each health point each student receives.\r\nMod default: {Defaults.StudentHealth}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResearchFacility)), "Research Facility" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResearchFacility)), $"Specify the upkeep cost for each Research Facilities.\r\nMod default: {Defaults.ResearchFacility}" },

                // Fire & Rescue
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FireTruck)), "Fire Trucks" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FireTruck)), $"Specify the upkeep cost for each Fire Trucks.\r\nMod default: {Defaults.FireTruck}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FireHelicopter)), "Fire Helicopters" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FireHelicopter)), $"Specify the upkeep cost for each Fire Helicopters.\r\nMod default: {Defaults.FireHelicopter}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FireDisasterCap)), "Disaster Response Teams" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FireDisasterCap)), $"Specify the upkeep cost for each Disaster Response teams.\r\nMod default: {Defaults.FireDisasterCap}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FireVehicleEffi)), "Firefighter Training" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.FireVehicleEffi)), $"Specify the upkeep cost for each Firefighter Training levels.\r\nMod default: {Defaults.FireVehicleEffi}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Firewatch)), "Firewatch" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Firewatch)), $"Specify the upkeep cost for each Firewatch Towers.\r\nMod default: {Defaults.Firewatch}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EarlyDisasterWarningSystem)), "Early Disaster Warning System" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EarlyDisasterWarningSystem)), $"Specify the upkeep cost for each Early Disaster Warning Systems.\r\nMod default: {Defaults.EarlyDisasterWarningSystem}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DisasterFacility)), "Disaster Facility" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DisasterFacility)), $"Specify the upkeep cost for each Disaster Facilities.\r\nMod default: {Defaults.DisasterFacility}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EvacuationBus)), "Evacuation Buses" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EvacuationBus)), $"Specify the upkeep cost for each Evacuation Bus.\r\nMod default: {Defaults.EvacuationBus}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShelterCap)), "Emergency Shelter Capacity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ShelterCap)), $"Specify the upkeep cost for having the shelter for each 1000 citizens. This will be incurring whether someone is talking shelter or not.\r\nMod default: {Defaults.ShelterCap}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EmergencyGenerator)), "Emergency Generator" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EmergencyGenerator)), $"Specify the upkeep cost for each MW power producable on emergency. This will be incurring whether there's a power cut or not\r\nMod default: {Defaults.EmergencyGenerator}" },

                // Police & Administration
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.PatrolCar)), "Patrol Cars" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.PatrolCar)), $"Specify the upkeep cost for each Patrol Cars.\r\nMod default: {Defaults.PatrolCar}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.PoliceHelicopter)), "Fire Helicopters" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.PoliceHelicopter)), $"Specify the upkeep cost for each Police Helicopters.\r\nMod default: {Defaults.PoliceHelicopter}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.LocalJail)), "Local Jail Capacity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.LocalJail)), $"Specify the upkeep cost for each local jail cells.\r\nMod default: {Defaults.LocalJail}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Patrol)), "Road Maintenance" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Patrol)), $"Specify the upkeep cost for being able to provide patrolling service.\r\nMod default: {Defaults.Patrol}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EmergencyPolice)), "Snow Ploughing" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EmergencyPolice)), $"Specify the upkeep cost for being able to provide emergency service.\r\nMod default: {Defaults.EmergencyPolice}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Intelligence)), "Towing" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Intelligence)), $"Specify the upkeep cost for being able to provide intelligence service.\r\nMod default: {Defaults.Intelligence}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.PrisonVan)), "Prison Vans" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.PrisonVan)), $"Specify the upkeep cost for each Prison Vans.\r\nMod default: {Defaults.PrisonVan}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.PrisonerCap)), "Prisoner Capacity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.PrisonerCap)), $"Specify the upkeep cost for each prison jail cells teams.\r\nMod default: {Defaults.PrisonerCap}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.PrisonerWellbeing)), "Prisoner Wellbeing" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.PrisonerWellbeing)), $"Specify the upkeep cost for each wellbeing point each prisoner receives.\r\nMod default: {Defaults.PrisonerWellbeing}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.PrisonerHealth)), "Prisoner Health" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.PrisonerHealth)), $"Specify the upkeep cost for each health point each prisoner receives.\r\nMod default: {Defaults.PrisonerHealth}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.WelfareOffice)), "Adminstration Building" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.WelfareOffice)), $"Specify the upkeep cost for each Welfare Offices.\r\nMod default: {Defaults.WelfareOffice}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AdminBuilding)), "Adminstration Building" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AdminBuilding)), $"Specify the upkeep cost for each Adminstration Buildings.\r\nMod default: {Defaults.AdminBuilding}" },

                // Transportation
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Bus)), "Bus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Bus)), $"Specify the upkeep cost for each Buses.\r\nMod default: {Defaults.Bus}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Train)), "Train" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Train)), $"Specify the upkeep cost for each Trains.\r\nMod default: {Defaults.Train}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Taxi)), "Taxi" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Taxi)), $"Specify the upkeep cost for each Taxis.\r\nMod default: {Defaults.Taxi}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Tram)), "Tram" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Tram)), $"Specify the upkeep cost for each Trams.\r\nMod default: {Defaults.Tram}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Ship)), "Ship" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Ship)), $"Specify the upkeep cost for each Ships.\r\nMod default: {Defaults.Ship}" },

                //{ m_Setting.GetOptionLabelLocaleID(nameof(Setting.Post)), "Post" },
                //{ m_Setting.GetOptionDescLocaleID(nameof(Setting.Post)), $"Specify the upkeep cost for each Posts.\r\nMod default: {Defaults.Post}" },

                //{ m_Setting.GetOptionLabelLocaleID(nameof(Setting.Helicopter)), "Helicopter" },
                //{ m_Setting.GetOptionDescLocaleID(nameof(Setting.Helicopter)), $"Specify the upkeep cost for each Helicopters.\r\nMod default: {Defaults.Helicopter}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Airplane)), "Airplane" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Airplane)), $"Specify the upkeep cost for each Airplanes.\r\nMod default: {Defaults.Airplane}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Subway)), "Subway" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Subway)), $"Specify the upkeep cost for each Subways.\r\nMod default: {Defaults.Subway}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Rocket)), "Rocket" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Rocket)), $"Specify the upkeep cost for each Rockets.\r\nMod default: {Defaults.Rocket}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnergyFuel)), "Non electric Refuelling" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnergyFuel)), $"Specify the upkeep cost for having fuel run vehicles.\r\nMod default: {Defaults.EnergyFuel}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnergyElectricity)), "Electric Charging" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnergyElectricity)), $"Specify the upkeep cost for having electricity run vehicles.\r\nMod default: {Defaults.EnergyElectricity}" },

                //{ m_Setting.GetOptionLabelLocaleID(nameof(Setting.ProductionBoost)), "ProductionBoost" },
                //{ m_Setting.GetOptionDescLocaleID(nameof(Setting.ProductionBoost)), $"Specify the upkeep cost for each Production Boost point each vehicles receive.\r\nMod default: {Defaults.ProductionBoost}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.MaintenanceBoost)), "Maintenance Boost" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.MaintenanceBoost)), $"Specify the upkeep cost for each Maintenance Boost point each vehicles receive.\r\nMod default: {Defaults.MaintenanceBoost}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DispatchCenter)), "Dispatch Center" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DispatchCenter)), $"Specify the upkeep cost for each Dispatch Centers.\r\nMod default: {Defaults.DispatchCenter}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TradedResource)), "Trade License" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TradedResource)), $"Specify the upkeep cost for each tradable resources.\r\nMod default: {Defaults.TradedResource}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DeliveryTruck)), "Bus" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DeliveryTruck)), $"Specify the upkeep cost for each Delivery Trucks.\r\nMod default: {Defaults.DeliveryTruck}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ComfortFactor)), "Comfort Factor" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ComfortFactor)), $"Specify the upkeep cost for each Comfort point each passengers receive.\r\nMod default: {Defaults.ComfortFactor}" },

                // Parks & Recreation
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenance)), "Park Maintenance" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenance)), $"Specify the upkeep cost for being able to provide park maintenance service.\r\nMod default: {Defaults.ParkMaintenance}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ParkMaintenanceVehicle)), "Park Maintenance Vehicles" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ParkMaintenanceVehicle)), $"Specify the upkeep cost for each Park Maintenance vehicles.\r\nMod default: {Defaults.ParkMaintenanceVehicle}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.LeisureEfficieny)), "Leisure Efficieny" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.LeisureEfficieny)), $"Specify the upkeep cost for each leisure efficiency points.\r\nMod default: {Defaults.LeisureEfficieny}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.LeisureMeals)), "Meal Service" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.LeisureMeals)), $"Specify the upkeep cost for Meal leisure services.\r\nMod default: {Defaults.LeisureMeals}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.LeisureEntertainment)), "Entertainment Service" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.LeisureEntertainment)), $"Specify the upkeep cost for Entertainment leisure services.\r\nMod default: {Defaults.LeisureEntertainment}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.LeisureCommercial)), "Commercial Service" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.LeisureCommercial)), $"Specify the upkeep cost for Commercial leisure services.\r\nMod default: {Defaults.LeisureCommercial}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.LeisureCityIndoors)), "Indoors Service" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.LeisureCityIndoors)), $"Specify the upkeep cost for CityIndoors leisure services.\r\nMod default: {Defaults.LeisureCityIndoors}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.LeisureTravel)), "Travel Service" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.LeisureTravel)), $"Specify the upkeep cost for Travel leisure services.\r\nMod default: {Defaults.LeisureTravel}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.LeisureCityPark)), "City Park Service" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.LeisureCityPark)), $"Specify the upkeep cost for CityPark leisure services.\r\nMod default: {Defaults.LeisureCityPark}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.LeisureCityBeach)), "City Beach Service" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.LeisureCityBeach)), $"Specify the upkeep cost for CityBeach leisure services.\r\nMod default: {Defaults.LeisureCityBeach}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.LeisureAttractions)), "Attractions Service" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.LeisureAttractions)), $"Specify the upkeep cost for Attractions leisure services.\r\nMod default: {Defaults.LeisureAttractions}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.LeisureRelaxation)), "Relaxation Service" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.LeisureRelaxation)), $"Specify the upkeep cost for Relaxation leisure services.\r\nMod default: {Defaults.LeisureRelaxation}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.LeisureSightseeing)), "Sightseeing Service" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.LeisureSightseeing)), $"Specify the upkeep cost for Sightseeing leisure services.\r\nMod default: {Defaults.LeisureSightseeing}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Attraction)), "Attraction" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Attraction)), $"Specify the upkeep cost for each Attractiveness points.\r\nMod default: {Defaults.Attraction}" },

                // Communication
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.PostVan)), "Post Vans" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.PostVan)), $"Specify the upkeep cost for each Post Vans.\r\nMod default: {Defaults.PostVan}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.PostTruck)), "Post Trucks" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.PostTruck)), $"Specify the upkeep cost for each Post Trucks.\r\nMod default: {Defaults.PostTruck}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.MailCap)), "Mail Storage Capacity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.MailCap)), $"Specify the upkeep cost for each tonnes of Mail Storage.\r\nMod default: {Defaults.MailCap}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.PostSortingRate)), "Mail Sorting" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.PostSortingRate)), $"Specify the upkeep cost for each tonnes of mail sorted.\r\nMod default: {Defaults.PostSortingRate}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TelecomRange)), "Internet Range" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TelecomRange)), $"Specify the upkeep cost for each km of service provided.\r\nMod default: {Defaults.TelecomRange}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.NetworkCap)), "Network Capacity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.NetworkCap)), $"Specify the upkeep cost for each GB of network capacity.\r\nMod default: {Defaults.NetworkCap}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Wireless)), "Wireless Internet" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Wireless)), $"Specify the upkeep cost for providing wireless internet connection.\r\nMod default: {Defaults.Wireless}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Fibre)), "Fiber Internet" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Fibre)), $"Specify the upkeep cost for providing fiber internet connection.\r\nMod default: {Defaults.Fibre}" },


                // Landscaping

                // General
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ServiceBudgetMultiplier)), "Service Budget Multiplier" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ServiceBudgetMultiplier)), $"Specify whether the upkeep cost changes with the budget value of the services.\r\nMod default: Active" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CityBonusMultiplier)), "City Bonus Multiplier" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CityBonusMultiplier)), $"Specify whether the upkeep cost will be affected by Bonus effects provided by the buildings.\r\nMod default: Active" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ServiceCoverageMultiplier)), "Service Coverage Multiplier" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ServiceCoverageMultiplier)), $"Specify the upkeep cost for providing services.\r\nMod default: {Defaults.ServiceCoverageMultiplier}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.PlotPrice)), "Plot Maintenance" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.PlotPrice)), $"Specify the upkeep cost for maintaining the each square lot.\r\nMod default: {Defaults.PlotPrice}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Uniqueness)), "Uniqueness" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Uniqueness)), $"Specify the upkeep multiplier for unique assets.\r\nMod default: {Defaults.Uniqueness}%" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.GroundPollution)), "Ground Pollution Penalty" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.GroundPollution)), $"Specify the upkeep cost for polluting the ground.\r\nMod default: {Defaults.GroundPollution} for 1000 pollution" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.NoisePollution)), "Noise Pollution Penalty" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.NoisePollution)), $"Specify the upkeep cost for polluting the sound.\r\nMod default: {Defaults.NoisePollution} for 1000 pollution" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirPollution)), "Air Pollution Penalty" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AirPollution)), $"Specify the upkeep cost for polluting the air.\r\nMod default: {Defaults.AirPollution} for 1000 pollution" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EmployeeUpkeep)), "Employee Upkeep" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EmployeeUpkeep)), $"Specify the upkeep cost for having the workplace for each worker. This is not the same as wage. This will be incurring whether someone is employed to a position or not.\r\nMod default: {Defaults.EmployeeUpkeep}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.StorageUpkeep)), "Storage Upkeep" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.StorageUpkeep)), $"Specify the upkeep cost for having the storage for each tonne of resource. This is not the same as resource cost. This will be incurring whether something is stored or not.\r\nMod default: {Defaults.StorageUpkeep}" },


                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.VerboseLogging)), "Verbose Logging" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.VerboseLogging)), $"Enable detailed logging for troubleshooting." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.NameText)), "Mod Name" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.NameText)), "" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.VersionText)), "Mod Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.VersionText)), "" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AuthorText)), "Author" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.AuthorText)), "" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BMaCLink)), "Buy Me a Coffee" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BMaCLink)), "Support the author." },
            };
        }

        public void Unload()
        {

        }
    }
}
