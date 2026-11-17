using Game.UI.Localization;
using StarQ.Shared.Extensions;

namespace SmartUpkeepManager
{
    public partial class Mod
    {
        public static void AddLocale()
        {
            string pageText = LocaleHelper
                .Translate("Editor.PAGE_FORMAT")
                .Replace(" {CURRENT}/{TOTAL}", "")
                .Replace("{CURRENT}/{TOTAL} ", "")
                .Replace("{CURRENT}/{TOTAL}", "")
                .Replace("{CURRENT}", "")
                .Replace("{TOTAL}", "")
                .Replace("/", "")
                .Replace("第頁", "頁")
                .Replace("共頁", "")
                .Replace("页码", "页");

            LocaleHelper.AddLocalization(
                LocaleHelper.GetOptionsTabLocaleId(Setting.Page1),
                $"{pageText} {new LocalizedNumber<int>(1).value}"
            );
            LocaleHelper.AddLocalization(
                LocaleHelper.GetOptionsTabLocaleId(Setting.Page2),
                $"{pageText} {new LocalizedNumber<int>(2).value}"
            );
            LocaleHelper.AddLocalization(
                LocaleHelper.GetOptionsTabLocaleId(Setting.Page3),
                $"{pageText} {new LocalizedNumber<int>(3).value}"
            );

            LocaleHelper.AddLocalization(
                LocaleHelper.GetOptionsGroupLocaleId(Setting.Generals),
                LocaleHelper.Translate("Options.SECTION[General]")
            );

            LocaleHelper.AddLocalization(
                LocaleHelper.GetOptionsGroupLocaleId(Setting.Roads),
                LocaleHelper.GetServiceName("Roads")
            );
            LocaleHelper.AddLocalization(
                LocaleHelper.GetOptionsGroupLocaleId(Setting.Electricity),
                LocaleHelper.GetServiceName("Electricity")
            );
            LocaleHelper.AddLocalization(
                LocaleHelper.GetOptionsGroupLocaleId(Setting.Water),
                LocaleHelper.GetServiceName("Water & Sewage")
            );
            LocaleHelper.AddLocalization(
                LocaleHelper.GetOptionsGroupLocaleId(Setting.Health),
                LocaleHelper.GetServiceName("Health & Deathcare")
            );
            LocaleHelper.AddLocalization(
                LocaleHelper.GetOptionsGroupLocaleId(Setting.Garbage),
                LocaleHelper.GetServiceName("Garbage Management")
            );
            LocaleHelper.AddLocalization(
                LocaleHelper.GetOptionsGroupLocaleId(Setting.Education),
                LocaleHelper.GetServiceName("Education & Research")
            );
            LocaleHelper.AddLocalization(
                LocaleHelper.GetOptionsGroupLocaleId(Setting.Fire),
                LocaleHelper.GetServiceName("Fire & Rescue")
            );
            LocaleHelper.AddLocalization(
                LocaleHelper.GetOptionsGroupLocaleId(Setting.Police),
                LocaleHelper.GetServiceName("Police & Administration")
            );
            LocaleHelper.AddLocalization(
                LocaleHelper.GetOptionsGroupLocaleId(Setting.Transportation),
                LocaleHelper.GetServiceName("Transportation")
            );
            LocaleHelper.AddLocalization(
                LocaleHelper.GetOptionsGroupLocaleId(Setting.Parks),
                LocaleHelper.GetServiceName("Parks & Recreation")
            );
            LocaleHelper.AddLocalization(
                LocaleHelper.GetOptionsGroupLocaleId(Setting.Communication),
                LocaleHelper.GetServiceName("Communications")
            );

            LabelDescCommon(nameof(m_Setting.RoadMaintenance), Defaults.RoadMaintenance);
            LabelDesc(
                nameof(m_Setting.ParkingSpots),
                "SubServices.NAME[RoadsParking]",
                Defaults.ParkingSpots
            );
            LabelDesc(
                nameof(m_Setting.RoadMaintenanceVehicle),
                "Infoviews.INFOMODE[Road Maintenance Vehicles]",
                Defaults.RoadMaintenanceVehicle
            );

            //LocaleHelper.AddLocalization(
            //    LocaleHelper.GetOptionsGroupLocaleId("ElectricityGroup"),
            //    LocaleHelper.Translate("Electricity")
            //);
            //LocaleHelper.AddLocalization(
            //    LocaleHelper.GetOptionsTabLocaleId("CargoTab"),
            //    LocaleHelper.Translate("Transport.TAB[Cargo]")
            //);
            //LocaleHelper.AddLocalization(
            //    LocaleHelper.GetOptionsGroupLocaleId("RoadGroup"),
            //    LocaleHelper.Translate("StatisticsPanel.STAT_TITLE[CargoCountTruck]")
            //);
            //LocaleHelper.AddLocalization(
            //    LocaleHelper.GetOptionsGroupLocaleId("TrainGroup"),
            //    LocaleHelper.Translate("StatisticsPanel.STAT_TITLE[CargoCountTrain]")
            //);
            //LocaleHelper.AddLocalization(
            //    LocaleHelper.GetOptionsGroupLocaleId("ShipGroup"),
            //    LocaleHelper.Translate("StatisticsPanel.STAT_TITLE[CargoCountShip]")
            //);
            //LocaleHelper.AddLocalization(
            //    LocaleHelper.GetOptionsGroupLocaleId("AirGroup"),
            //    LocaleHelper.Translate("StatisticsPanel.STAT_TITLE[CargoCountAirplane]")
            //);
        }

        private static string Def(string key, int def) =>
            $"{LocaleHelper.Translate($"{Id}.{key}_Desc")}\n"
            + $"<{LocaleHelper.Translate("Options.INTERFACE_STYLE[default]")}>: "
            + $"{LocaleHelper.Translate("Common.VALUE_MONEY_PER_MONTH").Replace("{SIGN}", "").Replace("{VALUE}", $"{def}")}";

        private static void Desc(string key, int def)
        {
            LocaleHelper.AddLocalization(LocaleHelper.GetOptionsDescLocaleId(key), Def(key, def));
        }

        private static void Label(string key, string label, bool labelRaw = false)
        {
            LocaleHelper.AddLocalization(
                LocaleHelper.GetOptionsLabelLocaleId(key),
                labelRaw ? label : LocaleHelper.Translate(label)
            );
        }

        private static void LabelDesc(string key, string label, int def, bool labelRaw = false)
        {
            Label(key, label, labelRaw);
            Desc(key, def);
        }

        private static void LabelDescCommon(string key, int def)
        {
            Label(key, $"{Id}.{key}");
            Desc(key, def);
        }
    }
}
