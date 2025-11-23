using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using KE.CustomRoles.Settings;
using KE.Utils.API.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using UserSettings.ServerSpecific;
using static UnityEngine.Rendering.RayTracingAccelerationStructure;

namespace KE.CustomRoles.API.Features
{
    public abstract class CustomSCP : KECustomRole
    {
        public const int MinValue = -5;
        public const int MaxValue = 5;
        public const int DefaultValue = 0;

        private SliderSetting sliderSetting;
        public abstract bool IsSupport { get; }

        private new RecyclableSettingId Id;
        public int SettingId => Id.Value;
        private static SettingsPage page;
        private static List<ServerSpecificSettingBase> settings;
        public static IEnumerable<CustomSCP> All => Registered.Where(c => c is CustomSCP).Cast<CustomSCP>();
        public override void Init()
        {

            
            if (page is null)
            {
                settings = new();
                page = new SettingsPage("Custom SCPs Preferences", settings);
            }
            Id = new RecyclableSettingId();

            settings.Add(new SSSliderSetting(SettingId, PublicName, MinValue, MaxValue, DefaultValue, true));

            Utils.API.Settings.SettingHandler.Instance.AddPages(page);
            base.Init();
        }


        public override void Destroy()
        {
            Id.Destroy();
            base.Destroy();
        }


        public int GetPreferences(Player player)
        {
            if (!SettingBase.TryGetSetting<SliderSetting>(player, sliderSetting.Id, out var setting)) return -6;
            return (int) setting.SliderValue;
        }

    }
}
