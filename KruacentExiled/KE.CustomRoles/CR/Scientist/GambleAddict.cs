using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.Abilities;
using KE.CustomRoles.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.Scientist
{
    [CustomRole(RoleTypeId.Scientist)]
    internal class GambleAddict : KECustomRole
    {
        public override string Description { get; set; } = "Tu es un <color=#FFFF7C>Gamble Addict</color> \nT'as trade ton kit et ta carte contre 2 pièces \nfais en bon usage";
        public override uint Id { get; set; } = 1043;
        public override string PublicName { get; set; } = "Gamble Addict";
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scientist;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;

        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(1, 1, 1);

        public override List<string> Inventory { get; set; } = new List<string>()
        {
          $"{ItemType.Coin}",
          $"{ItemType.Coin}",
        };

        public override List<CustomAbility> CustomAbilities { get; set; } = new()
        {
            new Trade()
        };
    }
}
