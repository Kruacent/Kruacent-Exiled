using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using KE.CustomRoles.API.Features;
using KE.Utils.API.Settings.SettingsCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSettings.ServerSpecific;

namespace KE.CustomRoles.Settings.DebugSettings
{
    public abstract class DebugSetting
    {

        internal static List<DebugSetting> settings = new();

        public HeaderSetting Header;

        private SettingsCategory category = null;

        public DebugSetting()
        {
            settings.Add(this);
        }

        public IReadOnlyCollection<SettingBase> Settings { get; private set; }


        public void Create()
        {
            Settings = CreateSettings();
            Header = Settings.First(s => s is HeaderSetting) as HeaderSetting;
        }

        protected abstract List<SettingBase> CreateSettings();

        public virtual void OnSettingValueReceived(Player player, ServerSpecificSettingBase settingBase)
        {

        }


        public SettingsCategory GetCategory()
        {
            if(category == null)
            {
                category = new SettingsCategory(Header, 0, Settings.Where(s => s is not HeaderSetting).ToList());
            }
            return category;
        }


    }
}
