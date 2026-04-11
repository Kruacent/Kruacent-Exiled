using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Usables.Scp330;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using PlayerRoles;
using PlayerRoles.Spectating;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.CR.ClassD
{
    public class Mime : KECustomRole, IColor
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Mime",
                    [TranslationKeyDesc] = "you make almost no sound while walking \nand you're flat",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Mime",
                    [TranslationKeyDesc] = "tu fais très peu de bruit quand tu marches\net t'es tout plat",
                },
                ["legacy"] = new()
                {
                    [TranslationKeyName] = "Mime",
                    [TranslationKeyDesc] = "Tu ne peux pas parler\nmais tu fais très peu de bruit quand tu marches\net t'es tout plat",
                }
            };
        }
        
        public override int MaxHealth { get; set; } = 100;
        public override RoleTypeId Role { get; set; } = RoleTypeId.ClassD;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = true;
        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(0.5f, 1, 1);
        public Color32 Color => new(255, 74, 74, 0);

        protected override void RoleAdded(Player player)
        {
            player.EnableEffect(EffectType.SilentWalk, -1, true); 
        }

        protected override void RoleRemoved(Player player)
        {
            player.DisableEffect(EffectType.SilentWalk);
        }

        public override HashSet<string> Abilities { get; } = new()
        {
            "SimulateDeath"
        };
    }
}
