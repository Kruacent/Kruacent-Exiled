using Exiled.API.Features;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;
using MEC;

namespace KruacentExiled.Misc.Features.GamblingCoin.Effect.NegativeEffect
{
    internal class Vaporize : ICoinEffect
    {
        public string Name { get; set; } = "Vaporize";
        public string Message { get; set; } = "I think eating moss in your cell is not good for you";
        public int Weight { get; set; } = 5;
        public EffectType Type { get; set; } = EffectType.Negative;

        public const int MaxSeconds = 600;

        public void Execute(Player player)
        {
            int id = player.Role.Base.UniqueLifeIdentifier;
            CoroutineHandle handle = Timing.CallDelayed(UnityEngine.Random.Range(30, MaxSeconds), () =>
            {
                if (id == player.Role.Base.UniqueLifeIdentifier)
                {
                    player.Vaporize();
                    KruacentExiled.CustomRoles.MainPlugin.ShowEffectHint(player, "You shouldn't gamble too much (you ate moss)");
                }
            });
        }
    }
}
