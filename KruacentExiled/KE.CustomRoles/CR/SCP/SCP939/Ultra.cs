using Exiled.API.Enums;
using Exiled.API.Features;
using HintServiceMeow.Core.Models.Arguments;
using KE.CustomRoles.API.Core.Positions;
using KE.CustomRoles.API.Features;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KE.CustomRoles.CR.SCP.SCP939
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
        protected override void RoleAdded(Player player)
        {
            Timing.CallDelayed(1f, () =>
            {
                DisplayHandler.Instance.CreateAuto(player, (arg) => PlayerInZone(arg), UltraPosition.HintPlacement);
            });
        }
        private string PlayerInZone(AutoContentUpdateArg arg)
        {

            if (!TrackedPlayers.Contains(Player.Get(arg.PlayerDisplay.ReferenceHub)))
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
            return Player.List.Count(p => !p.IsScp && p.Zone == zone);
        }
    }
}
