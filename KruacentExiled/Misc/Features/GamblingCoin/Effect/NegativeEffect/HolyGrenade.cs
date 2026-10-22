using Exiled.API.Features;
using KruacentExiled.CustomItems.API.Features;
using Exiled.API.Features.Pickups;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using UnityEngine;
using EffectType = KruacentExiled.Misc.Features.GamblingCoin.Types.EffectType;

internal class HolyGrenade : ICoinEffect
{
    public string Name { get; set; } = "HolyGrenade";
    public string Message { get; set; } = "Watch a holy grenade !";
    public int Weight { get; set; } = 15;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        KECustomGrenade.TrySpawn("SainteGrenada", player.Position + Vector3.up, out Pickup pickup);
        GrenadePickup grenadePickup = (GrenadePickup)pickup;
        grenadePickup.Explode(player.Footprint);
    }
}