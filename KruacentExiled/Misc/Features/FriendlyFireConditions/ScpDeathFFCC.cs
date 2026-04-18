using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Interfaces;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Misc.Features.FriendlyFireConditions
{
    internal class ScpDeathFFCC : FFChangingCondition
    {
        protected override float Chance => 20;

        public override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Dying += OnDying;
            base.SubscribeEvents();
        }

        public override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            base.UnsubscribeEvents();
        }

        private void OnDying(DyingEventArgs ev)
        {
            Player player = ev.Player;
            if (!SCPTeam.IsSCP(player.ReferenceHub) && player.Role != RoleTypeId.Scp0492) return;
            ChangeFriendlyFire();
        }
    }
}
