using Exiled.API.Features;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using EffectType = KruacentExiled.Misc.Features.GamblingCoin.Types.EffectType;

namespace KruacentExiled.Misc.Features.GamblingCoin.Effect.NegativeEffect
{
    internal class WarheadCoin : ICoinEffect
    {
        public string Name { get; set; } = "Warhead";
        public string Message { get; set; } = string.Empty;
        public int Weight { get; set; } = 5;
        public EffectType Type { get; set; } = EffectType.Negative;

        public void Execute(Player player)
        {
            if (!Warhead.IsDetonated || !Warhead.IsInProgress)
                Warhead.Start();
        }
    }
}
