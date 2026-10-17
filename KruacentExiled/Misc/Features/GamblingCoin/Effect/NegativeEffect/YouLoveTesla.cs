using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using KruacentExiled.CustomSpawnPoint;
using KruacentExiled.CustomSpawnPoint.Spawned;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;
using System.Collections.Generic;
using System.Linq;
using Tesla = Exiled.API.Features.TeslaGate;

internal class YouLoveTesla : ICoinEffect
{
    public string Name { get; set; } = "YouLoveTesla";
    public string Message { get; set; } = "You love Tesla much more than Elon Musk";
    public int Weight { get; set; } = 15;
    public EffectType Type { get; set; } = EffectType.Negative;

    public void Execute(Player player)
    {
        List<Room> rooms = ListPool<Room>.Pool.Get();


        foreach (Tesla tesla in Tesla.List)
        {
            rooms.Add(tesla.Room);
        }



        RoomSpawnedCustomPoint point = RoomCustomPointHandler.GetRandomPoint(RoomCustomPointType.Teleport, rooms.GetRandomValue());

        player.Teleport(point.Position);

        if (Warhead.IsDetonated)
        {
            player.Kill(Exiled.API.Enums.DamageType.Decontamination);
        }
    }
}