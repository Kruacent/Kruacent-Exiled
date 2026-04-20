using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using KE.Utils.API.Settings.SettingsCategories;
using System;
using System.Collections.Generic;


namespace KruacentExiled.CustomItems.API.Core.Settings
{
    internal class SettingsHandler
    {

        private const int _idHeader = 55;
        private const int _idDesc = 0;
        private const int _idPrefix = 1;
        private const int _idTimeCustomItem = 2;
        private const int _idTimeCustomItemEffect = 3;

        //public readonly SettingsPage page;

        public const string SettingDescription = "Description";
        public const string SettingDescription1 = "Disabled";
        public const string SettingDescription2 = "Enabled";
        public const string SettingDescriptionDescription = "hide/show the description of the item and its upgrade chances";


        public SettingsHandler()
        {
            HeaderSetting header = new HeaderSetting(_idHeader, "Custom Items", padding: true);

            List<SettingBase> settings = new List<SettingBase>()
            {

                new TwoButtonsSetting(_idDesc,SettingDescription,SettingDescription1,SettingDescription2,true,SettingDescriptionDescription),
                new TwoButtonsSetting(_idPrefix,"Pickup/Select prefixes","Disabled","Enabled",false,"show/hide the whole sentence for the picking or selecting of the item, if hidden it will show a (P) if pickup or a (I) if in inventory before the name of the item "),
                new SliderSetting(_idTimeCustomItem,"Time shown",0,30,10),
                new SliderSetting(_idTimeCustomItemEffect,"Time effect shown",0,30,10),
            };

            SettingsCategory category = new SettingsCategory(header,999, settings);

            //page = new("Custom Items", settings);

        }



        internal float GetTime(Player p)
        {
            if (!SettingBase.TryGetSetting<SliderSetting>(p, _idTimeCustomItem, out var setting)) return 10;
            return setting.SliderValue;
        }

        internal float GetTimeEffect(Player p)
        {
            if (!SettingBase.TryGetSetting<SliderSetting>(p, _idTimeCustomItemEffect, out var setting)) return 10;
            return setting.SliderValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns> true if the player wants description ; false otherwise</returns>
        internal bool GetDescriptionsSettings(Player p)
        {
            try
            {
                if (!SettingBase.TryGetSetting<TwoButtonsSetting>(p, _idDesc, out var setting)) return true;
                return setting.IsSecond;
            }
            catch(InvalidCastException e)
            {
                Log.Error(e);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns>true if the player wants prefixes ; false otherwise</returns>
        internal bool GetPrefixes(Player p)
        {
            if (!SettingBase.TryGetSetting<TwoButtonsSetting>(p, _idPrefix, out var setting)) return false;
            return setting.IsSecond;
        }
    }
}
