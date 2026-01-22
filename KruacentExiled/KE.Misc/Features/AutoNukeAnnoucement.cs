using Exiled.API.Features;
using GameCore;
using KE.Utils.API.Interfaces;
using MEC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features
{
    public class NukeKill : IUsingEvents
    {
        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Warhead.Detonated += OnDetonated;
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Warhead.Detonated -= OnDetonated;
            
        }

        private void OnDetonated()
        {
            Timing.CallDelayed(10, () =>
            {
                foreach(Player player in Player.Enumerable)
                {
                    if(player.Zone != Exiled.API.Enums.ZoneType.Surface)
                    {
                        player.Kill(Exiled.API.Enums.DamageType.Warhead);
                    }

                    if (player.Lift is not null)
                    {
                        player.Kill(Exiled.API.Enums.DamageType.Warhead);
                    }

                }
            });
        }




    }
}
