using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using KruacentExiled.CustomItems.API.Interface;
using KruacentExiled.CustomItems.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.CustomItems.Events.CustomItem.EventArgs
{
    public class RemovedMScanEventArgs : IExiledEvent, IPlayerEvent, ICustomItemEvent<MScan>
    {
        public MScan Item { get; }

        public Player Player { get; }

        public RemovedMScanEventArgs(Player player,MScan item)
        {
            Item = item;
            Player = player;
        }
    }
}
