using System.Collections.Concurrent;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using MEC;
using Exiled.Events.EventArgs.Player;
using Exiled.API.Features;
using UnityEngine;
using System.Linq;
using KE.Items.Interface;

[CustomItem(ItemType.SCP1853)]
public class Defibrilator : CustomItem, ILumosItem
{
    public override uint Id { get; set; } = 1041;
    public override string Name { get; set; } = "Defibrilator";
    public override string Description { get; set; } = "The defibrillator is used to revive a person who has lost consciousness. It will revive the person closest to the player who uses it (the location of death, not where the body is).";
    public override float Weight { get; set; } = 0.65f;
    public UnityEngine.Color Color { get; set; } = UnityEngine.Color.magenta;

    private ConcurrentDictionary<Player, Vector3> positionMort = new ConcurrentDictionary<Player, Vector3>();

    public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
    {
        Limit = 4,
        DynamicSpawnPoints = new List<DynamicSpawnPoint>
        {
            new DynamicSpawnPoint() { Chance = 50, Location = SpawnLocationType.Inside079Secondary },
            new DynamicSpawnPoint() { Chance = 20, Location = SpawnLocationType.InsideHidLower },
            new DynamicSpawnPoint() { Chance = 20, Location = SpawnLocationType.InsideHidUpper },
        },

        LockerSpawnPoints = new List<LockerSpawnPoint>
        {
             new LockerSpawnPoint(){ Chance= 50, Type = LockerType.Medkit, },
        }
    };

    protected override void SubscribeEvents()
    {
        Exiled.Events.Handlers.Player.UsingItem += OnUsingItem;
        Exiled.Events.Handlers.Player.Dying += OnDeathEvent;
        Exiled.Events.Handlers.Player.Spawned += OnSpawningEvent;
        base.SubscribeEvents();
    }

    protected override void UnsubscribeEvents()
    {
        Exiled.Events.Handlers.Player.UsingItem -= OnUsingItem;
        Exiled.Events.Handlers.Player.Dying -= OnDeathEvent;
        Exiled.Events.Handlers.Player.Spawned -= OnSpawningEvent;
        base.UnsubscribeEvents();
    }

    private void OnDeathEvent(DyingEventArgs ev)
    {
        Log.Debug(positionMort.Count());
        Log.Debug(ev.Player.Nickname);
        positionMort.TryAdd(ev.Player, ev.Player.Position);
        Log.Debug(positionMort.Count());
        Log.Debug("Role : " + ev.Player.Role);
    }

    private void OnSpawningEvent(SpawnedEventArgs ev)
    {
        if (ev.Player.IsAlive)
        {
            Log.Debug("Enlèvement du joueur");
            positionMort.TryRemove(ev.Player, out _);
        }
    }

    private void OnUsingItem(UsingItemEventArgs ev)
    {
        if (!Check(ev.Player.CurrentItem))
        {
            return;
        }

        Timing.CallDelayed(1f, () =>
        {
            ev.IsAllowed = false;
            ev.Player.RemoveItem(ev.Item);

            Timing.RunCoroutine(EffectAttribution(ev.Player));
        });
    }

    private IEnumerator<float> EffectAttribution(Player joueur)
    {
        joueur.DisableEffect(EffectType.Scp1853);
        Log.Debug("Utilisation item");
        Log.Debug("Nombre de mort : " + positionMort.Count());

        if (positionMort.Count == 0)
        {
            joueur.Broadcast(5, "There is no death", Broadcast.BroadcastFlags.Normal, true);
            Exiled.CustomItems.API.Features.CustomItem.TryGive(joueur, 1041);
        }
        else
        {
            var playerPosition = joueur.Position;

            Exiled.API.Features.Player closestDeadPlayer = null;
            float shortestDistance = float.MaxValue;

            foreach (var dead in positionMort)
            {
                float distance = Vector3.Distance(playerPosition, dead.Value);


                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestDeadPlayer = dead.Key;
                }
            }

            if (closestDeadPlayer != null)
            {
                Log.Debug($"Le joueur mort le plus proche est à une distance de {shortestDistance:F2} unités. C'est : " + closestDeadPlayer.Nickname);

                closestDeadPlayer.IsGodModeEnabled = true;
                closestDeadPlayer.Role.Set(joueur.Role);
                closestDeadPlayer.Health = 40;

                closestDeadPlayer.Teleport(joueur.Position);

                closestDeadPlayer.Broadcast(5, joueur.Nickname + " revived you!", Broadcast.BroadcastFlags.Normal, true);
                joueur.Broadcast(5, "You revived " + closestDeadPlayer.Nickname + "!", Broadcast.BroadcastFlags.Normal, true);

                yield return Timing.WaitForSeconds(1);

                closestDeadPlayer.IsGodModeEnabled = false;
            }
        }
    }
}