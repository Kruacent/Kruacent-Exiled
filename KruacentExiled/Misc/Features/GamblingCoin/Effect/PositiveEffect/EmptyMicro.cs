using Exiled.API.Features;
using Exiled.API.Features.Items;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;
using UnityEngine;

internal class EmptyMicro : ICoinEffect
{
    public string Name { get; set; } = "EmptyMicro";
    public string Message { get; set; } = "DID YOU JUST GET A MICRO HID !?";
    public int Weight { get; set; } = 10;
    public EffectType Type { get; set; } = EffectType.Positive;

    public void Execute(Player player)
    {
        MicroHid item = (MicroHid)Item.Create(ItemType.MicroHID, player);
        item.Energy = 0;
        item.CreatePickup(player.Position);
    }
}