using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using KruacentExiled.CustomItems.Items;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.CustomRoles.CR.Human
{
    public class Curiosophile : GlobalCustomRole, IColor
    {
        public override SideEnum Side { get; set; } = SideEnum.Human;
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Curiosophile",
                    [TranslationKeyDesc] = "",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Curiosophile",
                    [TranslationKeyDesc] = "",
                },
                ["legacy"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Curiosophile",
                    [TranslationKeyDesc] = "",
                }

            };
        }
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = true;
        public override float SpawnChance { get; set; } = 100;
        public Color32 Color => new Color32(9, 25, 67,0);

        private Dictionary<string, int> ItemValues = new Dictionary<string, int>()
        {
            { "TrueDivinePills", -30 },
        };

        /// <summary>
        /// < 0 -> Buff player
        /// 0 -> Neutral
        /// > 0 -> Nerf player
        /// </summary>
        /// <param name="inventory"></param>
        /// <returns></returns>
        private int InventoryScore(List<Item> inventory)
        {
            int inventoryScore = 0;
            foreach (Item item in inventory)
            {
                inventoryScore += ItemScore(item);
            }

            return inventoryScore;
        } 

        private int ItemScore(Item item)
        {
            ItemTierFlags

            // IRevivingItem, SainteGrenada -> -30
            // item arme spécial -> -20 // ItemCategory.SpecialWeapon
            // OverseerProtocolDocument -> -15
            // arme lourde -> -15 (Logicer, Capitaine)
            // item SCP -> -10 (sauf mami)
            // Com 45 -> -7
            // keycard avec plus de 7 permission -> -5
            // TP Grenada -> -3

            // Soin +3
            // item osef -> flashlight, radio -> +4
            // SCP Mami, Smoke Grenade, Impact Flash, Presse Purée -> +5
            // Arme légère +8 (Com 15, Com 18, Crossvec, FSP9)
            // HealZone, Molotov, Pièce, LGG, Proximity Grenade -> + 15
            // A7 -> +20
            // Life Ender -> +40
        }
    }
}
