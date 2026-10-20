using Exiled.API.Features;
using Exiled.API.Features.Items;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;
using KruacentExiled.Misc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

internal class RandomItem : ICoinEffect
{
    public string Name { get; set; } = "RandomItem";
    public string Message { get; set; } = "You got a random item !";
    public int Weight { get; set; } = 20;
    public EffectType Type { get; set; } = EffectType.Positive;

    public void Execute(Player player)
    {
        if (player == null) return;
        Array values = Enum.GetValues(typeof(ItemType));
        ItemType randomItem = (ItemType)values.GetValue(UnityEngine.Random.Range(0, values.Length));

        if (BlacklistItemTypeManager.BlackListItem.Contains(randomItem))
        {
            return;
        }

        Item item = Item.Create(randomItem);

        if (item == null) return;

        item.CreatePickup(player.Position);
    }
}