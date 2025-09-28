using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using KE.Utils.API.Interfaces;
using KE.Utils.Quality.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using UserSettings.ServerSpecific;

namespace KE.Items.API.Core.Settings
{
    internal class SettingsHandler : IUsingEvents
    {

        private SettingBase[] _settings;
        private const int _idHeader = 55;
        private const int _idDesc = 0;
        private const int _idPrefix = 1;
        private const int _idTimeCustomItem = 2;
        private const int _idTimeCustomItemEffect = 3;



        private void CreateSettings()
        {

            _settings =
            [
                new HeaderSetting (_idHeader,"Custom Items Settings"),
                new TwoButtonsSetting(_idDesc,"Descriptions","Disabled","Enabled",true,"hide/show the description of the item and its upgrade chances ", onChanged:OnChanged),
                new TwoButtonsSetting(_idPrefix,"Pickup/Select prefixes","Disabled","Enabled",false,"show/hide the whole sentence for the picking or selecting of the item, if hidden it will show a (P) if pickup or a (I) if in inventory before the name of the item ", onChanged:OnChanged),
                new SliderSetting(_idTimeCustomItem,"Time shown",0,30,10),
                new SliderSetting(_idTimeCustomItemEffect,"Time effect shown",0,30,10),

            ];
            SettingBase.Register(_settings);

        }


        public void SubscribeEvents()
        {
            CreateSettings();
            SettingBase.SendToAll();
        }

        public void UnsubscribeEvents()
        {
            SettingBase.Unregister();
        }


        private void OnChanged(Player p, SettingBase settings)
        {

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
            if (!SettingBase.TryGetSetting<TwoButtonsSetting>(p, _idDesc, out var setting)) return true;
            return setting.IsSecond;
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
