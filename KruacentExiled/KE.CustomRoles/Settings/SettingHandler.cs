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
        private int id =143;
        private IReadOnlyCollection<SettingBase> settings;
        public SettingHandler()
        {

            settings = new List<SettingBase>()
            {
                new KeybindSetting(id, "testkey", default),
            };
        }

        public void SubscribeEvents()
        {
            SettingBase.Register(settings);
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += OnSettingValueReceived;
            Exiled.Events.Handlers.Player.Verified += OnVerified;

        }

        public void UnsubscribeEvents()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= OnSettingValueReceived;
            Exiled.Events.Handlers.Player.Verified -= OnVerified;
            SettingBase.Unregister((p => p.Id == p.Id), settings);
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

    }
}
