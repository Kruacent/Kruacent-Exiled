using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.CustomItems.Events.CustomItem.EventArgs
{
    public class UsedDivinePillsEffectArgs : IExiledEvent, IPlayerEvent
    {
        public Player Player { get; }

        public bool Death { get; }

        public UsedDivinePillsEffectArgs(Player player,bool death)
        {
            Player = player;
            Death = death;
        }
    }
}
