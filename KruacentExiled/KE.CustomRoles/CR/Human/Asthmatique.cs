using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;
using Utils.NonAllocLINQ;

namespace KE.CustomRoles.CR.Human
{
    public class Asthmatique : GlobalCustomRole, IColor, IHealable
    {
        public override SideEnum Side { get; set; } = SideEnum.Human;
        public override string Description { get; set; } = "T'as stamina est réduit de moitié\nMais tu vises mieux";
        public override string PublicName { get; set; } = "Asthmatique";
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override float SpawnChance { get; set; } = 100;
        public Color32 Color => new Color32(191, 255, 0, 0);
        public HashSet<ItemType> HealItem => [ItemType.SCP500];

        protected override void RoleAdded(Player player)
        {
            player.EnableEffect(EffectType.Scp1853, -1, true);
            player.EnableEffect(EffectType.Exhausted, -1, true);
        }

        protected override void RoleRemoved(Player player)
        {
            player.DisableEffect(EffectType.Scp1853);
            player.DisableEffect(EffectType.Exhausted);
        }
    }
}
