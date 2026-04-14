using Exiled.API.Features;
using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled
{
    public abstract class KEPlugin
    {
        public abstract string Name { get; }
        public abstract string Prefix { get; }

        

        public abstract IConfig Config { get; }


        public void InternalOnEnabled()
        {
            if (Config.IsEnabled)
            {
                OnEnabled();

                Log.Send(Name + " has been enabled!", Discord.LogLevel.Info, ConsoleColor.DarkYellow);
            }

            
        }
        public abstract void OnEnabled();
        public abstract void OnDisabled();


    }
}
