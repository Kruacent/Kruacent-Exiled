using Exiled.API.Features;
using KE.Utils.API.Features;
using KruacentExiled.CustomItems.Commands;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;
using KruacentExiled.Misc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

internal class RandomizeInventory : ICoinEffect
{
    public string Name { get; set; } = "RandomizeInventory";
    public string Message { get; set; } = "You shouldn't gamble !";
    public int Weight { get; set; } = 10;
    public EffectType Type { get; set; } = EffectType.Positive;

    public void Execute(Player player)
    {
        int numberOfItem = player.Items.Count;
        player.ClearInventory();

        for (int i = 0; i < numberOfItem; i++)
        {
            player.AddItem(GiveRandomItem());
        }
    }

    public static ItemType GiveRandomItem()
    {
        Array values = Enum.GetValues(typeof(ItemType));
        List<ItemType> itemPool = new List<ItemType>();
                
        foreach (ItemType item in values)
        {
            if(!BlacklistItemTypeManager.BlackListItem.Contains(item))
            {
                itemPool.Add(item);
            }
        }

        ItemType randomItem = itemPool.RandomItem();
        KELog.Debug($"Random Item {randomItem}");

        return randomItem;
    }
}