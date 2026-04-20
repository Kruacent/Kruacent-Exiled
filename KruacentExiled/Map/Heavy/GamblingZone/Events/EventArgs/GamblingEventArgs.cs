using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Map.Heavy.GamblingZone.Events.EventArgs
{
    public class GamblingEventArgs : IPlayerEvent, IDeniableEvent, IItemEvent
    {
        public Player Player { get; }

        public bool IsAllowed { get; set; }

        public Item Item { get; }

        public GamblingEventArgs(Player player, Item item,bool isAllowed = true)
        {
            Player = player;
            Item = item;
            IsAllowed = isAllowed;
        }
    }
}
