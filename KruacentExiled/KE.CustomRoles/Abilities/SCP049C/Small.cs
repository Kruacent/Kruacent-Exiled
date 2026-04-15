using Exiled.API.Features;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.Abilities.SCP049C
{
    public class Small : KEAbilities
    {
        public override string Name { get; } = "Small";


        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Small",
                    [TranslationKeyDesc] = "Get small for 30 seconds",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Potit",
                    [TranslationKeyDesc] = "Devient plus petit pendant 30 secondes",
                }
            };
        }

        public override float Cooldown { get; } = 120f;

        protected override bool AbilityUsed(Player player)
        {

            Vector3 oldScale = player.Scale;
            player.Scale = new Vector3(oldScale.x, 0.8f, oldScale.z);

            Timing.CallDelayed(30, () =>
            {
                player.Scale = oldScale;
            });

            return base.AbilityUsed(player);
        }
    }
}
