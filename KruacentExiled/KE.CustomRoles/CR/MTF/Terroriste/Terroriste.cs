using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.MTF.Terroriste
{
    public class Terroriste : KECustomRole, IColor
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Terrorist",
                    [TranslationKeyDesc] = "Kaboom!",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Terroriste",
                    [TranslationKeyDesc] = "Ne fait pas exploser la facilité \ntu commences avec des grenades",
                },
                ["legacy"] = new()
                {
                    [TranslationKeyName] = "Terroriste",
                    [TranslationKeyDesc] = "Ne fait pas exploser la facilité \ntu commences avec des grenades",
                },
            };
        }
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfSergeant;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public Color32 Color => new(105, 52, 22, 0);

        public override float SpawnChance { get; set; } = 100;
        public override List<string> Inventory { get; set; } = new List<string>()
       {
          $"{ItemType.GrenadeHE}",
          $"{ItemType.GrenadeHE}",
          $"{ItemType.ArmorCombat}",
          $"{ItemType.GunE11SR}",
          $"{ItemType.Adrenaline}",
          $"{ItemType.KeycardMTFOperative}",
          $"{ItemType.Radio}"
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato556, 100}
        };

        public override HashSet<string> Abilities { get; } = new()
        {
            "Explode"
        };


        protected override void RoleAdded(Player player)
        {
            player.GameObject.AddComponent<TerroristeLight>();


            base.RoleAdded(player);
        }

        protected override void RoleRemoved(Player player)
        {
            if(player.GameObject.TryGetComponent<TerroristeLight>(out var comp))
            {
                Object.Destroy(comp);
            }
            base.RoleRemoved(player);
        }
    }
}
