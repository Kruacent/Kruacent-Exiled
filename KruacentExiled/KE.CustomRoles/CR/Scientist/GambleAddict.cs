using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.Abilities;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using PlayerRoles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.Scientist
{
    public class GambleAddict : KECustomRole, IColor
    {
        public override string Description { get; set; } = "T'as trade ton kit et ta carte contre 2 pièces \nfais en bon usage";
        public override string PublicName { get; set; } = "Gamble Addict";
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scientist;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;

        public override float SpawnChance { get; set; } = 100;

        public override List<string> Inventory { get; set; } = new List<string>()
        {
          $"{ItemType.Coin}",
          $"{ItemType.Coin}",
        };

        public override HashSet<string> Abilities { get; } = new()
        {
            "Trade"
        };

        public Color32 Color => new(0, 105, 59,0);
    }
}
