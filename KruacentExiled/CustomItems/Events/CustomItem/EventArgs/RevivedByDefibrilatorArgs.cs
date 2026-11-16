using Exiled.Events.EventArgs.Interfaces;
using LabApi.Events.Arguments.Interfaces;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPlayerEvent = Exiled.Events.EventArgs.Interfaces.IPlayerEvent;

namespace KruacentExiled.CustomItems.Events.CustomItem.EventArgs
{
    public class RevivedByDefibrilatorArgs : IExiledEvent, IPlayerEvent, ITargetEvent
    {

        /// <summary>
        /// The player using the <see cref="Items.Defibrillator"/>
        /// </summary>
        public Player Target { get; }
        /// <summary>
        /// The player revived
        /// </summary>
        public Exiled.API.Features.Player Player { get; }


        public RevivedByDefibrilatorArgs(Player player, Player target)
        {
            Target = target;
            Player = player;
        }
    }
}
