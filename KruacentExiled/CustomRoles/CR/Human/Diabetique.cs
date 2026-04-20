using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.CustomRoles.CR.Human
{
    public class Diabetique : GlobalCustomRole, IColor, IHealable, IEffectImmunity
    {
        public override SideEnum Side { get; set; } = SideEnum.Human;
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Diabetic",
                    [TranslationKeyDesc] = "Fucking type 1. 1",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Diabétique",
                    [TranslationKeyDesc] = "T'as mangé le crambleu au pomme de mael",
                },
                ["legacy"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Diabetique",
                    [TranslationKeyDesc] = "T'as mangé le crambleu au pomme de mael",
                }

            };
        }
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override float SpawnChance { get; set; } = 100;
        public HashSet<ItemType> HealItem => new HashSet<ItemType>() { ItemType.SCP500 };
        public Color32 Color => new Color32(255, 255, 0,0);

        public HashSet<EffectType> ImmuneEffects => new HashSet<EffectType>() { EffectType.Poisoned };

        protected override void RoleAdded(Player player)
        {

            Timing.CallDelayed(TimeAttributingInventory, () =>
            {
                if (player.IsInventoryFull)
                {
                    Pickup.CreateAndSpawn(ItemType.Medkit, player.Position);
                }
                else
                {
                    player.AddItem(ItemType.Medkit);
                }

            });
            

            player.EnableEffect(EffectType.Scp207, -1, true);
        }
        protected override void RoleRemoved(Player player)
        {
            player.DisableEffect(EffectType.Scp207);
        }
    }
}
