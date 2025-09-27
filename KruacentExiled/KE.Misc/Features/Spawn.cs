using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Server;
using KE.CustomRoles.API.Features;
using KE.Misc.Features.CR;
using KE.Utils.API.Interfaces;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KE.Misc.Features
{
    internal class Spawn : MiscFeature
    {
        private RoleTypeId[] baseRole =
        [
            RoleTypeId.Scp173,
            RoleTypeId.Scp106,
            RoleTypeId.Scp049,
            RoleTypeId.Scp079,
            RoleTypeId.Scp096,
            RoleTypeId.Scp939,
        ];

        private Dictionary<int,CustomSCP> SelectableCustomSCPs => CustomSCP.All.Where(cs => cs.Id >= baseRole.Length).ToDictionary(cs => (int)cs.Id, cs => cs);

        private bool _set035 = true;


        public void OnRoundStarted()
        {
            if (!MainPlugin.Instance.Config.ScpPreferences) return;

            


            foreach (Player player in Player.List.Where(p => p.IsScp && !p.IsNPC))
            {
                SetScpPreferences(player);
            }
        }

        private void SetScpPreferences(Player player)
        {
            Config config = MainPlugin.Instance.Config;
            if (config == null)
            {
                Log.Warn("no config, no custom preferences this round");
                return;
            }
            Dictionary<int, int> chancescp = GetPreferences(player);

            if(chancescp == null)
            {
                Log.Error("no setting found");
            }


            int roleScp = ChooseRandomRole(chancescp);
            Log.Debug($"Scp ({player.Nickname}) is {roleScp} previous : {player.Role.Type}");

            SetRoleWithId(player, roleScp);
            /*
            if (config.Scp035Enabled && roleScp == RoleTypeId.Scp079)
            {
                Player pl = Player.List.GetRandomValue(p => p.Role == RoleTypeId.ClassD || p.Role == RoleTypeId.Scientist);
                RoleTypeId otherScp = ChooseRandomRole(GetPreferences(pl));

                if (otherScp == RoleTypeId.Scp079)
                {
                    _set035 = !_set035;
                    if (_set035)
                    {
                        CustomRole scp = CustomRole.Registered.FirstOrDefault(c => c.Id == 10);
                        scp.AddRole(pl);
                        pl.Teleport(SpawnLocationType.Inside096);
                    }
                    else
                    {
                        pl.Role.Set(otherScp);
                    }
                }
                else
                {
                    pl.Role.Set(otherScp);
                    Timing.CallDelayed(1f, () =>
                    {
                        pl.MaxHealth /= 4;
                        pl.Health = pl.MaxHealth;
                    });
                }

            }*/
        }


        private Dictionary<int, int> GetPreferences(Player player)
        {
            if (player.ScpPreferences.Preferences == null) return null;
            Dictionary<int, int> idChance = new();

            for (int i = 0; i < baseRole.Length; i++)
            {
                idChance.Add(i, player.ScpPreferences.Preferences[baseRole[i]]+5);
            }

            foreach(CustomSCP customSCP in SelectableCustomSCPs.Values)
            {
                idChance.Add((int)customSCP.Id, customSCP.GetPreferences(player)+5);
            }



            return idChance;
        }




        private int ChooseRandomRole(IDictionary<int, int> chancescp)
        {
            if (chancescp == null) throw new ArgumentException("Dictionary null");
            List<int> weightedPool = new();
            foreach (int ge in chancescp.Keys)
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

        private void SetRoleWithId(Player player, int id)
        {

            if(id < baseRole.Length)
            {
                player.Role.Set(baseRole[id],SpawnReason.RoundStart);
            }
            else
            {
                SelectableCustomSCPs[id].AddRole(player);
            }
        }



        public override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.EndingRound += EndingRound;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }

        public override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.EndingRound -= EndingRound;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        }

        public void EndingRound(EndingRoundEventArgs ev)
        {
            if (Scp035._trackedPlayers.Count <= 0) return;

            if (ev.ClassList.mtf_and_guards != 0 || ev.ClassList.scientists != 0) ev.IsAllowed = false;
            else if (ev.ClassList.class_ds != 0 || ev.ClassList.chaos_insurgents != 0) ev.IsAllowed = false;
            else if (ev.ClassList.scps_except_zombies + ev.ClassList.zombies > 0) ev.IsAllowed = true;
            else ev.IsAllowed = true;
        }
    }



}
