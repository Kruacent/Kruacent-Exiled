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
using Exiled.CustomItems.API.EventArgs;
using Exiled.Events.EventArgs.Scp914;
using Exiled.API.Features.Items;
using System.Data;
using Exiled.API.Features.Pickups;

/// <inheritdoc />
[CustomItem(ItemType.Painkillers)]
public class DivinePills : CustomItem, ILumosItem
{
    /// <inheritdoc/>
    public override uint Id { get; set; } = 1407;

    /// <inheritdoc/>
    public override string Name { get; set; } = "Divine Pills";

    /// <inheritdoc/>
    public override string Description { get; set; } = "25% chance you die\n 75% you respawn someone\n 10% to upgrade in 914 on very fine";

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
        PlayerHandle.UsingItem += OnUsingItem;
        Exiled.Events.Handlers.Scp914.UpgradingInventoryItem += OnUpgrading;
        //Exiled.Events.Handlers.Scp914.UpgradingPickup += Up; //break the lights
        base.SubscribeEvents();
    }

    /// <inheritdoc/>
    protected override void UnsubscribeEvents()
    {
        PlayerHandle.UsingItem -= OnUsingItem;
        Exiled.Events.Handlers.Scp914.UpgradingInventoryItem -= OnUpgrading;
        //Exiled.Events.Handlers.Scp914.UpgradingPickup -= Up; //break the lights
        base.UnsubscribeEvents();
    }

    private void Up(UpgradingPickupEventArgs ev)
    {
        if (!Check(ev.Pickup))
            return;
        if (ev.KnobSetting != Scp914.Scp914KnobSetting.VeryFine)
            return;
        var rng = Random.value;
        Log.Debug($"pickup {Name} : {rng}");
        if (rng < .1f)
        {
            //success
            ev.Pickup.Destroy();
            TrySpawn("True Divine Pills",ev.OutputPosition,out Pickup _);
            ev.IsAllowed = true;
        }
        else
            ev.IsAllowed = false;
    }

    private void OnUpgrading(UpgradingInventoryItemEventArgs ev)
    {
        if (!Check(ev.Item))
            return;
        if (ev.KnobSetting != Scp914.Scp914KnobSetting.VeryFine)
            return;
        var rng = Random.value;
        Log.Debug($"inventory {Name} : {rng}");
        if (rng < .1f)
        {
            //success
            ev.Player.RemoveItem(ev.Item);
            TryGive(ev.Player, "True Divine Pills");
            ev.IsAllowed = true;
        }
        else
        {
            ev.Player.ShowHint("no luck");
            ev.IsAllowed = false;
        }

    }

    private void OnUsingItem(UsingItemEventArgs ev)
    {
        if (!Check(ev.Item))
        {
            return;
        }
        Player player = ev.Player;
        

        if(Player.List.Where(x => x.Role == RoleTypeId.Spectator).Count() == 0)
        {
            player.ShowHint("No spectators to respawn");
            ev.IsAllowed = false;
            return;
        }
        var random = Random.Range(0, 100);
        if (random <= 25)
        {
            player.Kill("unlucky bro");
            return;
        }
        Player respawning = Player.List.Where(x => x.Role == RoleTypeId.Spectator).GetRandomValue();
        respawning.Role.Set(player.Role);
        if (random > 75)
        {
            respawning.Teleport(player);
        }
    }

}