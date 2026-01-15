using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using MEC;
using PlayerRoles.FirstPersonControl.Thirdperson.Subcontrollers;
using PlayerStatsSystem;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.Abilities
{
    public class SimulateDeath : KEAbilities, ICustomIcon
    {
        public override string Name { get; } = "SimulateDeath";
        public override string PublicName { get; } = "Simulate Death";

        public override string Description { get; } = "T'es talent de mime te permettent de simuler la mort.";

        public int Duration = 10;
        public Utils.API.GifAnimator.TextImage IconName => MainPlugin.Instance.icons[Name];
        public override float Cooldown { get; } = 60f;

        protected override bool AbilityUsed(Player player)
        {
            Dictionary<byte, DeathTranslation> deathTranslation = DeathTranslations.TranslationsById;

            Vector3 pScale = player.Scale;
            Vector3 pPos = player.Position;
            Ragdoll ragdoll = Ragdoll.CreateAndSpawn(player.Role, player.DisplayNickname, deathTranslation[(byte)UnityEngine.Random.Range(0, deathTranslation.Count)].DeathscreenTranslation, player.Position, player.ReferenceHub.PlayerCameraReference.rotation, player);
            
            player.EnableEffect(EffectType.Invisible, this.Duration);
            player.EnableEffect(EffectType.Ensnared, this.Duration);
            player.EnableEffect(EffectType.AmnesiaItems, this.Duration);
            player.Scale = new Vector3(0.1f, 0.1f, 0.1f);

            Timing.CallDelayed(this.Duration, () =>
            {
                ragdoll.Destroy();
                player.Scale = pScale;
                player.Position = pPos;
            });
            return base.AbilityUsed(player);
        }
    }
}
