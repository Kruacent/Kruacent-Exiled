using Exiled.API.Features;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using EffectType = KruacentExiled.Misc.Features.GamblingCoin.Types.EffectType;

namespace KruacentExiled.Misc.Features.GamblingCoin.Effect.NegativeEffect
{
    internal class TurnOffLight : ICoinEffect
    {
        public string Name { get; set; } = "TurnOffLight";
        public string Message { get; set; } = string.Empty;
        public int Weight { get; set; } = 10;
        public EffectType Type { get; set; } = EffectType.Negative;

        public static int BlackoutTime = 10;

        public void Execute(Player player)
        {
            Exiled.API.Features.Map.TurnOffAllLights(BlackoutTime);
        }
    }
}
