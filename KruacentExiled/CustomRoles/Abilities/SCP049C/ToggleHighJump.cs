using Exiled.API.Features;
using KruacentExiled.CustomRoles.API.Features.Abilities;
using KruacentExiled.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities.Tier1;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.CustomRoles.Abilities.SCP049C
{
    public class ToggleHighJump : ToggleableAbility
    {
        public override string Name { get; } = "ToggleHighJump";


        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Toggle Big Jump",
                    [TranslationKeyDesc] = "",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Active/désactive grand saut",
                    [TranslationKeyDesc] = "",
                }
            };
        }

        public override float Cooldown { get; } = 2f;

        public override Color ColorOn => Color.green;

        public override Color ColorOff => Color.red;

        

        protected override bool AbilityUsed(Player player)
        {
            if (!player.GameObject.TryGetComponent<HigherJumpComp>(out var comp))
            {
                player.GameObject.AddComponent<HigherJumpComp>();
                return true;
            }
            comp.IsActive = !comp.IsActive;

            return base.AbilityUsed(player);
        }

        public override bool GetState(Player player)
        {
            if (!player.GameObject.TryGetComponent<HigherJumpComp>(out var comp))
            {
                return false;
            }

            return comp.IsActive;
        }
    }
}
