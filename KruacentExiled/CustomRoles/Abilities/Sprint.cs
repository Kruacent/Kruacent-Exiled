using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.Utils.API.GifAnimator;
using KE.Utils.Extensions;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.CustomRoles.Abilities
{
    public class Sprint : KEAbilities, ICustomIcon
    {
        public override string Name { get; } = "Sprint";

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Sprint",
                    [TranslationKeyDesc] = "You run faster for a short time",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Sprint",
                    [TranslationKeyDesc] = "Cours plus vite pendant 5s",
                }
            };
        }

        public override float Cooldown { get; } = 95f;

        public TextImage IconName => MainPlugin.Instance.icons[Name];

        protected override bool AbilityUsed(Player player)
        {
            List<StatusEffectBase> effectList = player.ActiveEffects.ToList();

            bool wentIn = false;

            foreach (var effect in effectList)
            {
                if(effect.GetEffectType() == EffectType.MovementBoost)
                {
                    effect.Intensity += 50;
                    Timing.CallDelayed(5f, () =>
                    {
                        effect.Intensity -= 50;
                    });
                    wentIn = true;
                }

            }

            if (!wentIn) 
            { 
                player.EnableEffect(EffectType.MovementBoost, duration: 5, intensity: 50);
            }


            return true;
        }
    }
}
