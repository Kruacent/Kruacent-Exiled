using Exiled.API.Enums;
using Exiled.API.Features;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using KruacentExiled.CustomRoles.CR.MTF.Terroriste;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.CustomRoles.CR.MTF.Medic
{
    public class Medic : KECustomRole, IColor
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Medic",
                    [TranslationKeyDesc] = "You are here to heal other's",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Médecin",
                    [TranslationKeyDesc] = "Vas soigner les autres là",
                },
                ["legacy"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Medic",
                    [TranslationKeyDesc] = "You are here to heal other's",
                }
            };
        }

        public override int MaxHealth { get; set; } = 75;
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfSergeant;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public Color32 Color => new Color32(78, 148, 59, 0);
        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(1f, 1f, 1f);

        public override List<string> Inventory { get; set; } = new List<string>()
        {
          $"{ItemType.KeycardMTFOperative}",
          $"{ItemType.GunCOM18}",
          $"{ItemType.Medkit}",
          $"{ItemType.Medkit}",
          $"{ItemType.Adrenaline}",
          $"{ItemType.Radio}",
          $"{ItemType.ArmorLight}",
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
          { AmmoType.Nato9, 70},
        };

        protected override void GiveInventory(Player player)
        {
            base.GiveInventory(player);

            Exiled.CustomItems.API.Features.CustomItem.TryGive(player, "Defibrillator", false);
        }

        public override HashSet<string> Abilities { get; } = new HashSet<string>()
        {
            "HealAura"
        };

        protected override void RoleAdded(Player player)
        {
            player.GameObject.AddComponent<MedicLight>();


            base.RoleAdded(player);
        }

        protected override void RoleRemoved(Player player)
        {
            if (player.GameObject.TryGetComponent<MedicLight>(out var comp))
            {
                Object.Destroy(comp);
            }
            base.RoleRemoved(player);
        }
    }
}
