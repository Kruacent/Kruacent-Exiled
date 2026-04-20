using Exiled.API.Enums;
using Exiled.API.Features;
using HintServiceMeow.Core.Models.Arguments;
using HintServiceMeow.Core.Utilities;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KE.Utils.API.Features;
using KruacentExiled.CustomRoles.API.Core.Positions;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using MEC;
using Mirror;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KruacentExiled.CustomRoles.CR.SCP.SCP939
{
    public class Ultra : KECustomRole
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Ultra SCP-939",
                    [TranslationKeyDesc] = "You can sense where people are located",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Ultra SCP-939",
                    [TranslationKeyDesc] = "Tu sais où est tout le monde",
                },
                ["legacy"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Ultra SCP-939",
                    [TranslationKeyDesc] = "You can sense where people are located",
                },
            };
        }
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override float SpawnChance { get; set; } = 100;
        public override int MaxHealth { get; set; } = 2700;

        public override RoleTypeId Role => RoleTypeId.Scp939;

        public const float RefreshRate = 20;
        public const int SizeText = 20;

        public static readonly HintPosition UltraPosition = new UltraPosition();


        public const float UpdateCooldown = 60f;
        private Dictionary<Player, DateTime> lastUpdate;
        private Dictionary<Player, CoroutineHandle> handles;

        protected override void SubscribeEvents()
        {
            lastUpdate = new Dictionary<Player, DateTime>();
            handles = new Dictionary<Player, CoroutineHandle>();
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            lastUpdate = null;
            handles = null;
        }



        protected override void RoleAdded(Player player)
        {
            lastUpdate.Add(player, DateTime.Now);
            //DisplayHandler.Instance.CreateAuto(player, (arg) => PlayerInZone(player), UltraPosition.HintPlacement, HintServiceMeow.Core.Enum.HintSyncSpeed.UnSync);
            handles.Add(player, Timing.RunCoroutine(Loop(player)));

        }


        protected override void RoleRemoved(Player player)
        {
            DisplayHandler.Instance.RemoveHint(player, UltraPosition.HintPlacement);
            lastUpdate.Remove(player);

            Timing.KillCoroutines(handles[player]);
            handles.Remove(player);
        }

        private IEnumerator<float> Loop(Player player)
        {
            while (Check(player))
            {
                PlayerDisplay display = PlayerDisplay.Get(player);
                KELog.Debug("update");
                DisplayHandler.Instance.AddHint(UltraPosition.HintPlacement, player, PlayerInZone(player), UpdateCooldown);
                yield return Timing.WaitForSeconds(UpdateCooldown);
                

            }


        }



        private string PlayerInZone(Player player)
        {

            if (!Check(player))
            {
                return string.Empty;
            }
            
            string result = $"<size={SizeText}>";
            int nbPlayer;
            foreach (ZoneType zone in Enum.GetValues(typeof(ZoneType)))
            {
                nbPlayer = GetPlayerInZone(zone);
                if (nbPlayer > 0 || MainPlugin.Instance.Config.Debug)
                {
                    result += zone.ToString() + " : " + nbPlayer + "\n";
                }
            }
            result += "</size>";
            return result;
        }

        private int GetPlayerInZone(ZoneType zone)
        {
            return Player.Enumerable.Count(p => !SCPTeam.IsSCP(p.ReferenceHub) && p.IsAlive && p.Zone == zone);
        }
    }
}
