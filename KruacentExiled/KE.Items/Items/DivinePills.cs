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
using KE.Items.ItemEffects;
using KE.Items.Upgrade;
using Scp914;
using System.Collections.ObjectModel;
using KE.Items;

/// <inheritdoc />
[CustomItem(ItemType.Painkillers)]
public class DivinePills : KECustomItem, ILumosItem, ISwichableEffect, IUpgradableCustomItem
{
    /// <inheritdoc/>
    public override uint Id { get; set; } = 1047;

    /// <inheritdoc/>
    public override string Name { get; set; } = "Divine Pills";

    /// <inheritdoc/>
    public override string Description { get; set; } = "25% chance you die\n 75% you respawn someone\n 10% to upgrade in 914 on very fine";

    /// <inheritdoc/>
    public override float Weight { get; set; } = 0.65f;
    public UnityEngine.Color Color { get; set; } = UnityEngine.Color.yellow;
    public IReadOnlyDictionary<Scp914KnobSetting, UpgradeProperties> Upgrade { get; private set; } = new Dictionary<Scp914KnobSetting, UpgradeProperties>()
        {
            //very fine -> true divine pills 10%
            { Scp914KnobSetting.VeryFine,new UpgradeProperties(10, 1050)}
        };

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

    public CustomItemEffect Effect { get;set; }
    public DivinePills()
    {
        Effect = new DivinePillsEffect();
    }

    /// <inheritdoc/>
    protected override void SubscribeEvents()
    {
        PlayerHandle.UsedItem += OnUsedItem;
        base.SubscribeEvents();
    }

    /// <inheritdoc/>
    protected override void UnsubscribeEvents()
    {
        PlayerHandle.UsedItem -= OnUsedItem;
        base.UnsubscribeEvents();
    }

    private void OnUsedItem(UsedItemEventArgs ev)
    {
        if (!Check(ev.Item)) return;
        Effect.Effect(ev);
    }

}