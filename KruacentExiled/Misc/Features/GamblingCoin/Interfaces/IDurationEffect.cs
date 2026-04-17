using Exiled.API.Features;

namespace KruacentExiled.Misc.Features.GamblingCoin.Interfaces
{
    /// <summary>
    /// Represents an effect that can be applied via gambling coin.
    /// </summary>
    public interface IDurationEffect : ICoinEffect
    {
        /// <summary>
        /// Duration of the effect in seconds (-1 for infinite, 0 if not used).
        /// </summary>
        float Duration { get; set; }

        /// <summary>
        /// Executes the effets on the given player after the duration time.
        /// </summary>
        /// <param name="player"></param>
        void ExecuteAfterDuration(Player player);
    }
}