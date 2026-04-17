using Exiled.API.Enums;
using Exiled.API.Features;
using KE.Utils.API.GifAnimator;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using KruacentExiled.CustomRoles.API.Interfaces.Ability;
using PlayerStatsSystem;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.CustomRoles.Abilities
{
    public class SimulateDeath : KEAbilities, ICustomIcon, IDuration
    {
        public override string Name { get; } = "SimulateDeath";

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Play dead",
                    [TranslationKeyDesc] = "Simulate your own death",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Faire le mort",
                    [TranslationKeyDesc] = "T'es talent de mime te permettent de simuler la mort",
                }
            };
        }

        public TextImage IconName => MainPlugin.Instance.icons[Name];
        public override float Cooldown => 60f;
        public float Duration => 10f;

        private Ragdoll ragdoll;
        private Vector3 pScale;
        private Vector3 pPos;

        protected override bool AbilityUsed(Player player)
        {
            Dictionary<byte, DeathTranslation> deathTranslation = DeathTranslations.TranslationsById;

            pScale = player.Scale;
            pPos = player.Position;
            ragdoll = Ragdoll.CreateAndSpawn(player.Role, player.DisplayNickname, deathTranslation[(byte)Random.Range(0, deathTranslation.Count)].DeathscreenTranslation, player.Position, player.ReferenceHub.PlayerCameraReference.rotation, player);
            
            player.EnableEffect(EffectType.Invisible, Duration);
            player.EnableEffect(EffectType.Ensnared, Duration);
            player.EnableEffect(EffectType.AmnesiaItems, Duration);
            player.Scale = new Vector3(0.1f, 0.1f, 0.1f);

            return base.AbilityUsed(player);
        }

        public void ActionAfterAbility(Player player)
        {
            ragdoll.Destroy();
            player.Scale = pScale;
            player.Position = pPos;
        }
    }
}
