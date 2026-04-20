using Exiled.API.Features;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;

internal class ReduceHP : ICoinEffect
{
    public string Name { get; set; } = "ReduceHP";
    public string Message { get; set; } = "The coin fall on your head !";
    public int Weight { get; set; } = 20;
    public EffectType Type { get; set; } = EffectType.Negative;

    /// <summary>
    /// Damage dealt to the player
    /// </summary>
    public static float Damage = 30;

    public void Execute(Player player)
    {
        player.Hurt(Damage, "Coin fall too hard");
    }
}