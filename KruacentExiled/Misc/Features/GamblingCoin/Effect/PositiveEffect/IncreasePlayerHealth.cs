using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;
using UnityEngine;

internal class IncreasePlayerHealth : ICoinEffect
{
    public string Name { get; set; } = "IncreasePlayerHeal";
    //human i remember you're life expectancy
    public string Message { get; set; } = "You're life expectancy is extended !";
    public int Weight { get; set; } = 10;
    public EffectType Type { get; set; } = EffectType.Positive;

    /// <summary>
    /// % of heal applied to the player.
    /// </summary>
    public int Heal = 30;

    public void Execute(Player player)
    {
        player.Heal(((player.Health*Heal)/100), true);
    }
}