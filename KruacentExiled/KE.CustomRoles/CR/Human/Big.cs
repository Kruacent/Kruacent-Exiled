using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.Human
{
    public class Big : GlobalCustomRole
    {
        public override SideEnum Side { get; set; } = SideEnum.Human;
        public override string Description { get; set; } = "Faut arrêter le McDo au bout d'un moment !";
        public override uint Id { get; set; } = 1068;
        public override string PublicName { get; set; } = "Big";
        public override bool KeepRoleOnDeath { get; set; } = true;
        public override bool KeepRoleOnChangingRole { get; set; } = true;

        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(1, 1, 1.4f);
    }
}