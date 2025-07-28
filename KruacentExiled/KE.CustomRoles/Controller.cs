using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using Utils.NonAllocLINQ;

namespace KE.CustomRoles
{
    internal class Controller
    {
        /// <summary>
        /// The chance of having a CustomRole
        /// </summary>
        public const int Chance = 40;


        internal Dictionary<CustomRole, float> GetAvailableCustomRole(Player player)
        {
            return CustomRole.Registered.Where(c => c.Role == player.Role || c is GlobalCustomRole cgr && cgr.Side == SideClass.Get(player.Role.Side)).ToDictionary(c => c, c=> c.SpawnChance);
        }


        /// <summary>
        /// Gives a CustomRole to a player
        /// </summary>
        /// <param name="player"></param>
        internal void GiveRole(Player player)
        {
            if (player == null)
                return;
            if (UnityEngine.Random.Range(0, 101) > Chance)
            {
                Log.Debug("no luck");
                return;
            }
            
            
            CustomRole cr = AssignRole(GetAvailableCustomRole(player));
            Log.Debug($"{player.Id} : {cr.Name}");

            //error assigning cr to a player with a gcr 
            cr?.AddRole(player);
        }

        private CustomRole AssignRole(Dictionary<CustomRole, float> roleChances)
        {
            float totalWeight = roleChances.Values.Sum();
            float randomValue = UnityEngine.Random.Range(0f, totalWeight);

            foreach (var role in roleChances)
            {
                randomValue -= role.Value;
                if (randomValue <= 0)
                    return role.Key;
            }

            return roleChances.Keys.First();
        }


        /// <summary>
        /// Gives CustomRoles to multiple players
        /// </summary>
        /// <param name="players"></param>
        internal void GiveRole(IEnumerable<Player> players)
        {
            foreach (Player p in players)
            {
                GiveRole(p);
            }
        }
    }
}