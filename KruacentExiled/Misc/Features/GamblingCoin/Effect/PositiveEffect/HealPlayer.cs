using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;
using UnityEngine;

internal class HealPlayer : ICoinEffect
{
    public string Name { get; set; } = "HealPlayer";
    public string Message { get; set; } = "You got magically healed !";
    public int Weight { get; set; } = 10;
    public EffectType Type { get; set; } = EffectType.Positive;

    public static int Heal = 25;

    public void Execute(Player player)
    {
        player.Heal(Heal, true);
    }
}