using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Scp914;
using KE.Utils.API.Features;
using KE.Utils.API.Interfaces;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using KruacentExiled.Misc.Utils;
using Scp914;
using UnityEngine;

namespace KruacentExiled.CustomItems.API.Core.Upgrade
{
    internal class UpgradeHandler : IUsingEvents
    {

        public void SubscribeEvents()
        {
            
            Exiled.Events.Handlers.Scp914.UpgradingInventoryItem += UpgradeItem;
            Exiled.Events.Handlers.Scp914.UpgradingPickup += UpgradePickUp;
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Scp914.UpgradingInventoryItem -= UpgradeItem;
            Exiled.Events.Handlers.Scp914.UpgradingPickup -= UpgradePickUp;
        }


        private void UpgradeItem(UpgradingInventoryItemEventArgs ev)
        {
            if (!CustomItem.TryGet(ev.Item, out CustomItem ci)) return;
            if (!(ci is IUpgradableCustomItem upgradable)) return;
            Log.Debug("upgrading item");
            if (UpgradeCheck(upgradable, ev.KnobSetting, ev.Player))
            {
                Log.Debug("success");
                var newItemid = upgradable.Upgrade[ev.KnobSetting].UpgradedItem;

                CustomItem newItem = CustomItem.Get(newItemid);

                ev.Player.RemoveItem(ev.Item);
                newItem?.Give(ev.Player);
                if (newItem == null) Log.Warn("warning id of custom item not found");

            } else if (DestroyCheck(upgradable, ev.KnobSetting, ev.Player))
            {
                KELog.Debug("destroyed");
                ev.Player.RemoveItem(ev.Item);
            }
            ev.IsAllowed = false;
        }

        private void UpgradePickUp(UpgradingPickupEventArgs ev)
        {
            Pickup pickup = ev.Pickup;
            if (!CustomItem.TryGet(pickup, out CustomItem ci)) return;
            if (!(ci is IUpgradableCustomItem upgradable)) return;
            Log.Debug("upgrading pickup");

            if(upgradable.Upgrade is null || upgradable.Upgrade.Count == 0)
            {
                throw new System.ArgumentException("upgradable null or empty");
            }
            

            if (UpgradeCheck(upgradable, ev.KnobSetting, pickup.PreviousOwner))
            {
                Log.Debug("success");
                string newItemName = upgradable.Upgrade[ev.KnobSetting].UpgradedItem;

                KECustomItem newItem = KECustomItem.Get(newItemName);
                Quaternion rotation = pickup.Rotation;

                pickup.Destroy();
                if (newItem == null)
                {
                    Log.Warn("warning id of custom item not found");
                    return;
                }
                
                Pickup newPickup = newItem.Spawn(ev.OutputPosition);
                newPickup.Rotation = rotation;
            } else if (DestroyCheck(upgradable, ev.KnobSetting, pickup.PreviousOwner))
            {
                KELog.Debug("destroyed");
                pickup.Destroy();
            }


            ev.IsAllowed = false;
        }

        private bool UpgradeCheck(IUpgradableCustomItem upgradable, Scp914KnobSetting knob, Player player)
        {
            if (!upgradable.Upgrade.TryGetValue(knob, out UpgradeProperties item)) return false;
            if (MainPlugin.Instance.Config.Debug)
            {
                Log.Warn("debug activated!");
                return true;
            }

            bool willUpgrade = RandomNumberGenerator.RollChance(item.Chance, player, true);
            return willUpgrade;
        }

        private bool DestroyCheck(IUpgradableCustomItem upgradable, Scp914KnobSetting knob, Player player)
        {
            if (!upgradable.Upgrade.TryGetValue(knob, out UpgradeProperties item)) return false;
            if (item.DestroyChance <= 0) return false;

            float remain = 100f - item.Chance;
            if (remain <= 0) return false;
            float value = (item.Chance * 100) / remain;

            KELog.Debug($"Item destroy chance for 914 is {item.DestroyChance}");
            bool willDestroy = RandomNumberGenerator.RollChance(value, player, false);
            return willDestroy;
        }
    }
}
