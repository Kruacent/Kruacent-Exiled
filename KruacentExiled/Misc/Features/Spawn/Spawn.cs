using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Server;
using KE.Utils.API.Features;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using KruacentExiled.Misc;
using KruacentExiled.Misc.Features.Spawn.DummySCPs;
using MEC;
using PlayerRoles;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace KruacentExiled.Misc.Features.Spawn
{
    public class Spawn : MiscFeature
    {

        public static readonly IReadOnlyCollection<ISCPPreferences> allRoles;


        public const int baseValue = 1;


        public static event Action<SpawnedEventArgs> OnAssigned = delegate { };

        private SpawnedEventArgs eventarg;


        static Spawn()
        {
            
            List<ISCPPreferences> role = new List<ISCPPreferences>()
            {
                new Dummy049(),
                new Dummy079(),
                new Dummy096(),
                new Dummy106(),
                new Dummy173(),
                new Dummy939(),
            };

            role.AddRange(CustomSCP.AllValid);

            allRoles = role.AsReadOnly();
        }

        public static ISCPPreferences GetSCP(Player player)
        {
            KECustomRole customrole = CustomSCP.Get(player).FirstOrDefault();

            if(customrole is CustomSCP custom)
            {
                return custom;
            }

            foreach(ISCPPreferences scp in allRoles)
            {
                if(scp is DummySCP dummy)
                {
                    if(dummy.Role == player.Role)
                    {
                        return dummy;
                    }
                }
            }

            return null;
        }

        private void OnRoundStarted()
        {
            eventarg = new SpawnedEventArgs();

            if (MainPlugin.Configs.ScpPreferences)
            {
                int scpcount = 0;
                HashSet<ISCPPreferences> freeRoles = new HashSet<ISCPPreferences>(allRoles);

                foreach (Player player in Player.Enumerable.Where(p => p.IsScp))
                {
                    bool flag;
                    if (scpcount <= 1)
                    {
                        flag = SetScpPreferences(player, freeRoles);
                    }
                    else
                    {
                        flag = SetScpPreferencesWithSupport(player, freeRoles);
                    }

                    if (flag)
                    {
                        scpcount++;
                    }
                }
            }

            

            OnAssigned?.Invoke(eventarg);
        }


        private bool SetScpPreferencesWithSupport(Player player, HashSet<ISCPPreferences> availableRoles)
        {


            //todo make the player choose which support scp they want to play (like scp049cgui)
            return SetScpPreferences(player, availableRoles, false);
        }



        private bool SetScpPreferences(Player player, HashSet<ISCPPreferences> availableRoles, bool ignoreSupport = true)
        {
            Config config = MainPlugin.Configs;
            if (config == null)
            {
                Log.Warn("no config, no custom preferences this round");
                return false;
            }

            
            ISCPPreferences roleScp = ChooseRandomRole(player,availableRoles, ignoreSupport);

            if(roleScp is CustomSCP)
            {
                KELog.Debug("customSCP");
                eventarg.CustomRolesSCPs.Add(player);
            }
            else
            {
                KELog.Debug("vanilla scp");
                eventarg.VanillaSCPs.Add(player);
            }


            KELog.Debug($"Scp ({player.Nickname}) is {roleScp.SCPId} previous : {player.Role.Type}");
            roleScp.Set(player);
            availableRoles.Remove(roleScp);

            return true;
            
        }

        private ISCPPreferences ChooseRandomRole(Player player, HashSet<ISCPPreferences> availableRoles, bool ignoreSupport)
        {

            List<ISCPPreferences> weightedPool = new List<ISCPPreferences>();



            if(availableRoles.Count == 0)
            {
                availableRoles = allRoles.ToHashSet();
            }


            foreach (ISCPPreferences scp in availableRoles)
            {
                if (scp.IsSupport && ignoreSupport) continue;

                int pref = scp.GetPreferences(player) + 5 + baseValue;

                KELog.Debug(scp.SCPId + " : " + pref);
                for (int i = 0; i < pref; i++)
                {
                    
                    weightedPool.Add(scp);
                }
            }

            int randomIndex = UnityEngine.Random.Range(0, weightedPool.Count);

            return weightedPool[randomIndex];
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
