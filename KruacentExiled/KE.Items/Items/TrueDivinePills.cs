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

/// <inheritdoc />
[CustomItem(ItemType.SCP500)]
public class TrueDivinePills : CustomItem, ILumosItem
{
    /// <inheritdoc/>
    public override uint Id { get; set; } = 1050;

    /// <inheritdoc/>
    public override string Name { get; set; } = "True Divine Pills";

    /// <inheritdoc/>
    public override string Description { get; set; } = "Guaranteed to respawn everybody";

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

    protected override void OnDropping(DroppingItemEventArgs ev)
    {
        if (!Check(ev.Item))
            return;
        if (ev.IsThrown)
        {
            ev.IsAllowed = true;
            return;
        }

        tp = !tp;
        if (tp)
            ev.Player.ShowHint("Players will spawn to you");
        else
            ev.Player.ShowHint("Players won't spawn to you");
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
            player.ShowHint("No one to respawn");
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