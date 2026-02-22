using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Pickups;
using Exiled.CustomRoles.API.Features;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;
using Utils.NonAllocLINQ;

namespace KE.CustomRoles.CR.Human
{
    public class Diabetique : GlobalCustomRole, IColor, IHealable, IEffectImmunity
    {
        public override SideEnum Side { get; set; } = SideEnum.Human;
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Diabetic",
                    [TranslationKeyDesc] = "Fucking type 1. 1",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Diabetique",
                    [TranslationKeyDesc] = "T'as mangé le crambleu au pomme de mael",
                }
            };
        }
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override float SpawnChance { get; set; } = 100;
        public HashSet<ItemType> HealItem => [ItemType.SCP500];
        public Color32 Color => new(255, 255, 0,0);

        public HashSet<EffectType> ImmuneEffects => [EffectType.Poisoned];

        protected override void RoleAdded(Player player)
        {

            Timing.CallDelayed(KECustomRole.TimeAttributingInventory, () =>
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
