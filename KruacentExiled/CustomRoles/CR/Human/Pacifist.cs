using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Features;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using MEC;
using PlayerRoles;
using System.Collections.Generic;

namespace KruacentExiled.CustomRoles.CR.Human
{
    public class Pacifist : KECustomRoleMultipleRole, INoPickupItem
    {
        public string NoPickupTranslationKey => "PacifistCantPickup";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Pacifist",
                    [TranslationKeyDesc] = "You're incapable of violence.\nRemove when escaping and bring more people",
                    [NoPickupTranslationKey] = "Picking up this item make you sick",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Pacifiste",
                    [TranslationKeyDesc] = "T'es idées empêche quelconque violence.\nS'enlève quand tu t'échappes et ramène plus de renfort",
                    [NoPickupTranslationKey] = "Juste la vue de cet objet te rend malade",
                },
                ["legacy"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Pacifiste",
                    [TranslationKeyDesc] = "T'es idées empêche quelconque violence.\nS'enlève quand tu t'échappes et ramène plus de renfort",
                },
            };
        }
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override float SpawnChance { get; set; } = 100;
        public override bool KeepInventoryOnSpawn { get; set; } = true;

        public override HashSet<RoleTypeId> Roles => new HashSet<RoleTypeId>() { RoleTypeId.Scientist, RoleTypeId.ClassD };

        public List<string> BannedItem { get; set; } = null;
        public List<string> WhitelistItem { get; set; } = null;
        public bool BlockScpItems { get; set; } = false;
        public bool BlockRevivingItems { get; set; } = false;
        public bool BlockViolentItems { get; set; } = true;

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Item.DisruptorFiring += OnDisruptorFiring;
            Exiled.Events.Handlers.Item.Swinging += OnSwinging;
            Exiled.Events.Handlers.Item.ChargingJailbird += OnChargingJailbird;
            Exiled.Events.Handlers.Player.Shooting += OnShooting;
            Exiled.Events.Handlers.Player.Escaping += OnEscaping;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Item.DisruptorFiring -= OnDisruptorFiring;
            Exiled.Events.Handlers.Item.Swinging -= OnSwinging;
            Exiled.Events.Handlers.Item.ChargingJailbird -= OnChargingJailbird;
            Exiled.Events.Handlers.Player.Shooting -= OnShooting;
            Exiled.Events.Handlers.Player.Escaping -= OnEscaping;
            base.UnsubscribeEvents();
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
                    respawned.Role.Set(ev.NewRole, SpawnReason.Respawn, RoleSpawnFlags.All);
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
