using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.API.Features;
using KE.Items.API.Features;
using KE.Utils.API.Features;
using MEC;
using PlayerRoles;
using System.Collections.Generic;

namespace KE.CustomRoles.CR.Human
{
    public class Pacifist : KECustomRoleMultipleRole
    {
        public const string TranslationCantPickup = "PacifistCantPickup";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Pacifist",
                    [TranslationKeyDesc] = "You're incapable of violence.\nRemove when escaping and bring more people",
                    [TranslationCantPickup] = "Picking up this item make you sick",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Pacifiste",
                    [TranslationKeyDesc] = "T'es idées empêche quelconque violence.\nS'enlève quand tu t'échappes et ramène plus de renfort",
                    [TranslationCantPickup] = "Juste la vue de cet objet te rend malade",
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
            Exiled.Events.Handlers.Player.Escaping += OnEscaping;
            Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem;
            Exiled.Events.Handlers.Player.ItemAdded += OnItemAdded;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Item.DisruptorFiring -= OnDisruptorFiring;
            Exiled.Events.Handlers.Item.Swinging -= OnSwinging;
            Exiled.Events.Handlers.Item.ChargingJailbird -= OnChargingJailbird;
            Exiled.Events.Handlers.Player.Shooting -= OnShooting;
            Exiled.Events.Handlers.Player.Escaping -= OnEscaping;
            Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpItem;
            Exiled.Events.Handlers.Player.ItemAdded -= OnItemAdded;
            base.UnsubscribeEvents();
        }

        private void OnItemAdded(ItemAddedEventArgs ev)
        {
            Player player = ev.Player;
            if (!Check(player)) return;
            Item item = ev.Item;

            if (IsViolent(item))
            {
                ShowEffectHint(player, GetTranslation(player, TranslationCantPickup));
                Timing.CallDelayed(.01f, () =>
                {
                    player.DropItem(item);
                });
            }           
        }


        private void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            Player player = ev.Player;
            if (!ev.IsAllowed) return;
            if (!Check(player)) return;

            bool isViolent = IsViolent(ev.Pickup);
            ev.IsAllowed = !isViolent;

            KELog.Debug("violent " + isViolent);
            if (isViolent)
            {
                ShowEffectHint(player, GetTranslation(player, TranslationCantPickup));
            }
        }

        private bool IsViolent(Pickup pickup)
        {
            bool result = IsViolent(pickup.Type);
            if (KECustomItem.TryGet(pickup, out CustomItem ci))
            {
                if (ci is KECustomItem kecr)
                {
                    result = KECustomItem.IsConsideredViolent(kecr);
                }

            }
            return result;
        }

        private bool IsViolent(Item item)
        {
            bool result = IsViolent(item.Type);
            if (KECustomItem.TryGet(item, out CustomItem ci))
            {
                if (ci is KECustomItem kecr)
                {
                    result = KECustomItem.IsConsideredViolent(kecr);
                }

            }
            return result;
        }

        private bool IsViolent(ItemType itemtype)
        {
            if (itemtype.IsWeapon())
            {
                return true;
            }

            if (itemtype.GetCategory() == ItemCategory.Firearm)
            {
                return true;
            }

            ProjectileType projectileType = itemtype.GetProjectileType();
            if (projectileType == ProjectileType.FragGrenade || projectileType == ProjectileType.Scp018)
            {
                return true;
            }
            return false;
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
