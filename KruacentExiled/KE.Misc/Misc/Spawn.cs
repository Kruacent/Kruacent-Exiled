using Exiled.API.Extensions;
using Exiled.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Misc
{
    public class Spawn
    {

        //heal reduit Malfunction
        public void OnRoundStarted()
        {
            HashSet<Player> pl = Player.List.Where(p => p.IsScp).ToHashSet();


            foreach (Player player in pl)
            {
                SetScpPreferences(player);
            }
        }

        private void SetScpPreferences(Player player)
        {
            Dictionary<RoleTypeId, int> chancescp = player.ScpPreferences.Preferences.ToDictionary(p => p.Key, p => p.Value + 6);


            RoleTypeId roleScp = ChooseRandomRole(chancescp);
            Log.Debug($"Scp ({player.Nickname}) is {roleScp} previous : {player.Role.Type}");

            player.Role.Set(roleScp);
            if (roleScp == RoleTypeId.Scp096 || roleScp == RoleTypeId.Scp079)
            {
                player.Role.Set(RoleTypeId.Scp173);
                //TODO the commands
                //ChangeScp(player);
            }
        }


        private void ChangeScp(Player player)
        {
            Hint h = new Hint()
            {
                Content = "You're a support class \nYou can change your scp by doing the command .scp <scp number>\n(eg .scp 173 -> scp-173)",
                Duration = 60,
            };
            player.ShowHint(h, 100);

        }


        private RoleTypeId ChooseRandomRole(Dictionary<RoleTypeId, int> chancescp)
        {
            List<RoleTypeId> weightedPool = new List<RoleTypeId>();
            foreach (RoleTypeId ge in chancescp.Keys)
            {
                for (int i = 0; i < chancescp[ge]; i++)
                {
                    Log.Debug(ge);
                    weightedPool.Add(ge);
                }
            }
            Log.Debug("end");

            int randomIndex = UnityEngine.Random.Range(0, weightedPool.Count);

            return weightedPool[randomIndex];
        }
    }
}
