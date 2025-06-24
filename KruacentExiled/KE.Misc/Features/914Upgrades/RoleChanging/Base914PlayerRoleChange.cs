using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp914;
using KE.Utils.API.Interfaces;
using MEC;
using PlayerRoles;
using Scp914;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features._914Upgrades
{
    public abstract class Base914PlayerRoleChange : Base914PlayerUpgrade
    {
        public abstract RoleTypeId InputRole { get; }

        public abstract IReadOnlyDictionary<Scp914KnobSetting, RoleOutput> OutputRoles { get; }
        protected sealed override float Chance => 100;



        protected sealed override void OnUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {

            if (ev.Player.Role != InputRole) return;
            if (!OutputRoles.TryGetValue(ev.KnobSetting, out var newRole)) return;
            if (!LuckCheck(newRole.chance)) return;
            Log.Debug($"upgrading {ev.Player.Role}->{newRole.role}");

            Timing.CallDelayed(.5f, () =>
            {
                SetRole(ev.Player, newRole.role);
            });

            
            
        }

        protected virtual void SetRole(Player player,RoleTypeId newRole)
        {
            player.Role.Set(newRole, RoleSpawnFlags.None);
        }


        





    }
}
