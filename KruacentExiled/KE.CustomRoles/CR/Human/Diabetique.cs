using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;
using Utils.NonAllocLINQ;

namespace KE.CustomRoles.CR.Human
{
    public class Diabetique : GlobalCustomRole, IColor, IHealable
    {
        public override SideEnum Side { get; set; } = SideEnum.Human;
        public override string Description { get; set; } = "T'as mangé le crambleu au pomme de mael";
        public override string PublicName { get; set; } = "Diabetique";
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override float SpawnChance { get; set; } = 100;
        public HashSet<ItemType> HealItem => [ItemType.SCP500];
        public Color32 Color => new(255, 255, 0,0);

        protected override void RoleAdded(Player player)
        {
            player.EnableEffect(EffectType.Scp207, -1, true);
        }
        protected override void RoleRemoved(Player player)
        {
            player.DisableEffect(EffectType.Scp207);
        }
    }
}
