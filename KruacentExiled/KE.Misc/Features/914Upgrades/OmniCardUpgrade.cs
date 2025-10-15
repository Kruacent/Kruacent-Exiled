using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Scp914;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using LabPlayer = LabApi.Features.Wrappers.Player;
using DetailBase = InventorySystem.Items.Keycards.DetailBase;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Keycards;
using System.Xml.Linq;
using KeycardItem = LabApi.Features.Wrappers.KeycardItem;
using InventorySystem;


namespace KE.Misc.Features._914Upgrades
{
    public class OmniCardUpgrade : Base914PlayerUpgrade
    {
        protected override float Chance => 5;

        public static readonly Color32 CardColor = new(45, 44, 249,255);
        protected override void OnUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            LabPlayer player = ev.Player;
            if (player.CurrentItem is null) return;
            if (player.CurrentItem is not KeycardItem keycard) return;

            player.RemoveItem(keycard);

            player.CurrentItem = CreateOmniCard(player);
        }

        public static KeycardItem CreateOmniCard(LabPlayer player)
        {
            return KeycardItem.CreateCustomKeycardSite02(player, "Omni-Card", string.Empty, "Omni-Card",
                new KeycardLevels(3, 3, 3),
                CardColor, CardColor, Color.white, 1);
        }

    }
}
