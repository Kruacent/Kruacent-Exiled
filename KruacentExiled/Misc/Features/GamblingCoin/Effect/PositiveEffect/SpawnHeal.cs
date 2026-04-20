using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;
using UnityEngine;

internal class SpawnHeal : ICoinEffect
{
    public string Name { get; set; } = "SpawnHeal";
    public string Message { get; set; } = string.Empty;
    public int Weight { get; set; } = 25;
    public EffectType Type { get; set; } = EffectType.Positive;

    public void Execute(Player player)
    {
        Pickup.CreateAndSpawn(ItemType.Medkit, player.Position, new Quaternion());
        Pickup.CreateAndSpawn(ItemType.Painkillers, player.Position, new Quaternion());
    }
}