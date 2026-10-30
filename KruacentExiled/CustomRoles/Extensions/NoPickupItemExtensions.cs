using Exiled.API.Extensions;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using KruacentExiled.CustomRoles.API.Interfaces;
using KruacentExiled.Extensions;

namespace KruacentExiled.CustomRoles.Extensions
{
    public static class NoPickupItemExtensions
    {
        /// <summary>
        /// Evaluates whether a <see cref="Pickup"/> is banned for a given role implementing <see cref="INoPickupItem"/>.
        /// </summary>
        public static bool IsItemBanned(this INoPickupItem role, Pickup pickup)
        {
            if (role == null || pickup == null) return false;

            string itemName = pickup.GetName();

            if (role.WhitelistItem != null && role.WhitelistItem.Contains(itemName))
                return false;

            if (role.BannedItem != null && role.BannedItem.Contains(itemName))
                return true;

            if (role.BlockScpItems && pickup.Type.IsScp())
                return true;

            if (role.BlockViolentItems && pickup.IsViolent())
                return true;

            if (role.BlockRevivingItems && CustomItem.TryGet(pickup, out CustomItem ci) && ci is IRevivingCustomItem)
                return true;

            return false;
        }

        /// <summary>
        /// Evaluates whether an <see cref="Item"/> is banned for a given role implementing <see cref="INoPickupItem"/>.
        /// </summary>
        public static bool IsItemBanned(this INoPickupItem role, Item item)
        {
            if (role == null || item == null) return false;

            string itemName = item.GetName();

            if (role.WhitelistItem != null && role.WhitelistItem.Contains(itemName))
                return false;

            if (role.BannedItem != null && role.BannedItem.Contains(itemName))
                return true;

            if (role.BlockScpItems && item.Type.IsScp())
                return true;

            if (role.BlockViolentItems && item.IsViolent())
                return true;

            if (role.BlockRevivingItems && CustomItem.TryGet(item, out CustomItem ci) && ci is IRevivingCustomItem)
                return true;

            return false;
        }
    }
}
