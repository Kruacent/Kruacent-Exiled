using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.Human
{
    public class Cleptoman : GlobalCustomRole
    {
        public override SideEnum Side { get; set; } = SideEnum.Human;
        public override string Description { get; set; } = "Tu es un <color=#FFFF7C>Cccleptoman</color> \nTu peux voler les items des autres joueurs";
        public override uint Id { get; set; } = 1453;
        public override string PublicName { get; set; } = "Cleptoman";
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = true;

        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(1.01f, 0.99f, 1);

        public override HashSet<int> Abilities { get; } = new()
        {
            2006
        };
    }
}