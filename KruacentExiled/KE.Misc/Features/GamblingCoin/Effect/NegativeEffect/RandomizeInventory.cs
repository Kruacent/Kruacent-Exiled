using Exiled.API.Features;
using KE.Misc.Features.GamblingCoin.Interfaces;
using KE.Misc.Features.GamblingCoin.Types;
using System;

internal class RandomizeInventory : ICoinEffect
{
    public string Name { get; set; } = "RandomizeInventory";
    public string Message { get; set; } = "You shouldn't gamble !";
    public int Weight { get; set; } = 5;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        player.ClearInventory();
        int numberOfItem = UnityEngine.Random.Range(0, 9);

        for (int i = 0; i < numberOfItem; i++)
        {
            player.AddItem(GiveRandomItem());
        }
    }

    public static ItemType GiveRandomItem()
    {
        Array values = Enum.GetValues(typeof(ItemType));
        ItemType randomItem = (ItemType)values.GetValue(UnityEngine.Random.Range(0, values.Length));

        return randomItem;
    }
}