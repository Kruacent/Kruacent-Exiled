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
    public class Medic : KECustomRole, IColor, INoPickupItem
    {
        public string NoPickupTranslationKey => "MTFMEDICNOPICKUPITEM";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Medic",
                    [TranslationKeyDesc] = "You are here to heal others",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Médecin",
                    [TranslationKeyDesc] = "Vas soigner les autres là",
                },
                ["legacy"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Medic",
                    [TranslationKeyDesc] = "You are here to heal others",
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

        public bool BlockScpItems { get; set; } = false;
        public bool BlockRevivingItems { get; set; } = false;
        public bool BlockViolentItems { get; set; } = false;
        public List<string> WhitelistItem { get; set; }

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

        public List<string> BannedItem { get; set; } = new List<string>()
        {
            $"{ItemType.Jailbird}",
            $"{ItemType.ParticleDisruptor}",
            $"{ItemType.MicroHID}",
            $"{ItemType.GunFRMG0}",
            $"{ItemType.GunLogicer}",
            $"{ItemType.GunAK}",
            $"{ItemType.GunE11SR}",
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
          { AmmoType.Nato9, 70},
        };

        protected override void GiveInventory(Player player)
        {
            base.GiveInventory(player);

            Exiled.CustomItems.API.Features.CustomItem.TryGive(player, "Defibrilator", false);
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
