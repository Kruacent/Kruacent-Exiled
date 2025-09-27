using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using KE.Utils.Quality.Enums;
using System;

namespace KE.Utils.Quality.Settings
{
    public class QualitySettings
    {

        private static int _idQuality = 0;
        private static int _idModels = 1;
        private static int _idSlider = 2;
        private SettingBase[] _settings;
        public QualitySettings(Action<Player,SettingBase> onChanged)
        {
            //HeaderSetting header = new("Quality Settings");
            _settings =
            [
                //header,
                new DropdownSetting(_idQuality,"ModelQuality",["Low","Medium", "High"],onChanged:onChanged),
                new TwoButtonsSetting(_idModels,"Pickup models","Disabled","Enabled", onChanged:onChanged)
            ];
        }

        public void Register()
        {
            SettingBase.Register(_settings);
            SettingBase.SendToAll();
        }

        public void Unregister()
        {
            SettingBase.Unregister(settings:_settings);
        }




        public static ModelQuality Get(Player p)
        {
            if (!SettingBase.TryGetSetting<DropdownSetting>(p, _idQuality, out var setting)) return ModelQuality.None;

            return setting.SelectedOption switch
            {
                "Low" => ModelQuality.Low,
                "Medium" => ModelQuality.Medium,
                "High" => ModelQuality.High,
                _ => ModelQuality.None,
            };
        }


        public static bool PickmodelActivated(Player p)
        {
            if (!SettingBase.TryGetSetting<TwoButtonsSetting>(p, _idModels, out var setting)) return false;
            return setting.IsSecond;
        }


    }
}
