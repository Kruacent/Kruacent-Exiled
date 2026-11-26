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
    public class DeployedMScanEventArgs : IExiledEvent, IPlayerEvent, ICustomItemEvent<MScan>
    {
        public MScan Item { get; }

        public Player Player { get; }
        public DeployedMScanEventArgs(Player player,MScan item)
        {
            Player = player;
            Item = item;
        }
    }
}
