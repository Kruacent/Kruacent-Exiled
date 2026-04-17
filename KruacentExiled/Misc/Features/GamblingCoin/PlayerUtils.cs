using Exiled.API.Features;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;

namespace KruacentExiled.Misc.Features.GamblingCoin
{
    internal static class PlayerUtils
    {

        private static HintPosition Position = new CoinHintPosition();
        public static void SendBroadcast(Player player, string message)
        {
            DisplayHandler.Instance.AddHint(Position.HintPlacement, player, message,5);
            
        }
    }
}
