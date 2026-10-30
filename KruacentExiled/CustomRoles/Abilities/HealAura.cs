using Exiled.API.Features;
using KruacentExiled.CustomRoles.API.Features.Abilities;
using KruacentExiled.CustomRoles.CR.MTF.Medic;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.CustomRoles.Abilities
{
    public class HealAura : ToggleableAbility
    {
        public override string Name { get; } = "HealAura";

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string> ()
                {
                    [TranslationKeyName] = "Heal Aura",
                    [TranslationKeyDesc] = "You can heal teammates arround you",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Aura de Soin",
                    [TranslationKeyDesc] = "Tu peux soigner les personnes autour de toi",
                }
            };
        }


        public override float Cooldown { get; } = 90f;
        public override Color ColorOn => Color.green;

        public override Color ColorOff => Color.white;

        protected override bool AbilityUsed(Player player)
        {
            if (!player.GameObject.TryGetComponent<HealTeammatesComp>(out var comp))
            {
                comp = player.GameObject.AddComponent<HealTeammatesComp>();
                comp.IsActive = true;
                comp.StartHealingZone();
                return true;
            }

            comp.IsActive = !comp.IsActive;

            if (comp.IsActive)
            {
                comp.StartHealingZone();
            }
            else
            {
                comp.DestroyZoneAndComponent();
            }

            return base.AbilityUsed(player);
        }

        public override bool GetState(Player player)
        {
            if (!player.GameObject.TryGetComponent<HealTeammatesComp>(out var comp))
            {
                return false;
            }

            return comp.IsActive;
        }

    }
}
