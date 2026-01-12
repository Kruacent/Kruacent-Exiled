using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp106;
using Exiled.Permissions.Commands.Permissions;
using KE.Utils.API.Interfaces;
using KE.Utils.Extensions;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KE.Misc.Features
{
    public class SCPBuff : IUsingEvents
    {
        public const float RefreshRate = 1f;
        public float IncreaseSCPHealth { get; } = 1.25f;
        internal SCPBuff() { }

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
            
            if (!ev.NewRole.IsScp() || ev.NewRole == RoleTypeId.Scp0492) return;
            if(ev.Player.Role == RoleTypeId.None) return;
            Player p = ev.Player;
            Timing.CallDelayed(2, () =>
            {
                p.MaxHealth *= IncreaseSCPHealth;
                p.Health = p.MaxHealth;
            });
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
