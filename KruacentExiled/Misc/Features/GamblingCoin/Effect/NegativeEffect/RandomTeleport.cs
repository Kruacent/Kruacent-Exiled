using Exiled.API.Enums;
using Exiled.API.Features;
using KruacentExiled.CustomSpawnPoint;
using KruacentExiled.CustomSpawnPoint.Spawned;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using System;
using EffectType = KruacentExiled.Misc.Features.GamblingCoin.Types.EffectType;

public class RandomTeleport : ICoinEffect
{
    public string Name { get; set; } = "RandomTeleport";
    public string Message { get; set; } = "You were randomly teleported";
    public int Weight { get; set; } = 15;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        RoomSpawnedCustomPoint point = RoomCustomPointHandler.GetRandomPoint(RoomCustomPointType.Teleport, Room.Random());
        player.Teleport(point.Position);
    }
}