using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using KE.CustomRoles.API.Features;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KE.CustomRoles.CR.SCP
{
    [CustomRole(RoleTypeId.None)]
    public class Ultra : GlobalCustomRole
    {
        private static Dictionary<Player, CoroutineHandle> _handles = new();
        public override SideEnum Side { get; set; } = SideEnum.SCP;
        public override string Description { get; set; } = "You can sense where people are located";
        public override uint Id { get; set; } = 1079;
        public override string PublicName { get; set; } = "Ultra";
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override float MaxHealthMultiplicator { get; set; } = 1f;
        public override float SpawnChance { get; set; } = 100;
        public const float RefreshRate = 20;
        public const int SizeText = 20;
        protected override void RoleAdded(Player player)
        {
            _handles.Add(player, Timing.RunCoroutine(DisplayInfos(player)));
        }

        protected override void RoleRemoved(Player player)
        {
            Timing.KillCoroutines(_handles[player]);
            _handles.Remove(player);
        }



        private IEnumerator<float> DisplayInfos(Player player)
        {

            while (true)
            {
                Log.Debug("Ultra : showing");
                ShowEffectHint(player, PlayerInZone());
                yield return Timing.WaitForSeconds(RefreshRate);
            }
        }


        private string PlayerInZone()
        {
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
