using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using MEC;
using Exiled.Events.EventArgs.Player;
using PlayerHandle = Exiled.Events.Handlers.Player;
using Exiled.API.Features;
using Exiled.API.Extensions;
using UnityEngine;
using CustomPlayerEffects;
using System.Linq;
using PlayerRoles;
using KE.Items.Interface;

/// <inheritdoc />
[CustomItem(ItemType.Painkillers)]
public class DivinePills : CustomItem, ILumosItem
{
    /// <inheritdoc/>
    public override uint Id { get; set; } = 1406;

    /// <inheritdoc/>
    public override string Name { get; set; } = "Divine Pills";

    /// <inheritdoc/>
    public override string Description { get; set; } = "25% chance you die\n 75% you respawn someone";

    /// <inheritdoc/>
    public override float Weight { get; set; } = 0.65f;
    public UnityEngine.Color Color { get; set; } = UnityEngine.Color.yellow;

    /// <inheritdoc/>
    public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
    {
        LockerSpawnPoints = new List<LockerSpawnPoint>
        {
            new LockerSpawnPoint()
            {
                Chance = 75,
                UseChamber = true,
                Type = LockerType.Misc,
                Zone = ZoneType.Entrance,
            },
            new LockerSpawnPoint()
            {
                Chance = 25,
                UseChamber = true,
                Type = LockerType.Medkit,
                Zone = ZoneType.LightContainment,
            },
        },
        RoomSpawnPoints = new List<RoomSpawnPoint>
        {
            new RoomSpawnPoint()
            {
                Chance = 100,
                Room = RoomType.LczGlassBox,
            },
        },

    };

    /// <inheritdoc/>
    protected override void SubscribeEvents()
    {
        PlayerHandle.UsedItem += OnUsingItem;
        base.SubscribeEvents();
    }

    /// <inheritdoc/>
    protected override void UnsubscribeEvents()
    {
        PlayerHandle.UsedItem -= OnUsingItem;
        base.UnsubscribeEvents();
    }

    private void OnUsingItem(UsedItemEventArgs ev)
    {
        if (!Check(ev.Item))
        {
            return;
        }
        if (TryGet(ev.Item, out var result) && result.Id == Id)
        {
            Player player = ev.Player;
            var random = Random.Range(0, 100);

            if(Player.List.Where(x => x.Role == RoleTypeId.Spectator).Count() == 0)
            {
                player.ShowHint("No spectators to respawn");
                return;
            }

            if (random <= 25)
            {
                player.Kill("unlucky bro");
                return;
            }
            Player respawning = Player.List.Where(x => x.Role == RoleTypeId.Spectator).GetRandomValue();
            respawning.Role.Set(player.Role);
            if (random > 75)
            {
                respawning.Position = player.Position;
            }
        }
    }

}