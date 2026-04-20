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




        internal void StartBuff()
        {
            Timing.RunCoroutine(PeanutShield());
            
        }


        private void BecomingSCP(ChangingRoleEventArgs ev)
        {
            Player player = ev.Player;
            if (!ev.NewRole.IsScp() || ev.NewRole == RoleTypeId.Scp0492) return;
            if(player.Role == RoleTypeId.None) return;
            float healthincrease = IncreaseSCPHealth;
            if(RoleBuff.TryGetValue(ev.NewRole,out float val) && Player.Enumerable.Count(p => SCPTeam.IsSCP(p.ReferenceHub)) > 1)
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


        private IEnumerator<float> PeanutShield()
        {
            while (Round.InProgress)
            {
                List<Player> peanuts = Player.List.Where(p => p.Role == RoleTypeId.Scp173).ToList();
                peanuts.ForEach(p =>
                {
                    AddHumeShield(p, CheckPlayerAround(p, 6));
                });
                yield return Timing.WaitForSeconds(RefreshRate);
            }
        }


        /// <summary>
        /// Count the number of player around
        /// </summary>
        /// <param name="p">The Player (the peanut) to check around them</param>
        /// <param name="radius"></param>
        /// <param name="countFriendly"></param>
        /// <returns></returns>
        private int CheckPlayerAround(Player p, float radius, bool countFriendly = false)
        {
            int result = 0;
            foreach (Player player in Player.List)
            {
                if (player == p) continue;
                if (player.Role.Side == p.Role.Side && !countFriendly) continue;
                if (IsPlayerInZone(player, p.Position, radius, radius))
                    result += 5;
            }
            return result;
        }


        private bool IsPlayerInZone(Player player, Vector3 zonePosition, float radius, float height)
        {
            
            float horizontalDistance = Vector3.Distance(
                new Vector3(player.Position.x, 0, player.Position.z),
                new Vector3(zonePosition.x, 0, zonePosition.z)
            );

            // Calculate the vertical difference (y)
            float verticalDifference = Mathf.Abs(player.Position.y - zonePosition.y);

            // Check if the player is in the 3d zone.
            return horizontalDistance <= radius / 2 && verticalDifference <= height / 2;
        }


        /// <summary>
        /// Add HumeShield to a player 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="hum"></param>
        /// <returns>true if it's not over the max HumeShield of the player; false otherwise</returns>
        private bool AddHumeShield(Player p, float hum)
        {
            float max = p.MaxHumeShield;
            if (max < hum + p.HumeShield)
            {
                p.HumeShield = max;
                return false;
            }
            p.HumeShield += hum;
            return true;
        }


    }
}
