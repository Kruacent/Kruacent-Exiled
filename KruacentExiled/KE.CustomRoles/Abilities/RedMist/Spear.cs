using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.DamageHandlers;
using Exiled.API.Features.Items;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using Exiled.CreditTags.Features;
using InventorySystem.Items.MicroHID.Modules;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.CR.MTF.RedMist;
using KE.Utils.API.Features;
using PlayerRoles;
using PlayerRoles.FirstPersonControl.Thirdperson;
using PlayerStatsSystem;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
namespace KE.CustomRoles.Abilities.RedMist
{
    public class Spear : KEAbilities
    {
        public override string Name { get; } = "Spear";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Spear",
                    [TranslationKeyDesc] = "Manifest your E.G.O, gain powerful buff but rapid health drain.\nCan be deactivated anytime",
                    ["ForwardSlashFailEGO"] = "You need to manifest your E.G.O. first",
                    ["ForwardSlashFailWeapon"] = "You need your weapon",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "todo",
                    [TranslationKeyDesc] = "todo",
                    ["ForwardSlashFailEGO"] = "todo",
                    ["ForwardSlashFailWeapon"] = "todo",
                }
            };
        }
        public override float Cooldown { get; } = 0f;
        public const float Damage = 100;

        public const float MaxDistance = 15;

        private RaycastHit[] NonAlloc = new RaycastHit[16];

        public static readonly CustomReasonDamageHandler BallDamage = new("Burned to death", 25, string.Empty);
        protected override bool AbilityUsed(Player player)
        {

            
            if (!player.ReferenceHub.gameObject.TryGetComponent<EGO>(out var ego))
            {
                return false;
            }



            if (!ego.Active)
            {
                //show ForwardSlashFailEGO
                return false;
            }
            


            return base.AbilityUsed(player);
        }


    }
}
