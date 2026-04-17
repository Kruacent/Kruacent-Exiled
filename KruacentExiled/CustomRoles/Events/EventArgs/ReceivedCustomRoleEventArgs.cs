using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.CustomRoles.Events.EventArgs
{
    public class ReceivedCustomRoleEventArgs : IExiledEvent, IPlayerEvent, IKECustomRole
    {

        public Player Player { get; }

        public KECustomRole KECustomRole { get; }

        public ReceivedCustomRoleEventArgs(Player player,KECustomRole kECustomRole)
        {
            Player = player;
            KECustomRole = kECustomRole;

        }



    }
}
