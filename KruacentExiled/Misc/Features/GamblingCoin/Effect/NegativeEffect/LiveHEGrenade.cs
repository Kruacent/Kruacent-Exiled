using Exiled.API.Features;
using Exiled.API.Features.Items;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using UnityEngine;
using EffectType = KruacentExiled.Misc.Features.GamblingCoin.Types.EffectType;

internal class LiveHEGrenade : ICoinEffect
{
    public string Name { get; set; } = "LiveHEGrenade";
    public string Message { get; set; } = "Watch your head !";
    public int Weight { get; set; } = 30;
    public EffectType Type { get; set; } = EffectType.Negative;
    public static float FuseTime = 3.25f;

    public void Execute(Player player)
    {
        ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
        grenade.FuseTime = FuseTime;
        grenade.SpawnActive(player.Position + Vector3.up, player);
    }
}