using System.Collections.Generic;

namespace KruacentExiled.CustomRoles.API.Interfaces
{
    public interface INoPickupItem
    {
        /// <summary>
        /// Gets or sets list of banned item names
        /// </summary>
        /// <remarks>
        /// Evaluated after the <see cref="WhitelistItem"/> and before global flags.
        /// </remarks>
        public List<string> BannedItem { get; set; }

        /// <summary>
        /// Gets or sets the list of allowed exceptions. 
        /// Any item present in this list bypasses global restriction flags.
        /// </summary>
        /// <remarks>
        /// If an item is in this list, it is always allowed.
        /// </remarks>
        public List<string> WhitelistItem {  get; set; }

        /// <summary>
        /// Gets the translation key used to display a restriction message
        /// to the player when they attempt to pick up a banned item.
        /// </summary>
        public string NoPickupTranslationKey { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the role is prohibited to take any SCP item.
        /// </summary>
        /// <value>
        /// <c>true</c> to block all SCP items except those present in <see cref="WhitelistItem"/>; otherwise, <c>false</c>.
        /// </value>
        public bool BlockScpItems { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the role is prohibited from picking up custom reviving items.
        /// </summary>
        /// <value>
        /// <c>true</c> to block reviving items except those present in <see cref="WhitelistItem"/>; otherwise, <c>false</c>.
        /// </value>
        public bool BlockRevivingItems { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the role is prohibited from picking up violent items.
        /// </summary>
        /// <value>
        /// <c>true</c> to block violent items except those present in <see cref="WhitelistItem"/>; otherwise, <c>false</c>.
        /// </value>
        public bool BlockViolentItems { get; set; }
    }
}
