using Exiled.API.Enums;
using Exiled.API.Features;
using KE.Misc.Features.GamblingCoin.Interfaces;
using System;
using EffectType = KE.Misc.Features.GamblingCoin.Types.EffectType;

public class RandomTeleport : ICoinEffect
{
    public string Name { get; set; } = "RandomTeleport";
    public string Message { get; set; } = "You were randomly teleported";
    public int Weight { get; set; } = 15;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        player.Teleport(Room.Random());
    }
}