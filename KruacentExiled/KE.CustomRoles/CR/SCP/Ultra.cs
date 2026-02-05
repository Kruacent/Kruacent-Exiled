using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using HintServiceMeow.Core.Models.Arguments;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.Core.Positions;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KE.CustomRoles.CR.SCP
{
    public class Ultra : GlobalCustomRole
    {
        private static Dictionary<Player, CoroutineHandle> _handles = new();
        public override SideEnum Side { get; set; } = SideEnum.SCP;
        public override string Description { get; set; } = "You can sense where people are located";
        public override string PublicName { get; set; } = "Ultra";
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override float MaxHealthMultiplicator { get; set; } = 1f;
        public override float SpawnChance { get; set; } = 100;
        public const float RefreshRate = 20;
        public const int SizeText = 20;

        public static readonly HintPosition UltraPosition = new UltraPosition();
        protected override void RoleAdded(Player player)
        {
            Timing.CallDelayed(1f, () =>
            {
                DisplayHandler.Instance.CreateAuto(player, (AutoContentUpdateArg arg) => PlayerInZone(arg), UltraPosition.HintPlacement);
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
