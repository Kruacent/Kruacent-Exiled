using Exiled.API.Features;
using KE.Misc.Features.GamblingCoin.Types;

namespace KE.Misc.Features.GamblingCoin.Interfaces
{
    /// <summary>
    /// Represents an effect that can be applied via gambling coin.
    /// </summary>
    public interface ICoinEffect
    {
        /// <summary>
        /// Name of the Coin Effect (must be unique).
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Message displayed to the player that used the coin.
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Weight for random selection. Higher weight = higher chance to be chosen.
        /// </summary>
        int Weight { get; set; }

        /// <summary>
        /// Type of the effect (positive, negative, neutral).
        /// </summary>
        EffectType Type { get; set; }

        /// <summary>
        /// Executes the effect on the given player.
        /// </summary>
        /// <param name="player">Player to apply the effect to.</param>
        void Execute(Player player);
    }
}