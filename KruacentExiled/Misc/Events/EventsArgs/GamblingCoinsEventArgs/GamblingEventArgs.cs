using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Misc.Events.EventsArgs.GamblingCoinsEventArgs
{
    public class GamblingEventArgs : IExiledEvent, IPlayerEvent, IItemEvent, IDeniableEvent
    {

        public Item Item { get; }
        public Player Player { get; }
        public bool IsAllowed { get; set; }

        public GamblingEventArgs(Player player, Item item,bool isAllowed = true)
        {
            Item = item;
            Player = player;
            IsAllowed = isAllowed;
        }
    }
}
