using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.Items.API.Features;
using KE.Items.API.Interface;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PlayerHandle = Exiled.Events.Handlers.Player;

/// <inheritdoc />
public class TrueDivinePills : KECustomItem, ILumosItem
{
    protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
    {
        return new Dictionary<string, Dictionary<string, string>>()
        {
            ["en"] = new Dictionary<string, string>()
            {
                [TranslationKeyName] = "True Divine Pills",
                [TranslationKeyDesc] = "Guaranteed to respawn everybody, drop to change the mode",
            },
            ["fr"] = new Dictionary<string, string>()
            {
                [TranslationKeyName] = "True Divine Pills",
                [TranslationKeyDesc] = "Fait réappaître tout le monde, lâcher pour changer le mode",
            },
        };
    }

    public override ItemType ItemType => ItemType.SCP500;

    /// <inheritdoc/>
    public override string Name { get; set; } = "TrueDivinePills";

    /// <inheritdoc/>

    /// <inheritdoc/>
    public override float Weight { get; set; } = 0.65f;
    public Color Color { get; set; } = Color.yellow;
    private bool tp = false;

    /// <inheritdoc/>
    public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
    {
        
    };

    /// <inheritdoc/>
    protected override void SubscribeEvents()
    {
        PlayerHandle.UsingItem += OnUsingItem;
        base.SubscribeEvents();
    }

    /// <inheritdoc/>
    protected override void UnsubscribeEvents()
    {
        PlayerHandle.UsingItem -= OnUsingItem;
        base.UnsubscribeEvents();
    }

    protected override void OnDroppingItem(DroppingItemEventArgs ev)
    {
        if (!Check(ev.Item))
            return;

        if (ev.IsThrown)
        {
            ev.IsAllowed = true;
            return;
        }
        Player player = ev.Player;

        tp = !tp;
        if (tp)
            KECustomItem.ItemEffectHint(player, "Players will spawn to you");
        else
            KECustomItem.ItemEffectHint(player, "Players won't spawn to you");
        ev.IsAllowed = false;
        
        

    }

    private void OnUsingItem(UsingItemEventArgs ev)
    {
        if (!Check(ev.Item))
            return;
        Player player = ev.Player;
        Log.Debug(Player.List.Count);
        Log.Debug(Player.List.Where(x => x.Role == RoleTypeId.Spectator).Count());

        if (Player.List.Where(x => x.Role == RoleTypeId.Spectator).Count() == 0)
        {
            KECustomItem.ItemEffectHint(player, "No one to respawn");
            ev.IsAllowed = false;
            return;
        }


        Player.List.Where(x => x.Role == RoleTypeId.Spectator).ToList().ForEach(x =>
        {
            switch (player.Role.Side)
            {
                case Side.ChaosInsurgency:
                    x.Role.Set(RoleTypeId.ChaosRifleman);
                    break;
                case Side.Mtf:
                    x.Role.Set(RoleTypeId.NtfPrivate);
                    break;
            }
            if (tp)
            {
                x.Teleport(player);
            }
        });
        
    }

}