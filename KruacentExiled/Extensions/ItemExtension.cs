using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Features;

namespace KruacentExiled.Extensions
{
    public static class ItemExtensions
    {
        #region GetNames
        /// <summary>
        /// Gets the identifier of an <see cref="Item"/>.
        /// Returns the KECustomItem name if registered, otherwise returns item type name.
        /// </summary>
        /// <param name="item">The targeted item</param>
        /// <returns>The KECustomItem name or vanilla item type string, or an empty string if null</returns>
        public static string GetName(this Item item)
        {
            if (item == null)
                return string.Empty;

            if(CustomItem.TryGet(item, out CustomItem customItem))
            {
                return customItem.Name;
            }

            return item.Type.ToString();
        }

        /// <summary>
        /// Gets the identifier of an <see cref="Pickup"/>.
        /// Returns the KECustomItem name if registered, otherwise returns pickup type name.
        /// </summary>
        /// <param name="pickup">The targeted pickup</param>
        /// <returns>The KECustomItem name or vanilla pickup type string, or an empty string if null</returns>
        public static string GetName(this Pickup pickup)
        {
            if (pickup == null)
                return string.Empty;

            if (CustomItem.TryGet(pickup, out CustomItem customItem))
            {
                return customItem.Name;
            }

            return pickup.Type.ToString();
        }

        #endregion

        #region ViolentItems

        /// <summary>
        /// Checks if a <see cref="Pickup"/> is considered violent.
        /// </summary>
        public static bool IsViolent(this Pickup pickup)
        {
            if (pickup == null) return false;

            if (CustomItem.TryGet(pickup, out CustomItem ci) && ci is KECustomItem kecr)
            {
                return KECustomItem.IsConsideredViolent(kecr);
            }

            return IsViolent(pickup.Type);
        }

        /// <summary>
        /// Checks if an <see cref="Item"/> is considered violent.
        /// </summary>
        public static bool IsViolent(this Item item)
        {
            if (item == null) return false;

            if (CustomItem.TryGet(item, out CustomItem ci) && ci is KECustomItem kecr)
            {
                return KECustomItem.IsConsideredViolent(kecr);
            }

            return IsViolent(item.Type);
        }

        /// <summary>
        /// Checks if an <see cref="ItemType"/> is considered violent.
        /// </summary>
        public static bool IsViolent(this ItemType itemType)
        {
            if (itemType.IsWeapon() || itemType.GetCategory() == ItemCategory.Firearm)
                return true;

            ProjectileType projectileType = itemType.GetProjectileType();
            if (projectileType == ProjectileType.FragGrenade || projectileType == ProjectileType.Scp018)
                return true;

            return false;
        }
        #endregion


    }
}