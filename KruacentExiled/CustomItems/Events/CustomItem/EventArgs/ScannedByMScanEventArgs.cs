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
    public class ScannedByMScanEventArgs : IExiledEvent, ICustomItemEvent<MScan>, IPlayerEvent
    {
        public MScan Item { get; }

        public Player Player { get; }

        public Player Owner { get; }


        public ScannedByMScanEventArgs(Player player,MScan mscan,Player scanned)
        {
            Owner = player;
            Item = mscan;
            Player = scanned;
        }
    }
}
