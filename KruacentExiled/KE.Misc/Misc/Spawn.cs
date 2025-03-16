using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Server;
using KE.Misc.Misc.CR;
using KE.Utils.Display;
using MEC;
using PlayerRoles;
using RueI.Displays;
using RueI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KE.Misc.Misc
{
    public class Spawn
    {
        private bool _set035 = true;
        private Dictionary<Player,Element> _players = [];
        public void OnRoundStarted()
        {
            if (!MainPlugin.Instance.Config.ScpPreferences) return;

            HashSet<Player> pl = Player.List.Where(p=> p.IsScp).ToHashSet();

            foreach (Player player in pl)
            {
                SetScpPreferences(player);
            }
        }

        private void SetScpPreferences(Player player)
        {
            Config config = MainPlugin.Instance.Config;
            if(config == null) return;
            Dictionary<RoleTypeId, int> chancescp = player.ScpPreferences.Preferences.ToDictionary(p => p.Key, p => p.Value + 6);


            RoleTypeId roleScp = ChooseRandomRole(chancescp);
            Log.Debug($"Scp ({player.Nickname}) is {roleScp} previous : {player.Role.Type}");

            player.Role.Set(roleScp);
            SupportClassBackup(player);
            if (config.Scp035Enabled && roleScp == RoleTypeId.Scp079)
            {
                Player pl = Player.List.GetRandomValue(p => p.Role == RoleTypeId.ClassD || p.Role == RoleTypeId.Scientist);
                RoleTypeId otherScp = ChooseRandomRole(pl.ScpPreferences.Preferences.ToDictionary(p => p.Key, p => p.Value + 6));

                if(otherScp == RoleTypeId.Scp079)
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
                    SupportClassBackup(pl);
                    Timing.CallDelayed(1f, () =>
                    {
                        pl.MaxHealth /= 2;
                        pl.Health = pl.MaxHealth;
                    });
                }
                
            }
        }


        private void SupportClassBackup(Player player)
        {
            if (player.Role == RoleTypeId.Scp096 || player.Role == RoleTypeId.Scp079 && !MainPlugin.Instance.Config.Scp035Enabled || MainPlugin.Instance.Config.Debug) return;
            float timetodecide = 60;
            RueIHint h = new(Utils.Display.Enums.HPosition.Center, Utils.Display.Enums.VPosition.CustomRole, "You're a support class \nYou can change your scp by doing the command .scp <scp number>\n(eg .scp 173 -> scp-173)", timetodecide);
            var a = DisplayPlayer.Get(player).Hint(h);
            ChangeSCP._players.Add(player, a);
            Timing.CallDelayed(timetodecide, () =>
            {
                ChangeSCP._players.Remove(player);
                DisplayPlayer.Get(player).RemoveHint(a);
            });
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

        public void EndingRound(EndingRoundEventArgs ev)
        {
            if (Scp035._trackedPlayers.Count <= 0) return;

            if (ev.ClassList.mtf_and_guards != 0 || ev.ClassList.scientists != 0) ev.IsAllowed = false;
            else if (ev.ClassList.class_ds != 0 || ev.ClassList.chaos_insurgents != 0) ev.IsAllowed = false;
            else if (ev.ClassList.scps_except_zombies + ev.ClassList.zombies > 0) ev.IsAllowed = true;
            else ev.IsAllowed = true;
        }
    }



    [CommandHandler(typeof(ClientCommandHandler))]
    public class ChangeSCP : ICommand
    {
        internal static Dictionary<Player,Element> _players = new();
        public string Command { get; } = "scp";
        public string[] Aliases { get; } = new string[] {  };
        public string Description { get; } = "change scp";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player psender = Player.Get(sender);

            if (psender == null)
            {
                response = string.Empty;
                return false;
            }

            if (!_players.ContainsKey(psender))
            {
                response = "you're not authorized to use this command";
                return false;
            }
            if(arguments.Count < 1)
            {
                response = "not enough argument usage: .scp <scp number> \n(eg .scp Scp173 -> scp-173)";
                return false;
            }

            string scp = arguments.At(0);
            if (!Enum.TryParse(scp, out RoleTypeId roleType) && roleType.IsScp() && roleType != RoleTypeId.Scp0492)
            {
                response = "wrong scp number \n(eg .scp Scp173 -> scp-173)";
                return false;
            }
            
            psender.Role.Set(roleType);
            DisplayPlayer.Get(psender).RemoveHint(_players[psender]);
            _players.Remove(psender);

            response = $"you're now SCP-{scp}";
            return true;
        }

    }
}
