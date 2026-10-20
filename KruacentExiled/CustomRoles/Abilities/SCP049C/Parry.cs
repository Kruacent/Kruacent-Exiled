using Exiled.API.Features;
using KruacentExiled.CustomRoles.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.CustomRoles.Abilities.SCP049C
{
    public class Parry : KEAbilities
    {
        public override string Name { get; } = "Parry";


        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Parry",
                    [TranslationKeyDesc] = "Invincible for 0.5 seconds",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Contre",
                    [TranslationKeyDesc] = "Invincible pendant 0.5 secondes",
                }
            };
        }

        public override float Cooldown { get; } = 120f;

        protected override bool AbilityUsed(Player player)
        {
            player.IsGodModeEnabled = true;

            Timing.CallDelayed(.5f, () =>
            {
                player.IsGodModeEnabled = false;
            });         

            return base.AbilityUsed(player);
        }
    }
}
