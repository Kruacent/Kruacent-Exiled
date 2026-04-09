using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerHandle = Exiled.Events.Handlers.Player;
using Scp914;
using KE.Items.API.Core.Upgrade;
using KE.Items.API.Interface;
using KE.Items.Items.ItemEffects;
using KE.Items.API.Features;

public class DivinePills : KECustomItem, ILumosItem, ISwitchableEffect, IUpgradableCustomItem
{
    protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
    {
        return new()
        {
            ["en"] = new()
            {
                [TranslationKeyName] = "Divine Pills",
                [TranslationKeyDesc] = "25% chance you die\n 75% you respawn someone\n",
            },
            ["fr"] = new()
            {
                [TranslationKeyName] = "Divine Pills",
                [TranslationKeyDesc] = "25% de chance de mourrir\n 75% de ramener quelqu'un à la vie",
            },
        };
    }
    public override ItemType ItemType => ItemType.Painkillers;

    /// <inheritdoc/>
    public override string Name { get; set; } = "Divine Pills";

    /// <inheritdoc/>
    public override float Weight { get; set; } = 0.65f;
    public UnityEngine.Color Color { get; set; } = UnityEngine.Color.yellow;
    public IReadOnlyDictionary<Scp914KnobSetting, UpgradeProperties> Upgrade { get; private set; } = new Dictionary<Scp914KnobSetting, UpgradeProperties>()
        {
            { Scp914KnobSetting.VeryFine,new UpgradeProperties(10, "TrueDivinePills")}
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
                Chance = 80,
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