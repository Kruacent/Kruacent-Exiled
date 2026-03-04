using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using KE.CustomRoles.API.Interfaces.Ability;
using MEC;
using PlayerRoles.FirstPersonControl.Thirdperson.Subcontrollers;
using PlayerStatsSystem;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.Abilities
{
    public class SimulateDeath : KEAbilities, ICustomIcon, IDuration
    {
        public override string Name { get; } = "SimulateDeath";

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Play dead",
                    [TranslationKeyDesc] = "Simulate your own death",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Faire le mort",
                    [TranslationKeyDesc] = "T'es talent de mime te permettent de simuler la mort",
                }
            };
        }

        public Utils.API.GifAnimator.TextImage IconName => MainPlugin.Instance.icons[Name];
        public override float Cooldown { get; } = 60f;
        public float Duration { get; set; } = 10f;

        private Ragdoll ragdoll;
        private Vector3 pScale;
        private Vector3 pPos;

        protected override bool AbilityUsed(Player player)
        {
            Dictionary<byte, DeathTranslation> deathTranslation = DeathTranslations.TranslationsById;

            this.pScale = player.Scale;
            this.pPos = player.Position;
            this.ragdoll = Ragdoll.CreateAndSpawn(player.Role, player.DisplayNickname, deathTranslation[(byte)UnityEngine.Random.Range(0, deathTranslation.Count)].DeathscreenTranslation, player.Position, player.ReferenceHub.PlayerCameraReference.rotation, player);
            
            player.EnableEffect(EffectType.Invisible, this.Duration);
            player.EnableEffect(EffectType.Ensnared, this.Duration);
            player.EnableEffect(EffectType.AmnesiaItems, this.Duration);
            player.Scale = new Vector3(0.1f, 0.1f, 0.1f);

            return base.AbilityUsed(player);
        }

        public void ActionAfterAbility(Player player)
        {
            this.ragdoll.Destroy();
            player.Scale = this.pScale;
            player.Position = this.pPos;
        }
    }
}
