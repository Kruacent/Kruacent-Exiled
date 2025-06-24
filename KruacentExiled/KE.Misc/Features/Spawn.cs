using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Server;
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
            Dictionary<RoleTypeId, int> chancescp = (Dictionary<RoleTypeId, int>) GetPreferences(player);


            RoleTypeId roleScp = ChooseRandomRole(chancescp);
            Log.Debug($"Scp ({player.Nickname}) is {roleScp} previous : {player.Role.Type}");

            player.Role.Set(roleScp);
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
                        pl.MaxHealth /= 2;
                        pl.Health = pl.MaxHealth;
                    });
                }

            }
        }


        private IDictionary<RoleTypeId, int> GetPreferences(Player player)
        {
            if (player.ScpPreferences.Preferences == null) return new Dictionary<RoleTypeId, int>();
            return player.ScpPreferences.Preferences.ToDictionary(p => p.Key, p => p.Value + 5);
        }




        private RoleTypeId ChooseRandomRole(IDictionary<RoleTypeId, int> chancescp)
        {
            if (chancescp == null) throw new ArgumentException("Dictionary null");
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
