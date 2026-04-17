using Exiled.API.Features;
using Exiled.API.Features.Items;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;
using System;

internal class RandomItem : ICoinEffect
{
    public string Name { get; set; } = "RandomItem";
    public string Message { get; set; } = "You got a random item !";
    public int Weight { get; set; } = 35;
    public EffectType Type { get; set; } = EffectType.Positive;

    public void Execute(Player player)
    {
        Array values = Enum.GetValues(typeof(ItemType));
        ItemType randomItem = (ItemType)values.GetValue(UnityEngine.Random.Range(0, values.Length));

        Item.Create(randomItem).CreatePickup(player.Position);
    }
}