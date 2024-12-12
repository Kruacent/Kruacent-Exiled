using BlackoutKruacent;
using Exiled.API.Features;
using Exiled.Events.Commands.Reload;
using Exiled.Loader;
using GEFExiled.GEFE.API.Features;
using GEFExiled.GEFE.API.Utils;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEFExiled.GEFE.Examples.GE
{

    public class SystemMalfunction : GlobalEvent
    {
        public override int Id { get; set; } = 1;
        public override string Name { get; set; } = "System Malfunction";
        public override string Description { get; set; } = "On dirait que les systèmes informatiques sont défaillants";
        public override double Weight { get; set; } = 1;
        public int NewCooldown { get; set; } = 180;

        public override IEnumerator<float> Start()
        {
            MoreBlackOutNDoors();
            Coroutine.LaunchCoroutine(EarlyNuke());
            yield return 0;
        }

        private IEnumerator<float> EarlyNuke()
        {
            int timeNuke = UnityEngine.Random.Range(15, 30);
            Log.Debug($"the nuke will detonate in {timeNuke}min");
            yield return Timing.WaitUntilTrue(() => timeNuke == Round.ElapsedTime.TotalMinutes);
            Warhead.Start();
            Log.Debug($"kaboom");
        }

        private void MoreBlackOutNDoors()
        {
            var otherPlugin = Loader.Plugins.FirstOrDefault(plugin => plugin.Name == "BlackOutNDoors");
            if (otherPlugin != null)
            {
                if (otherPlugin is BlackoutKruacent.MainPlugin blackout)
                {
                    Log.Info("Found BlackOutNDoors");
                    blackout.ServerHandler.Cooldown = NewCooldown;
                }

            }
        }
        
    }
}