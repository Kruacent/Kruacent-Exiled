using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using KE.Misc.Features.GamblingCoin.Interfaces;
using KE.Misc.Features.GamblingCoin.Types;
using UnityEngine;

internal class Lightbulb : ICoinEffect
{
    public string Name { get; set; } = "Lightbulb";
    public string Message { get; set; } = "Follow the light !!";
    public int Weight { get; set; } = 15;
    public EffectType Type { get; set; } = EffectType.Positive;

    public void Execute(Player player)
    {
        Pickup.CreateAndSpawn(ItemType.SCP2176, player.Position, new Quaternion());
    }
}