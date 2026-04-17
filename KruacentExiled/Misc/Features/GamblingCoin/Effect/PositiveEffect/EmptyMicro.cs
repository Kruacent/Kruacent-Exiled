using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;

internal class EmptyMicro : ICoinEffect
{
    public string Name { get; set; } = "EmptyMicro";
    public string Message { get; set; } = "DID YOU JUST GET A MICRO HID !?";
    public int Weight { get; set; } = 5;
    public EffectType Type { get; set; } = EffectType.Positive;

    public void Execute(Player player)
    {
        MicroHIDPickup item = (MicroHIDPickup)Pickup.Create(ItemType.MicroHID);
        item.Position = player.Position;
        item.Energy = 2;
        item.Spawn();
    }
}