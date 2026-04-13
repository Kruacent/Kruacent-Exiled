using CommandSystem.Commands.RemoteAdmin;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using InventorySystem;
using InventorySystem.Items.Usables.Scp330;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.ClassD
{
    public class Enfant : KECustomRole, IColor
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Kid",
                    [TranslationKeyDesc] = "do not the kid \nyou start with a rainbow candy (for real this time) \nyou're a bit smaller",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Enfant",
                    [TranslationKeyDesc] = "do not the kid \ntu commences avec un bonbon arc-en-ciel (pour de vrai cette fois) \n t'es un peu plus petit que la normal",
                },
                ["legacy"] = new()
                {
                    [TranslationKeyName] = "Enfant",
                    [TranslationKeyDesc] = "do not the kid \ntu commences avec un bonbon arc-en-ciel (pour de vrai cette fois) \n t'es un peu plus petit que la normal",
                }
            };
        }


        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.ClassD;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = true;
        public Color32 Color => new(255, 192, 203, 0);

        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(1, 0.75f, 1);


        protected override void GiveInventory(Player player)
        {
            player.ReferenceHub.GrantCandy(CandyKindID.Rainbow, InventorySystem.Items.ItemAddReason.StartingItem);
            base.GiveInventory(player);
        }
    }
    
}
