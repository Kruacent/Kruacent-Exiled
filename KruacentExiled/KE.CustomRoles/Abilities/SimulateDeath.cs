using Exiled.API.Features;
using KE.CustomRoles.API.Features;
using Exiled.API.Enums;
using PlayerRoles.FirstPersonControl.Thirdperson.Subcontrollers;
using System.Collections.Generic;
using UnityEngine;
using Exiled.API.Extensions;
using MEC;
using PlayerStatsSystem;

namespace KE.CustomRoles.Abilities
{
    public class SimulateDeath : KEAbilities
    {
        public override string Name { get; } = "SimulateDeath";
        public override string PublicName { get; } = "Simulate Death";

        public override string Description { get; } = "T'es talent de mime te permettent de simuler la mort.";

        public int Duration = 10;

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
