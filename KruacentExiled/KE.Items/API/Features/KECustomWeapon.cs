using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Features;
using InventorySystem.Items.Firearms.Modules;
using KE.Items.API.Features.SpawnPoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Items.API.Features
{
    public abstract class KECustomWeapon : KECustomItem
    {


        public virtual float Damage { get; set; } = -1;
        public virtual byte ClipSize { get; }
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Shooting += InternalOnShooting;
            Exiled.Events.Handlers.Player.Hurting += InternalOnHurting;
            Exiled.Events.Handlers.Player.ReloadingWeapon += OnInternalReloading;
            Exiled.Events.Handlers.Player.ReloadedWeapon += OnInternalReloaded;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Shooting -= InternalOnShooting;
            Exiled.Events.Handlers.Player.Hurting -= InternalOnHurting;
            Exiled.Events.Handlers.Player.ReloadingWeapon -= OnInternalReloading;
            Exiled.Events.Handlers.Player.ReloadedWeapon -= OnInternalReloaded;
            base.UnsubscribeEvents();
        }

      
        private void InternalOnHurting(HurtingEventArgs ev)
        {
            if (Check(ev.Attacker.CurrentItem))
            {
                OnHurting(ev);
            }

        }
        private void InternalOnShooting(ShootingEventArgs ev)
        {
            if (Check(ev.Item))
            {
                OnShooting(ev);
            }
        }

        private void OnInternalReloading(ReloadingWeaponEventArgs ev)
        {
            if (Check(ev.Item))
            {
                if (ClipSize > 0 && ev.Firearm.TotalAmmo >= ClipSize)
                {
                    ev.IsAllowed = false;
                }
                else
                {
                    OnReloading(ev);
                }
            }
        }
        private void OnInternalReloaded(ReloadedWeaponEventArgs ev)
        {

            if (!Check(ev.Item))
                return;

            if (ClipSize > 0)
            {
                int ammoChambered = ((AutomaticActionModule)ev.Firearm.Base.Modules.FirstOrDefault(x => x is AutomaticActionModule))?.AmmoStored ?? 0;
                int ammoToGive = ClipSize - ammoChambered;

                AmmoType ammoType = ev.Firearm.AmmoType;
                int firearmAmmo = ev.Firearm.MagazineAmmo;
                int ammoDrop = -(ClipSize - firearmAmmo - ammoChambered);

                int ammoInInventory = ev.Player.GetAmmo(ammoType) + firearmAmmo;
                if (ammoToGive < ammoInInventory)
                {
                    ev.Firearm.MagazineAmmo = ammoToGive;
                    int newAmmo = ev.Player.GetAmmo(ammoType) + ammoDrop;
                    ev.Player.SetAmmo(ammoType, (ushort)newAmmo);
                }
                else
                {
                    ev.Firearm.MagazineAmmo = ammoInInventory;
                    ev.Player.SetAmmo(ammoType, 0);
                }
            }
            
            OnReloaded(ev);
        }

        protected virtual void OnShooting(ShootingEventArgs ev)
        {

        }

        protected virtual void OnHurting(HurtingEventArgs ev)
        {
            if (ev.IsAllowed && Damage >= 0)
            {
                ev.Amount = Damage;
                
            }
        }
        protected virtual void OnReloading(ReloadingWeaponEventArgs ev)
        {

        }

        protected virtual void OnReloaded(ReloadedWeaponEventArgs ev)
        {

        }


        public override Pickup Spawn(Vector3 position, Player previousOwner = null)
        {
            if (Item.Create(Type) is not Firearm firearm)
            {
                Log.Debug("Spawn: Item is not Firearm.");
                return null;
            }


            Pickup pickup = firearm.CreatePickup(position);
            if (pickup == null)
            {
                Log.Debug("Spawn: Pickup is null.");
                return null;
            }

            if (ClipSize > 0)
            {
                firearm.MagazineAmmo = ClipSize;
            }

            pickup.Weight = Weight;
            pickup.Scale = Scale;
            if ((object)previousOwner != null)
            {
                pickup.PreviousOwner = previousOwner;
            }

            base.TrackedSerials.Add(pickup.Serial);
            return pickup;
        }

        public override Pickup Spawn(Vector3 position, Item item, Player previousOwner = null)
        {
            if (item is Firearm firearm)
            {

                if (ClipSize > 0)
                {
                    firearm.MagazineAmmo = ClipSize;
                }

                int magazineAmmo = firearm.MagazineAmmo;
                Log.Debug(string.Format("{0}.{1}: Spawning weapon with {2} ammo.", "Name", "Spawn", magazineAmmo));
                Pickup pickup = firearm.CreatePickup(position);
                pickup.Scale = Scale;
                if ((object)previousOwner != null)
                {
                    pickup.PreviousOwner = previousOwner;
                }

                base.TrackedSerials.Add(pickup.Serial);
                return pickup;
            }

            return base.Spawn(position, item, previousOwner);
        }

        public override void Give(Exiled.API.Features.Player player, bool displayMessage = true)
        {
            Exiled.API.Features.Items.Item item = player.AddItem(Type);
            if (item is Firearm firearm)
            {

                if (ClipSize > 0)
                {
                    firearm.MagazineAmmo = ClipSize;
                }
            }

            Log.Debug(string.Format("{0}: Adding {1} to tracker.", "Give", item.Serial));
            base.TrackedSerials.Add(item.Serial);
            OnAcquired(player, item, displayMessage);
        }


        protected override void ShowPickedUpMessage(Player player)
        {
            Message(this, player, true);
        }

        protected override void ShowSelectedMessage(Player player)
        {
            Message(this, player);
        }

    }
}
