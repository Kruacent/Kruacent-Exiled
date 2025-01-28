using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using PlayerRoles;
using UnityEngine;
using Utils.NonAllocLINQ;

namespace KE.CustomRoles.CR.ClassD
{
    [CustomRole(RoleTypeId.ClassD)]
    internal class Asthmatique : Exiled.CustomRoles.API.Features.CustomRole
    {
        public override string Name { get; set; } = "Asthmatique";
        public override string Description { get; set; } = "Tu es <color=#BFFF00>asthmatique</color>\nT'as stamina est réduit de moitié\nMais tu vises mieux";
        public override uint Id { get; set; } = 1042;
        public override string CustomInfo { get; set; } = "Asmathique";
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.ClassD;
        public override bool KeepRoleOnDeath { get; set; } = true;
        public override bool KeepRoleOnChangingRole { get; set; } = true;
        public override bool IgnoreSpawnSystem { get; set; } = true;
        public override void AddRole(Player player)
        {
            player.EnableEffect(EffectType.Scp1853, -1,true);
            player.EnableEffect(EffectType.Exhausted, -1, true);
        }
    }
}
