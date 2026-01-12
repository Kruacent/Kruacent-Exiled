using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using KE.CustomRoles.API.Features;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KE.Misc.Features.Spawn
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


        public static event Action<SpawnedEventArgs> OnAssigned = delegate { };

        private SpawnedEventArgs eventarg;

        private void OnRoundStarted()
        {
            if (!MainPlugin.Instance.Config.ScpPreferences) return;

            eventarg = new();

            foreach (Player player in Player.List.Where(p => p.IsScp && !p.IsNPC))
            {
                SetScpPreferences(player);
            }

            OnAssigned?.Invoke(eventarg);
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
                eventarg.VanillaRoles.Add(player);
                
                Log.Info("vanilla scp");
                return;
            }

            if (SelectableCustomSCPs.ContainsKey(name))
            {
                SelectableCustomSCPs[name].AddRole(player);
                eventarg.CustomRoles.Add(player);
                Log.Info("custom scp");
                return;
            }

            throw new Exception($"SCP ({name}) not found");
                


        }



        public override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.AllPlayersSpawned += OnRoundStarted;
        }

        public override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.AllPlayersSpawned -= OnRoundStarted;
        }
    }



}
