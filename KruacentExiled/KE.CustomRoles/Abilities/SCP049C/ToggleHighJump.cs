using Exiled.API.Features;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities.Tier1;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.Abilities.SCP049C
{
    public class ToggleHighJump : KEAbilities
    {
        public override string Name { get; } = "ToggleHighJump";


        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Toggle Big Jump",
                    [TranslationKeyDesc] = "WIP",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Active/désactive grand saut",
                    [TranslationKeyDesc] = "WIP",
                }
            };
        }

        public override float Cooldown { get; } = 2f;

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
    }
}
