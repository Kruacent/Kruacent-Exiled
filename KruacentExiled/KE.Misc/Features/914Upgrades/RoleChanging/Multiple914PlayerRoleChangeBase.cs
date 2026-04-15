using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp914;
using KE.Utils.API.Features;
using KE.Utils.API.Interfaces;
using KE.Utils.Extensions;
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
    public abstract class Multiple914PlayerRoleChangeBase : Base914PlayerUpgrade
    {
        protected static HashSet<Player> _upgradingPlayer = new HashSet<Player>();

        public abstract HashSet<RoleTypeId> InputRole { get; }

        public abstract IReadOnlyDictionary<Scp914KnobSetting, RoleOutput> OutputRoles { get; }
        protected sealed override float Chance => 100;

        protected sealed override bool OnUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            Player player = ev.Player;
            if (!InputRole.Contains(ev.Player.Role)) return false;
            if (!OutputRoles.TryGetValue(ev.KnobSetting, out var newRole)) return false;
            if (_upgradingPlayer.Contains(player)) return false;
            if (!LuckCheck(newRole.chance)) return false;
            
            KELog.Debug($"upgrading {player.Role.Type}->{newRole.role}");


            SetRole(player, newRole.role);

            _upgradingPlayer.Add(player);
            return true;
        }

        protected virtual void SetRole(Player player,RoleTypeId newRole)
        {
            player.ChangeRole(newRole, Exiled.API.Enums.SpawnReason.ForceClass, RoleSpawnFlags.None);
            Timing.CallDelayed(.5f, () =>
            {
                _upgradingPlayer.Remove(player);
            });
        }


        





    }
}
