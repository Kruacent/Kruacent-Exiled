using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.Human
{
    [CustomRole(RoleTypeId.None)]
    internal class Enderman : GlobalCustomRole, IColor
    {
        public override SideEnum Side { get; set; } = SideEnum.Human;
        public override string Description { get; set; } = "Fais attention tu n'aimes pas l'eau !";
        public override uint Id { get; set; } = 1065;
        public override string PublicName { get; set; } = "Enderman";
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = true;
        public override float SpawnChance { get; set; } = 100;
        public override HashSet<int> Abilities { get; } = new()
        {
            2008,
            2000
        };

        public Color32 Color => new Color32(142, 37, 190, 0);
    }
}