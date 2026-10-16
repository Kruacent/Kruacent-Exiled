using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using KE.Utils.API.Features;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;
using System.Linq;

internal class DoorHate : ICoinEffect
{
    public string Name { get; set; } = "DoorHate";
    public string Message { get; set; } = "A door in the map hate you";
    public int Weight { get; set; } = 15;
    public EffectType Type { get; set; } = EffectType.Negative;

    private Door door;
    private Player player;

    public void Execute(Player player)
    {
        this.player = player;
        door = Door.List.Where(d => d.Zone == Exiled.API.Enums.ZoneType.Entrance || d.Zone == Exiled.API.Enums.ZoneType.HeavyContainment).GetRandomValue();
        KELog.Debug(door.Name);

        SubscribeEvents();
    }

    public void SubscribeEvents()
    {
        Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
        Exiled.Events.Handlers.Player.Dying += PlayerDying;
    }

    public void UnsubscribeEvents()
    {
        Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
        Exiled.Events.Handlers.Player.Dying -= PlayerDying;
    }

    public void PlayerDying(Exiled.Events.EventArgs.Player.DyingEventArgs ev)
    {
        if(ev.Player == this.player)
        {
            door = null;
            UnsubscribeEvents();
        } 
    }

    public void OnInteractingDoor(Exiled.Events.EventArgs.Player.InteractingDoorEventArgs ev)
    {
        if (ev.Door == door)
        {
            ev.Player.Explode();
        }
    }
}