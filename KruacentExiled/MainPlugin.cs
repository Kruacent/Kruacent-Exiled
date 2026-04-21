using Exiled.API.Features;
using Exiled.API.Interfaces;
using System;

namespace KruacentExiled
{
    internal class MainPlugin : Plugin<Config>
    {

        private KEPlugin[] plugins;
        public override string Author => "Patrique & OmerGS";
        public override Version Version { get; } = new Version(2, 0, 1);

        public static MainPlugin Instance { get; private set; }

        public override void OnEnabled()
        {
            Instance = this;

            plugins = new KEPlugin[]
            {
                new CustomRoles.MainPlugin(),
                new GlobalEventFramework.MainPlugin(),
                new GlobalEventFramework.Examples.MainPlugin(),
                new CustomItems.MainPlugin(),
                new Misc.MainPlugin(),
                new Map.MainPlugin(),
            };

            for (int i = 0; i < plugins.Length; i++)
            {
                KEPlugin plugin = plugins[i];
                try
                {
                    plugin.OnEnabled();
                }
                catch(Exception e)
                {
                    Log.Error(e);
                }

            }


            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            for (int i = 0; i < plugins.Length; i++)
            {
                KEPlugin plugin = plugins[i];

                plugin.OnDisabled();
                Log.Info(plugin.Name + " has been disabled!");

            }

            Instance = null;

            base.OnDisabled();
        }

    }




    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;


        public CustomRoles.Config CustomRoleConfig { get; set; } = new CustomRoles.Config();
        public CustomItems.Config CustomItemConfig { get; set; } = new CustomItems.Config();
        public Map.Config MapConfig { get; set; } = new Map.Config();
        public Misc.Config MiscConfig { get; set; } = new Misc.Config();
        public GlobalEventFramework.Config GEFConfig { get; set; } = new GlobalEventFramework.Config();
        public GlobalEventFramework.Examples.Config GEFEConfig { get; set; } = new GlobalEventFramework.Examples.Config();
    }

}
