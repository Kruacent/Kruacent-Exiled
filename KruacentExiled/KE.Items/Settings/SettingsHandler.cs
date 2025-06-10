using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using KE.Utils.API.Interfaces;
using KE.Utils.Quality.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace KE.Items.Settings
{
    internal class SettingsHandler : IUsingEvents
    {

        private SettingBase[] _settings;
        private int _idDesc = 1;
        private int _idPrefix = 2;



        private void CreateSettings()
        {
            
            _settings =
            [
                new HeaderSetting ("Custom Items Settings"),
                new TwoButtonsSetting(_idDesc,"Custom Items Descriptions","Disabled","Enabled",true,"hide/show the description of the item and its upgrade chances ", onChanged:OnChanged),
                new TwoButtonsSetting(_idPrefix,"Custom Items Pickup/Select prefixes","Disabled","Enabled",false,"show/hide the whole sentence for the picking or selecting of the item, if hidden it will show a (P) if pickup or a (I) if in inventory before the name of the item ", onChanged:OnChanged),
                
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
