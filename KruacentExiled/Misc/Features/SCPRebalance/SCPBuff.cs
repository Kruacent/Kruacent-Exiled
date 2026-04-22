using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp106;
using Exiled.Permissions.Commands.Permissions;
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
        public float IncreaseSCPHealth { get; } = 1.25f;
        private static Config Config => MainPlugin.Configs;

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
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ChangingRole -= BecomingSCP;
        }





        private void BecomingSCP(ChangingRoleEventArgs ev)
        {
            Player player = ev.Player;
            if (!ev.NewRole.IsScp() || ev.NewRole == RoleTypeId.Scp0492) return;
            if(player.Role == RoleTypeId.None) return;
            float healthincrease = IncreaseSCPHealth;
            if(RoleBuff.TryGetValue(ev.NewRole,out float val) && SCPTeam.SCPs.Count > 1)
            {
                healthincrease *= val;
            }

            BuffingSCPEventArgs ev1 = new BuffingSCPEventArgs(player, true, healthincrease);
            
            OnBuffingSCP?.Invoke(ev1);
            if (ev1.IsAllowed)
            {
                Timing.CallDelayed(2, () =>
                {
                    player.MaxHealth *= healthincrease;
                    player.Health = player.MaxHealth;
                });
                BuffedSCPEventArgs ev2 = new BuffedSCPEventArgs(player, healthincrease);
                OnBuffedSCP?.Invoke(ev2);
            }


        }
    }
}
