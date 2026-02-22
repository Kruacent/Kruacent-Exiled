using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using KE.Utils.API.Interfaces;
using KE.Utils.API.Settings;
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

        //public readonly SettingsPage page;


        public SettingsHandler()
        {
            _settings =
            [
                new HeaderSetting(_idHeader,"Custom Items",padding:true),
                new TwoButtonsSetting(_idDesc,"Descriptions","Disabled","Enabled",true,"hide/show the description of the item and its upgrade chances "),
                new TwoButtonsSetting(_idPrefix,"Pickup/Select prefixes","Disabled","Enabled",false,"show/hide the whole sentence for the picking or selecting of the item, if hidden it will show a (P) if pickup or a (I) if in inventory before the name of the item "),
                new SliderSetting(_idTimeCustomItem,"Time shown",0,30,10),
                new SliderSetting(_idTimeCustomItemEffect,"Time effect shown",0,30,10),

            ];

            List<ServerSpecificSettingBase> settings = new();


            foreach (SettingBase setting in _settings)
            {
                settings.Add(setting.Base);
            }

            //page = new("Custom Items", settings);

            SettingBase.Register(settings: _settings);

        }



        public void SubscribeEvents()
        {
            //Utils.API.Settings.SettingHandler.Instance.AddPages(page);
        }

        public void UnsubscribeEvents()
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
