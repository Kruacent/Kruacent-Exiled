using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using KE.CustomRoles.API.Features;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KE.Misc.Features
{
    public class Spawn : MiscFeature
    {

        private Dictionary<string, RoleTypeId> baseRole = new ()
        {
            { "173", RoleTypeId.Scp173 },
            { "106", RoleTypeId.Scp106 },
            { "049", RoleTypeId.Scp049 },
            { "939", RoleTypeId.Scp939 },
        };

        private Dictionary<string,CustomSCP> SelectableCustomSCPs => CustomSCP.All.ToDictionary(cs =>cs.Name, cs => cs);


        public static event Action<IEnumerable<Player>> OnAssigned = delegate { };

        private void OnRoundStarted()
        {
            if (!MainPlugin.Instance.Config.ScpPreferences) return;

            List<Player> players = new();


            foreach (Player player in Player.List.Where(p => p.IsScp && !p.IsNPC))
            {
                if (SetScpPreferences(player))
                {
                    players.Add(player);
                }
            }

            OnAssigned?.Invoke(players.ToList());
        }

        private bool SetScpPreferences(Player player)
        {
            Config config = MainPlugin.Instance.Config;
            if (config == null)
            {
                Log.Warn("no config, no custom preferences this round");
                return false;
            }
            Dictionary<string, int> chancescp = GetPreferences(player);

            if(chancescp == null)
            {
                Log.Error("no setting found");
                return false;
            }


            string roleScp = ChooseRandomRole(chancescp);
            Log.Debug($"Scp ({player.Nickname}) is {roleScp} previous : {player.Role.Type}");
            SetRoleWithId(player, roleScp);
            return true;
            
        }


        private Dictionary<string, int> GetPreferences(Player player)
        {
            if (player.ScpPreferences.Preferences == null) return null;
            Dictionary<string, int> idChance = new();


            foreach(var kvp in baseRole)
            {
                idChance.Add(kvp.Key, player.ScpPreferences.Preferences[kvp.Value] + 5);
            }

            foreach (CustomSCP customSCP in SelectableCustomSCPs.Values)
            {
                idChance.Add(customSCP.Name, customSCP.GetPreferences(player) + 5);
            }


            return idChance;
        }




        private string ChooseRandomRole(IDictionary<string, int> chancescp)
        {
            if (chancescp == null) throw new ArgumentException("Dictionary null");
            List<string> weightedPool = new();
            foreach (string ge in chancescp.Keys)
            {
                for (int i = 0; i < chancescp[ge]; i++)
                {
                    Log.Debug(ge);
                    weightedPool.Add(ge);
                }
            }

            int randomIndex = UnityEngine.Random.Range(0, weightedPool.Count);

            return weightedPool[randomIndex];
        }

        private void SetRoleWithId(Player player, string name)
        {

            if (baseRole.ContainsKey(name))
            {
                player.Role.Set(baseRole[name]);
                Log.Info("vanilla scp");
                return;
            }

            if (SelectableCustomSCPs.ContainsKey(name))
            {
                SelectableCustomSCPs[name].AddRole(player);
                Log.Info("custom scp");
                return;
            }

            throw new Exception($"SCP ({name}) not found");
                


        }



        public override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.EndingRound += EndingRound;
            Exiled.Events.Handlers.Server.AllPlayersSpawned += OnRoundStarted;
        }

        public override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.EndingRound -= EndingRound;
            Exiled.Events.Handlers.Server.AllPlayersSpawned -= OnRoundStarted;
        }

        public void EndingRound(EndingRoundEventArgs ev)
        {
            //if (Scp035._trackedPlayers.Count <= 0) return;

            //if (ev.ClassList.mtf_and_guards != 0 || ev.ClassList.scientists != 0) ev.IsAllowed = false;
            //else if (ev.ClassList.class_ds != 0 || ev.ClassList.chaos_insurgents != 0) ev.IsAllowed = false;
            //else if (ev.ClassList.scps_except_zombies + ev.ClassList.zombies > 0) ev.IsAllowed = true;
            //else ev.IsAllowed = true;
        }
    }



}
