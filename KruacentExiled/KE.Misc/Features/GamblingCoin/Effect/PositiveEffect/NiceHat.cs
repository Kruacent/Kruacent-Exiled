using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using KE.Misc.Features.GamblingCoin.Interfaces;
using KE.Misc.Features.GamblingCoin.Types;
using UnityEngine;

internal class NiceHat : ICoinEffect
{
    public string Name { get; set; } = "NiceHat";
    public string Message { get; set; } = "You've got a nice hat !";
    public int Weight { get; set; } = 10;
    public EffectType Type { get; set; } = EffectType.Positive;

    public void Execute(Player player)
    {
        Pickup.CreateAndSpawn(ItemType.SCP268, player.Position, Quaternion.identity);
    }
}