using Exiled.API.Features;
using System.Collections.Generic;
using UnityEngine;
using KruacentExiled.CustomRoles.API.Features.Abilities;
using KruacentExiled.CustomRoles.CR.MTF.RedMist;
namespace KruacentExiled.CustomRoles.Abilities.RedMist
{
    public class ToggleEGO : ToggleableAbility
    {

        public override Color ColorOn => Color.red;
        public override Color ColorOff => Color.white;

        public override string Name { get; } = "ToggleEGO";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Toggle E.G.O.",
                    [TranslationKeyDesc] = "Manifest your E.G.O, gain powerful buff but rapid health drain.\nCan be deactivated anytime",
                },
                ["fr"] = new Dictionary<string, string>()
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


        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
        }


    }
}
