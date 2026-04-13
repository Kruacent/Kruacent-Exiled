using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Events.EventsArgs
{
    public class BuffingSCPEventArgs : IExiledEvent, IDeniableEvent, IPlayerEvent
    {
        public bool IsAllowed { get; set; }

        public Player Player { get; }

        public float BuffAmount { get; set; }

        public BuffingSCPEventArgs(Player player,bool isAllowed,float buff)
        {
            Player = player;
            IsAllowed = isAllowed;
            BuffAmount = buff;
        }
    }
}
