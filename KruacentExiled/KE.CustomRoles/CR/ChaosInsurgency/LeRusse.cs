using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using KE.CustomRoles.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.ChaosInsurgency
{
    [CustomRole(RoleTypeId.ChaosConscript)]
    internal class Russe : KECustomRole
    {
        public override string Name { get; set; } = "Russe";
        public override string Description { get; set; } = "Tu es un <color=#FFC0CB>maitre de jeu</color> \ntu dois faire la roulette russe avec les autres joueurs";
        public override uint Id { get; set; } = 1050;
        public override string CustomInfo { get; set; } = "Le Russe";
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosConscript;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;

        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(1.1f, 1f, 1.1f);

        public override List<string> Inventory { get; set; } = new List<string>()
       {
          $"{ItemType.GunRevolver}",
          $"{ItemType.Radio}",
          $"{ItemType.Adrenaline}",
          $"{ItemType.KeycardChaosInsurgency}",
          $"{ItemType.GrenadeHE}",
          $"{ItemType.GrenadeHE}",
          $"{ItemType.GrenadeHE}"
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
          { AmmoType.Ammo44Cal, 100}
        };
    }
}
