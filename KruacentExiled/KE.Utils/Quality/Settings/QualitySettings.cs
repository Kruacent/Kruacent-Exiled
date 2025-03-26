using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Exiled.Events.Commands.Hub;
using KE.Utils.Quality.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSettings.ServerSpecific;

namespace KE.Utils.Quality.Settings
{
    internal class QualitySettings
    {

        private static int _idQuality = 0;
        private static int _idModels = 1;
        public QualitySettings()
        {
            HeaderSetting header = new("Quality Settings");
            SettingBase[] settings = new SettingBase[]
            {
                header,
                new TwoButtonsSetting(_idQuality,"ModelQuality","Low","High"),
                new TwoButtonsSetting(_idModels,"Pickup models","off","on")
            };
            SettingBase.Register(settings);
            SettingBase.SendToAll();

            

        }

        public static ModelQuality Get(Player p)
        {
            if (!SettingBase.TryGetSetting<TwoButtonsSetting>(p, _idQuality, out var setting)) return ModelQuality.None;
            if (setting.IsSecond)
                return ModelQuality.High;
            else
                return ModelQuality.Low;
        }


        public static bool PickmodelActivated(Player p)
        {
            if (!SettingBase.TryGetSetting<TwoButtonsSetting>(p, _idModels, out var setting)) return false;
            return setting.IsSecond;
        }


    }
}
