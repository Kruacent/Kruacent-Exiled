using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.Events.EventArgs
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
