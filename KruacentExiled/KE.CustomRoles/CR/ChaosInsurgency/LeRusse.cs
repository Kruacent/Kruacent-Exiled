using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.ChaosInsurgency
{
    public class Russe : KECustomRole, IColor
    {
        public override string Description { get; set; } = "Tu dois faire la roulette russe avec les autres joueurs";
        public override string InternalName => "Russe";
        public override string PublicName { get; set; } = "Le Russe";
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosRifleman;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public Color32 Color => new(255, 0, 0, 0);

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
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
          { AmmoType.Ammo44Cal, 18}
        };
    }
}
