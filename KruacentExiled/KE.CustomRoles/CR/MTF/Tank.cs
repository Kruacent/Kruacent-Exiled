using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Player = Exiled.Events.Handlers.Player;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;
using System;
using KE.CustomRoles.API;

namespace KE.CustomRoles.CR.ClassD
{
    [CustomRole(RoleTypeId.NtfCaptain)]
    internal class Tank : KECustomRole
    {
        public override string Name { get; set; } = "Tank";
        public override string Description { get; set; } = "Tu es un <color=#FFC0CB>TANK</color> tu es débuff mais ta force de tir est démultiplié (fait attention à tes balles)";
        public override uint Id { get; set; } = 1051;
        public override string CustomInfo { get; set; } = "Tank";
        public override int MaxHealth { get; set; } = 200;
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfCaptain;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;

        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(1.15f, 1.15f, 1.15f);

        public override List<string> Inventory { get; set; } = new List<string>()
       {
          $"{ItemType.GunLogicer}",
          $"{ItemType.GunFRMG0}",
          $"{ItemType.Radio}",
          $"{ItemType.GrenadeHE}",
          $"{ItemType.Adrenaline}",
          $"{ItemType.KeycardMTFCaptain}",
          $"{ItemType.Painkillers}",
          $"{ItemType.ArmorHeavy}"
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
          { AmmoType.Nato762, 200},
          { AmmoType.Nato556, 200}
        };

        protected override void SubscribeEvents()
        {
            Player.Shooting += Shooting;
            base.SubscribeEvents();
        }

        /// <inheritdoc/>
        protected override void UnsubscribeEvents()
        {
            Player.Shooting -= Shooting;
            base.UnsubscribeEvents();
        }

        private void Shooting(ShootingEventArgs ev)
        {
            Timing.CallDelayed(0.5f, () =>
            {
                Timing.RunCoroutine(EffectAttribution(ev.Player));
            });
        }

        private IEnumerator<float> EffectAttribution(Exiled.API.Features.Player player)
        {
            int nbMunition = player.GetAmmo(AmmoType.Nato762) / 100;
            byte nbMunitionByte = (byte) nbMunition;

            if (UnityEngine.Random.Range(0, 1) > 0.5f){
                player.DisableEffect(EffectType.Slowness);
                player.EnableEffect(EffectType.Slowness, nbMunitionByte, 99999, false);
            }

            yield return 0;
        }
    }
}
