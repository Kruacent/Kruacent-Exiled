using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSettings.ServerSpecific;

namespace KE.CustomRoles.Settings
{
    internal class SettingHandler : IUsingEvents
    {
        private int id;
        private int _idTimeCustomRole = 144;
        private int _idDesc = 143;
        private IReadOnlyCollection<SettingBase> settings;
        public SettingHandler()
        {

            settings = new List<SettingBase>()
            {
                new HeaderSetting ("Custom Roles Hint Settings"),
                new TwoButtonsSetting(_idDesc,"Descriptions","Disabled","Enabled",true,"hide/show the description the Custom Role "),
                new SliderSetting(_idTimeCustomRole,"Time shown",0,30,20),
            };
        }

        public void SubscribeEvents()
        {
            SettingBase.Register(settings);
            //ServerSpecificSettingsSync.ServerOnSettingValueReceived += OnSettingValueReceived;
            Exiled.Events.Handlers.Player.Verified += OnVerified;

        }

        public void UnsubscribeEvents()
        {
            //ServerSpecificSettingsSync.ServerOnSettingValueReceived -= OnSettingValueReceived;
            Exiled.Events.Handlers.Player.Verified -= OnVerified;
            SettingBase.Unregister(predicate:null, settings);
        }



        private void OnVerified(VerifiedEventArgs ev)
        {
            ServerSpecificSettingsSync.SendToPlayer(ev.Player.ReferenceHub);
        }

        private void OnSettingValueReceived(ReferenceHub hub, ServerSpecificSettingBase settingBase)
        {
            


            if (settingBase is SSKeybindSetting keyindSetting && keyindSetting.SettingId == id && keyindSetting.SyncIsPressed)
            {
                Log.Debug("pressed");
            }
        }


        internal float GetTime(Player p)
        {
            if (!SettingBase.TryGetSetting<SliderSetting>(p, _idTimeCustomRole, out var setting)) return 20;
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

    }
}
