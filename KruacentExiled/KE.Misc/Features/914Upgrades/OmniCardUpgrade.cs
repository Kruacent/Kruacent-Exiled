using Exiled.Events.EventArgs.Scp914;
using UnityEngine;
using LabPlayer = LabApi.Features.Wrappers.Player;
using Interactables.Interobjects.DoorUtils;
using KeycardItem = LabApi.Features.Wrappers.KeycardItem;


namespace KE.Misc.Features._914Upgrades
{
    public class OmniCardUpgrade : Base914PlayerUpgrade
    {
        protected override float Chance => 1;

        public static readonly Color32 CardColor = new(45, 44, 249,255);
        protected override bool OnUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            LabPlayer player = ev.Player;
            if (player.CurrentItem is null) return false;
            if (player.CurrentItem is not KeycardItem keycard) return false;

            player.RemoveItem(keycard);

            player.CurrentItem = CreateOmniCard(player);
            return true;
        }

        public static KeycardItem CreateOmniCard(LabPlayer player)
        {
            return KeycardItem.CreateCustomKeycardSite02(player, "Omni-Card", string.Empty, "Omni-Card",
                new KeycardLevels(3, 3, 3),
                CardColor, CardColor, Color.white, 1);
        }

    }
}
