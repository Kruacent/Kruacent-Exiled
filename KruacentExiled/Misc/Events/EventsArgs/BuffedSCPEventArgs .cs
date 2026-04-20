using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Misc.Events.EventsArgs
{
    public class BuffedSCPEventArgs : IExiledEvent, IPlayerEvent
    {

        public Player Player { get; }

        public float BuffAmount { get; set; }

        public BuffedSCPEventArgs(Player player,float buff)
        {
            Player = player;
            BuffAmount = buff;
        }
    }
}
