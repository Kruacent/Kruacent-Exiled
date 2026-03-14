using Exiled.API.Features;
using KE.CustomRoles.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KE.CustomRoles.CR.MTF.RedMist;
using KE.CustomRoles.API.Features.Abilities;
using UnityEngine;
namespace KE.CustomRoles.Abilities.RedMist
{
    public class ToggleEGO : ToggleableAbility
    {

        public override Color ColorOn => Color.red;
        public override Color ColorOff => Color.white;

        public override string Name { get; } = "ToggleEGO";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Toggle E.G.O.",
                    [TranslationKeyDesc] = "Manifest your E.G.O, gain powerful buff but rapid health drain.\nCan be deactivated anytime",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "todo",
                    [TranslationKeyDesc] = "todo",
                }
            };
        }
        public override float Cooldown { get; } = 0f;

        public override bool GetState(Player player)
        {
            if (!player.ReferenceHub.gameObject.TryGetComponent<EGO>(out var ego))
            {
                return false;
            }

            return ego.Active;
        }
        protected override bool AbilityUsed(Player player)
        {
            if(!player.ReferenceHub.gameObject.TryGetComponent<EGO>(out var ego))
            {
                return false;
            }

            ego.ToggleActive();



            return base.AbilityUsed(player);
        }


    }
}
