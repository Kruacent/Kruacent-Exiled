using Exiled.API.Features;
using Exiled.API.Interfaces;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.GlobalEventFramework.GEFE.API.Features
{
    internal class Loader
    {
        internal List<IPlugin<IConfig>> ActivePlugins => new List<IPlugin<IConfig>>();
        internal static Loader Instance { get; private set; } = new Loader();

        private Loader() { }
        internal void Load()
        {
            foreach (IPlugin<IConfig> plugin in Exiled.Loader.Loader.Plugins.Where(pl => pl.Name != MainPlugin.Instance.Name))
            {
                Log.Debug($"checking {plugin.Name}");
                foreach (Type type in plugin.Assembly.GetTypes())
                {
                    try
                    {
                        Log.Debug($"    checking {type.Name}");
                        if (type.IsSubclassOf(typeof(IGlobalEvent)) || type.IsSubclassOf(typeof(GlobalEvent)))
                        {
                            Log.Debug("good");
                            ActivePlugins.Add(plugin);

                            Log.Debug("creating instance");
                            IGlobalEvent ge = Activator.CreateInstance(type) as IGlobalEvent;
                            Log.Debug("registering");
                            GlobalEvent.Register(ge);
                            Log.Debug("end register");
                        }
                    }catch(System.Exception e)
                    {
                        Log.Error($"Error registering in plugin {plugin.Name} : {e.Message}");
                    }
                }
            }
        }
    }
}
