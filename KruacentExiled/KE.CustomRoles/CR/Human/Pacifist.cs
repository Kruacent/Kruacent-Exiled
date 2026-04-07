using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.ThrowableProjectiles;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using KE.Utils.API.Features;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.CR.Human
{
    public class Pacifist : KECustomRoleMultipleRole
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Pacifist",
                    [TranslationKeyDesc] = "You're incapable of violence.\nRemove when escaping and bring more people",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Pacifiste",
                    [TranslationKeyDesc] = "T'es idées empêche quelconque violence.\nS'enlève quand tu t'échappes et ramène plus de renfort",
                },
                ["legacy"] = new()
                {
                    [TranslationKeyName] = "Pacifiste",
                    [TranslationKeyDesc] = "T'es idées empêche quelconque violence.\nS'enlève quand tu t'échappes et ramène plus de renfort",
                },
            };
        }
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override float SpawnChance { get; set; } = 100;

        public override HashSet<RoleTypeId> Roles => [RoleTypeId.Scientist,RoleTypeId.ClassD];


        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Item.DisruptorFiring += OnDisruptorFiring;
            Exiled.Events.Handlers.Item.Swinging += OnSwinging;
            Exiled.Events.Handlers.Item.ChargingJailbird += OnChargingJailbird;
            Exiled.Events.Handlers.Player.Shooting += OnShooting;
            Exiled.Events.Handlers.Player.ThrownProjectile += OnThrownProjectile;
            Exiled.Events.Handlers.Player.Escaping += OnEscaping;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Item.DisruptorFiring -= OnDisruptorFiring;
            Exiled.Events.Handlers.Item.Swinging -= OnSwinging;
            Exiled.Events.Handlers.Item.ChargingJailbird -= OnChargingJailbird;
            Exiled.Events.Handlers.Player.Shooting -= OnShooting;
            Exiled.Events.Handlers.Player.ThrownProjectile -= OnThrownProjectile;
            Exiled.Events.Handlers.Player.Escaping -= OnEscaping;
            base.UnsubscribeEvents();
        }
        private void OnThrownProjectile(ThrownProjectileEventArgs ev)
        {
            if (Check(ev.Player))
            {
                ev.Throwable.Destroy();
            }

        }

        private void OnDisruptorFiring(DisruptorFiringEventArgs ev)
        {
            if (Check(ev.Attacker))
            {
                ev.IsAllowed = false;
            }

        }
        private void OnSwinging(SwingingEventArgs ev)
        {
            if (Check(ev.Player))
            {
                ev.IsAllowed = false;
            }
        }
        private void OnChargingJailbird(ChargingJailbirdEventArgs ev)
        {
            if (Check(ev.Player))
            {
                ev.IsAllowed = false;
            }
        }

        private void OnShooting(ShootingEventArgs ev)
        {
            if (Check(ev.Player))
            {
                ev.IsAllowed = false;
            }
        }

        private void OnEscaping(EscapingEventArgs ev)
        {
            Player escape = ev.Player;
            if (Check(escape) && ev.IsAllowed)
            {
                RemoveRole(escape);

                Player respawned = Player.Enumerable.GetRandomValue(p => p.IsDead);


                if (respawned != null)
                {
                    respawned.Role.Set(ev.NewRole, Exiled.API.Enums.SpawnReason.Respawn, RoleSpawnFlags.All);
                    GiveRandomRole(respawned);

                    Timing.CallDelayed(1f, () =>
                    {
                        KELog.Debug(respawned);
                    });
                }



            }
        }



    }
}
