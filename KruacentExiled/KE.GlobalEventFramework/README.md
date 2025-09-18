# How to Create a Global Event

This guide explains how to create a custom Global Event using the KE.GlobalEventFramework. A Global Event is a feature that introduces unique gameplay mechanics or effects during a round in SCP: Secret Laboratory.

## Prerequisites
1. **Knowledge of C#**: Basic understanding of C#.
2. **Exiled Framework**: Knowledge about the Exiled API is recommended.

---

## Step 1: Create a Class for the Global Event
Each Global Event must inherit from the `GlobalEvent` base class provided by the framework.

### Example:
```csharp
using KE.GlobalEventFramework.GEFE.API.Features;

namespace MyPlugin.GlobalEvents
{
    public class ExampleEvent : GlobalEvent
    {
        public override uint Id { get; set; } = 1234;
        public override string Name { get; set; } = "ExampleEvent";
        public override string Description { get; set; } = "This is a sample global event.";
        public override int Weight { get; set; } = 1;
    }
}
```

- Id: A unique identifier for your event.
- Name: The name of your event.
- Description: A short description of the event. (displayed to player when the round start)
- Weight: The likelihood of this event being chosen compared to others.

## Step 2: Implement Interfaces for Functionality
Global Events can implement additional interfaces to define behavior :
- IStart: Defines behavior when the event starts.
- IEvent: Handles event subscriptions.

### Example with IStart:
```csharp
using System.Collections.Generic;
using MEC;

public class ExampleEvent : GlobalEvent, IStart
{
    public IEnumerator<float> Start()
    {
        // Code to execute when the event starts
        yield return Timing.WaitForSeconds(1);
        Log.Info("ExampleEvent has started!");
    }
}
```

### Example with IEvent:
```csharp
using Exiled.Events.EventArgs.Player;

public class ExampleEvent : GlobalEvent, IEvent
{
    public void SubscribeEvent()
    {
        Exiled.Events.Handlers.Player.Hurting += OnPlayerHurt;
    }

    public void UnsubscribeEvent()
    {
        Exiled.Events.Handlers.Player.Hurting -= OnPlayerHurt;
    }

    private void OnPlayerHurt(HurtingEventArgs ev)
    {
        // Example effect: reduce all damage by half
        ev.Amount /= 2;
    }
}
```

## Step 3: Define Custom Behavior
You can add custom properties and methods to define the unique behavior of your Global Event.

### Example: Opening All Doors
```csharp
using Exiled.API.Features.Doors;
using System.Linq;

private void OpenAllDoors()
{
    Door.List.ToList().ForEach(door =>
    {
        door.IsOpen = true;
    });
}
```
You can call this method in your Start implementation or in your subscribed events.

## Step 4: Handle Cleanup
It is essential to clean up your event's effects when it ends. This can be done in the UnsubscribeEvent method.

### Exemple
```csharp
public void UnsubscribeEvent()
{
    Player.List.ToList().ForEach(player => player.DisableEffect<MovementBoost>());
}
```

## Step 5: Test Your Event
- Compile your plugin and place the .dll file in the Plugins folder in EXILED folder.
- Configure your event to ensure it's loaded by the framework.
- Test the event in a controlled environment to verify its behavior.

## Additional information
- Use IncompatibleGE to prevent certain events from running simultaneously.
```csharp
public override uint[] IncompatibleGE { get; set; } = { 101, 102 };
```

- Leverage Exiled's logging system (Log.Info, Log.Error) for debugging.
- Refer to the [Exiled Repository](https://github.com/ExMod-Team/EXILED/tree/master/EXILED) for more information on available APIs.
- [Example of Global Event](https://github.com/Kruacent/Kruacent-Exiled/tree/GlobalEvent/KruacentExiled/KE.GlobalEventFramework.Examples/GE)
