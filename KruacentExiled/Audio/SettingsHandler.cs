using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using KE.Utils.API.Features;
using KE.Utils.API.Interfaces;
using KE.Utils.API.Settings.SettingsCategories;
using System;
using System.Collections.Generic;
using System.Security;
using UserSettings.ServerSpecific;

namespace KruacentExiled.Audio
{
    internal class SettingsHandler: IUsingEvents
    {
        private int _idSound = 900;

        public const float DefaultValue = 50;





        private AudioHandler handler;
        public bool Debug => handler.Debug;

        private List<SettingBase> settings;

        private Dictionary<int, SoundType> ids;

        public SettingsHandler(AudioHandler handler)
        {
            this.handler = handler;
            HeaderSetting header = new HeaderSetting(_idSound, "Sound");
            ids = new Dictionary<int, SoundType>();

            settings = new List<SettingBase>();
            int settingid;

            foreach (SoundType type in Enum.GetValues(typeof(SoundType)))
            {
                settingid = GetSettingId(type);
                settings.Add(CreateSliderSetting(settingid, type, header));
                ids.Add(settingid, type);
            }


            new SettingsCategory(header, 1005, settings);
        }

        private int GetSettingId(SoundType type)
        {
            return (int) type + _idSound;
        }

        private bool CheckSetting(int settingId)
        {
            return settingId > _idSound && settingId < settings.Count + _idSound;
        }

        public void SubscribeEvents()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += SafeOnSettingValueReceived;
        }

        public void UnsubscribeEvents()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= SafeOnSettingValueReceived;
        }
        private void SafeOnSettingValueReceived(ReferenceHub hub, ServerSpecificSettingBase settingBase)
        {
            //not catching the exception will desync & kick the player
            try
            {
                OnSettingValueReceived(Player.Get(hub), settingBase);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private void OnSettingValueReceived(Player player, ServerSpecificSettingBase settingBase)
        {


            int settingId = settingBase.SettingId;

            if (CheckSetting(settingId))
            {

                if(!ids.TryGetValue(settingId,out SoundType type))
                {
                    throw new Exception("type not found");
                }

                float volume = GetVolume(player, type);

                KELog.Debug($"change volume {type} to {volume}% ");

                MainPlugin.AudioHandler.ChangeVolume(player, volume, type);
            }


        }

        private SliderSetting CreateSliderSetting(int id, SoundType type, HeaderSetting header)
        {
            return new SliderSetting(id, type.ToString(), 0, 100, DefaultValue, header:header);
        }


        public float GetVolume(Player player,SoundType type)
        {
            int id = GetSettingId(type);

            float volume;

            if (SettingBase.TryGetSetting<SliderSetting>(player, id, out var setting))
            {
                volume = setting.SliderValue;
            }
            else
            {
                volume = DefaultValue;
            }
            volume /= 25;

            return volume;

        }

    }
}
