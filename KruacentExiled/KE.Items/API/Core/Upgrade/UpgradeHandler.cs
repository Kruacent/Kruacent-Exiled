using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Scp914;
using KE.Items.API.Interface;
using KE.Utils.API.Interfaces;
using Scp914;

namespace KE.Items.API.Core.Upgrade
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
            if (UpgradeCheck(upgradable, ev.KnobSetting))
            {
                Log.Debug("success");
                var newItemid = upgradable.Upgrade[ev.KnobSetting].UpgradedItem;

                CustomItem newItem = CustomItem.Get(newItemid);

                ev.Player.RemoveItem(ev.Item);
                newItem?.Give(ev.Player);
                if (newItem == null) Log.Warn("warning id of custom item not found");

            }
            ev.IsAllowed = false;
        }

        private void UpgradePickUp(UpgradingPickupEventArgs ev)
        {

            if (!CustomItem.TryGet(ev.Pickup, out CustomItem ci)) return;
            if (!(ci is IUpgradableCustomItem upgradable)) return;
            Log.Debug("upgrading pickup");

            if (UpgradeCheck(upgradable, ev.KnobSetting))
            {
                Log.Debug("success");
                var newItemid = upgradable.Upgrade[ev.KnobSetting].UpgradedItem;

                CustomItem newItem = CustomItem.Get(newItemid);

                ev.Pickup.Destroy();
                newItem.Spawn(ev.OutputPosition);
                if (newItem == null) Log.Warn("warning id of custom item not found");
            }

            ev.IsAllowed = false;
        }

        private bool UpgradeCheck(IUpgradableCustomItem upgradable, Scp914KnobSetting knob)
        {
            if (!upgradable.Upgrade.TryGetValue(knob, out UpgradeProperties item)) return false;
            if (MainPlugin.Instance.Config.Debug)
            {
                Log.Warn("debug activated!");
                return true;
            }
            float random = UnityEngine.Random.Range(0f, 100f);
            return random < item.Chance;
        }

    }
}
