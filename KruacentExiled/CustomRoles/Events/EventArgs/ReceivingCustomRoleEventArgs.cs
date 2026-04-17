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
    public class ReceivingCustomRoleEventArgs : IExiledEvent, IPlayerEvent, IDeniableEvent, IKECustomRole
    {

        public Player Player { get; }


        public bool IsAllowed { get; set; }

        public KECustomRole KECustomRole { get; }

        public ReceivingCustomRoleEventArgs(Player player, KECustomRole kECustomRole, bool isAllowed = true)
        {
            Player = player;
            KECustomRole = kECustomRole;
            IsAllowed = isAllowed;

        }



    }
}
