using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using System.Collections.Generic;
using System.Linq;

namespace KE.CustomRoles.API.Features
{
    public abstract class CustomSCP : KECustomRole
    {
        public const int MinValue = -5;
        public const int MaxValue = 5;
        public const int DefaultValue = 0;

        private SliderSetting sliderSetting;
        public abstract bool IsSupport { get; }

        protected abstract int SettingId { get; }
        private static HeaderSetting header = null;
        private static int HeaderId => MainPlugin.Instance.Config.HeaderId;
        public static IEnumerable<CustomSCP> All => Registered.Where(c => c is CustomSCP).Cast<CustomSCP>();
        public override void Init()
        {

            
            if (header is null)
            {
                header = new(HeaderId, "SCP Spawn Preferences",string.Empty,true);
                SettingBase.Register([header]);
            }

            sliderSetting= new SliderSetting(SettingId, PublicName, MinValue, MaxValue, DefaultValue, true);
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

        public override bool IsAvailable(Player player)
        {
            return false;
        }

    }
}
