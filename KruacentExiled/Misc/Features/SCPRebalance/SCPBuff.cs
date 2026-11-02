using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp106;
using Exiled.Permissions.Commands.Permissions;
using KE.Utils.API.Features;
using KE.Utils.API.Interfaces;
using KE.Utils.Extensions;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using KruacentExiled.Misc.Events.EventsArgs;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KruacentExiled.Misc.Features.SCPRebalance
{
    public class SCPBuff : IUsingEvents
    {
        public const float RefreshRate = 1f;
        public float IncreaseSCPHealth => MainPlugin.Configs.MultBaseSCP;
        private static Config Config => MainPlugin.Configs;
        private readonly Dictionary<Player, CoroutineHandle> _activeBuffCoroutines = new Dictionary<Player, CoroutineHandle>();

        public Dictionary<RoleTypeId, float> RoleBuff = new Dictionary<RoleTypeId, float>()
        {
            {RoleTypeId.Scp049, Config.MultSCP049 },
            {RoleTypeId.Scp939, Config.MultSCP939 },
            {RoleTypeId.Scp106, Config.MultSCP106 },

        };

        


        internal SCPBuff() { }

        public static event Action<BuffingSCPEventArgs> OnBuffingSCP = delegate { };
        public static event Action<BuffedSCPEventArgs> OnBuffedSCP = delegate { };

        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ChangingRole += BecomingSCP;
            Exiled.Events.Handlers.Player.Left += OnPlayerLeft;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ChangingRole -= BecomingSCP;
            Exiled.Events.Handlers.Player.Left -= OnPlayerLeft;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;

            ClearAllCoroutines();
        }
        private void OnRoundStarted()
        {
            ClearAllCoroutines();
        }

        private void OnPlayerLeft(LeftEventArgs ev)
        {
            CancelBuff(ev.Player);
        }

        private void BecomingSCP(ChangingRoleEventArgs ev)
        {
            Player player = ev.Player;
            if (player == null) return;
            CancelBuff(player);
            if (!ev.NewRole.IsScp() || ev.NewRole == RoleTypeId.Scp0492) return;
            if(player.Role == RoleTypeId.None) return;
            float healthIncrease = IncreaseSCPHealth;
            if(RoleBuff.TryGetValue(ev.NewRole,out float val) && SCPTeam.SCPs.Count > 1)
            {
                healthIncrease *= val;
            }

            BuffingSCPEventArgs ev1 = new BuffingSCPEventArgs(player, true, healthIncrease);
            
            OnBuffingSCP?.Invoke(ev1);

            if (!ev1.IsAllowed) return;

            KELog.Debug("before coroutine timingcalldelayed");

            CoroutineHandle handle = Timing.CallDelayed(2f, () => ApplyBuff(player, healthIncrease));
            _activeBuffCoroutines[player] = handle;
        }
        
        private void ApplyBuff(Player player, float healthIncrease)
        {
            _activeBuffCoroutines.Remove(player);
            if (player == null || !player.IsConnected || !player.IsAlive || !SCPTeam.IsSCP(player.ReferenceHub))
            {
                return;
            }

            player.MaxHealth *= healthIncrease;
            player.Health = player.MaxHealth;

            KELog.Debug("health increase");

            BuffedSCPEventArgs ev2 = new BuffedSCPEventArgs(player, healthIncrease);
            OnBuffedSCP?.Invoke(ev2);
        }

        private void CancelBuff(Player player)
        {
            if (player != null)
            {
                KELog.Debug("cancelled buff");
                _activeBuffCoroutines.TryGetValue(player, out CoroutineHandle handle);
                Timing.KillCoroutines(handle);
                _activeBuffCoroutines.Remove(player);
            }
        }

        private void ClearAllCoroutines()
        {
            foreach (CoroutineHandle handle in _activeBuffCoroutines.Values)
            {
                Timing.KillCoroutines(handle);
                KELog.Debug("coroutine clear");
            }
            _activeBuffCoroutines.Clear();
        }
    }
}
