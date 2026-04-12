using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Interfaces;
using KE.Misc.Features.GamblingCoin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Events.EventsArgs.GamblingCoinsEventArgs
{
    public class GambledEventArgs : IExiledEvent, IPlayerEvent, IItemEvent
    {
        /// <summary>
        /// can be null
        /// </summary>
        public Item Item { get; }
        public Player Player { get; }

        public bool CoinBroke { get; }

        public ICoinEffect Effect { get; }

        /// <summary>
        /// no effect if the coins is broken
        /// </summary>
        public int RemainingUses { get; set; }

        public GambledEventArgs(Player player, Item item,ICoinEffect effect,int remaining,bool coinBroke)
        {
            Player = player;
            CoinBroke = coinBroke;
            Item = item;
            Effect = effect;
            RemainingUses = remaining;
        }
    }
}
