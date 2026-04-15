using Exiled.API.Features;
using Exiled.API.Interfaces;
using KE.CustomRoles.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled
{
    internal class MainPlugin : Plugin<Config>
    {

        private KEPlugin[] plugins;
        public override string Author => "Patrique & OmerGS";
        public override Version Version { get; } = new Version(2, 0, 0);

        public static MainPlugin Instance { get; private set; }

        public override void OnEnabled()
        {
            Instance = this;

            plugins = new KEPlugin[]
            {
                new KE.CustomRoles.MainPlugin(),
                new KE.GlobalEventFramework.MainPlugin(),
                new KE.GlobalEventFramework.Examples.MainPlugin(),
                new KE.Items.MainPlugin(),
                new KE.Misc.MainPlugin(),
                new KE.Map.MainPlugin(),
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


        public KE.CustomRoles.Config CustomRoleConfig { get; set; } = new KE.CustomRoles.Config();
        public KE.Items.Config CustomItemConfig { get; set; } = new KE.Items.Config();
        public KE.Map.Config MapConfig { get; set; } = new KE.Map.Config();
        public KE.Misc.Config MiscConfig { get; set; } = new KE.Misc.Config();
        public KE.GlobalEventFramework.Config GEFConfig { get; set; } = new KE.GlobalEventFramework.Config();
        public KE.GlobalEventFramework.Examples.Config GEFEConfig { get; set; } = new KE.GlobalEventFramework.Examples.Config();
    }

}
