using Exiled.API.Features.Attributes;
using PlayerRoles;
using UnityEngine;

namespace KE.CustomRoles.CR.ClassD
{
    [CustomRole(RoleTypeId.ClassD)]
    internal class Asmathique : Exiled.CustomRoles.API.Features.CustomRole
    {
        public override string Name { get; set; } = "Asmathique";
        public override string Description { get; set; } = "Tu es <color=#BFFF00>asthmatique</color>\nT'as stamina est réduit de moitié\nMais tu vises mieux";
        public override uint Id { get; set; } = 1048;
        public override string CustomInfo { get; set; } = "Asmathique";
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.ClassD;
        public override bool KeepRoleOnDeath { get; set; } = true;
        public override bool KeepRoleOnChangingRole { get; set; } = true;
        public override bool IgnoreSpawnSystem { get; set; } = true;

        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(1, 0.75f, 1);
    }
}
