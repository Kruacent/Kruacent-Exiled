using Exiled.API.Features.Attributes;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.Scientist
{
    [CustomRole(RoleTypeId.Scientist)]
    internal class GambleAddict : Exiled.CustomRoles.API.Features.CustomRole
    {
        public override string Name { get; set; } = "GambleAddict";
        public override string Description { get; set; } = "Tu es un <color=#FFFF7C>Gamble Addict</color> \nT'as trade ton kit et ta carte contre 4 pièces \nfais en bon usage";
        public override uint Id { get; set; } = 1403;
        public override string CustomInfo { get; set; } = "GambleAddict";
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scientist;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override bool IgnoreSpawnSystem { get; set; } = true;

        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(1, 1, 1);

        public override List<string> Inventory { get; set; } = new List<string>()
       {
          $"{ItemType.Coin}",
          $"{ItemType.Coin}",
          $"{ItemType.Coin}",
          $"{ItemType.Coin}"
        };
    }
}
