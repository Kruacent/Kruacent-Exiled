using Exiled.API.Features;
using KE.CustomRoles.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.Abilities.RedMist
{
    public class ToggleEGO : KEAbilities
    {
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



        protected override bool AbilityUsed(Player player)
        {
            if(!player.GameObject.TryGetComponent<EGO>(out var ego))
            {
                player.GameObject.AddComponent<EGO>();
            }

            ego.ToggleActive();



            return base.AbilityUsed(player);
        }


    }
}
