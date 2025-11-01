using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.API.Features
{
    public abstract class CustomSCP : KECustomRole
    {
        public const int MinValue = -5;
        public const int MaxValue = 5;
        public const int DefaultValue = 0;

        public static int ScpPreferenceHeaderId => MainPlugin.Instance.Config.ScpPreferenceHeaderId;
        private static HeaderSetting header = null;
        private SliderSetting sliderSetting;
        public abstract bool IsSupport { get; }
        public int SettingId => (int)Id;

        public static IEnumerable<CustomSCP> All => Registered.Where(c => c is CustomSCP).Cast<CustomSCP>();
        public override void Init()
        {
            if (header is null)
            {
                header = new HeaderSetting(ScpPreferenceHeaderId, "SCP Spawn Preferences");
            }


            sliderSetting = new SliderSetting(SettingId, PublicName, MinValue, MaxValue, DefaultValue, true, header: header);
            SettingBase.Register([sliderSetting]);

            
            base.Init();
        }


        public override void Destroy()
        {
            SettingBase.Unregister();
            base.Destroy();
        }


        public int GetPreferences(Player player)
        {
            if (!SettingBase.TryGetSetting<SliderSetting>(player, sliderSetting.Id, out var setting)) return -6;
            return (int) setting.SliderValue;
        }

    }
}
