using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.API;
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
        public const int Chance = 100;
        public static Controller controller = new Controller();

        private Controller() { }

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
                
            CustomRole cr = CustomRole.Registered.GetRandomValue(c => c.Role == player.Role || c is GlobalCustomRole cgr && cgr.Side == SideClass.Get(player.Role.Side));
            Log.Debug($"{player.Id} : {cr.Name}");
            cr?.AddRole(player);
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