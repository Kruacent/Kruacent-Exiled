using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;
using System;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using Exiled.API.Features;

namespace KE.CustomRoles.CR.MTF
{
    public class Tank : KECustomRole, IColor
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Tank",
                    [TranslationKeyDesc] = "The more bullet you got, the slower you are",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Tank",
                    [TranslationKeyDesc] = "Tu es ralenti selon ton nombre de balle",
                }
            };
        }

        public override int MaxHealth { get; set; } = 200;
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfSergeant;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public Color32 Color => new (255, 192, 203,0);
        public override float SpawnChance { get; set; } = 100;
        public override Vector3 Scale { get; set; } = new Vector3(1.15f, 1f, 1.15f);

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
            Exiled.Events.Handlers.Player.Shooting += Shooting;
            base.SubscribeEvents();
        }

        /// <inheritdoc/>
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Shooting -= Shooting;
            base.UnsubscribeEvents();
        }

        private void Shooting(ShootingEventArgs ev)
        {
            if (!Check(ev.Player)) return;

            Timing.CallDelayed(0.5f, () =>
            {
                EffectAttribution(ev.Player);
            });
        }

        private void EffectAttribution(Player player)
        {
            int nbMunition = player.GetAmmo(AmmoType.Nato762) / 100;
            byte nbMunitionByte = (byte)Mathf.Clamp(nbMunition,byte.MinValue,byte.MaxValue);

            if (UnityEngine.Random.Range(0, 1) > 0.5f)
            {
                player.DisableEffect(EffectType.Slowness);
                player.EnableEffect(EffectType.Slowness, nbMunitionByte, 99999, false);
            }

        }
    }
}
