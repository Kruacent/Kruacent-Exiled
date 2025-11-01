using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.Abilities;
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
        public override string Description { get; set; } = $"Tu peux te téléporter ! T tro for enféte";
        public override string PublicName { get; set; } = "Enderman";
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = true;
        public override float SpawnChance { get; set; } = 100;
        public override HashSet<string> Abilities { get; } = new()
        {
            "Teleportation",
            "SetPosition"
        };

        public Color32 Color => new Color32(142, 37, 190, 0);
    }
}