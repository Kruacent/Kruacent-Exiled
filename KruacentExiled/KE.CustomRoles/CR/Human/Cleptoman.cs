using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.Human
{
    public class Cleptoman : GlobalCustomRole, IColor
    {
        public override SideEnum Side { get; set; } = SideEnum.Human;
        public override string Description { get; set; } = "Tu peux voler les items des autres joueurs";
        public override string PublicName { get; set; } = "cccCleptoman";
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = true;
        public Color32 Color => new Color32(194, 0, 0, 0);

        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(1.01f, 0.99f, 1);

        public override HashSet<string> Abilities { get; } = new()
        {
            "Thief"
        };
    }
}