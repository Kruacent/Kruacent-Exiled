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
[CustomItem(ItemType.SCP1853)]

public class Defibrilator : CustomItem
{
    public override uint Id { get; set; } = 20;
    public override string Name { get; set; } = "DF-001";
    public override string Description { get; set; } = "Voici un défibrillateur, utilisez-le sur une personne que vous voulez réanimer. Utilisable uniquement pendant les 10 premières secondes après la perte de conscience de la personne. Utilisez-le lorsque vous êtes dans la même pièce que la personne.";
    public override float Weight { get; set; } = 0.65f;

    private ConcurrentDictionary<Player, Vector3> positionMort = new ConcurrentDictionary<Player, Vector3>();

    public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
    {
        Limit = 2,
        DynamicSpawnPoints = new List<DynamicSpawnPoint>
        {
            new DynamicSpawnPoint() { Chance = 100, Location = SpawnLocationType.Inside079Secondary },
            new DynamicSpawnPoint() { Chance = 10, Location = SpawnLocationType.InsideHidRight },
            new DynamicSpawnPoint() { Chance = 10, Location = SpawnLocationType.InsideHidLeft },
        },
    };

    protected override void SubscribeEvents()
    {
        Exiled.Events.Handlers.Player.UsedItem += OnUsingItem;
        Exiled.Events.Handlers.Player.Dying += OnDeathEvent;
        Exiled.Events.Handlers.Player.Spawned += OnSpawningEvent;
        base.SubscribeEvents();
    }

    protected override void UnsubscribeEvents()
    {
        Exiled.Events.Handlers.Player.UsedItem -= OnUsingItem;
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

    private void OnUsingItem(UsedItemEventArgs ev)
    {
        if (TryGet(ev.Item, out var result) && result.Id == 20)
        {
            Timing.CallDelayed(0.5f, () =>
            {
                ev.Player.DisableEffect(EffectType.Scp1853);
                Timing.RunCoroutine(EffectAttribution(ev.Player));
            });
        }
    }

    private IEnumerator<float> EffectAttribution(Player joueur)
    {
        Log.Debug("Utilisation item");
        Log.Debug("Nombre de mort : " + positionMort.Count());

        if (positionMort.Count == 0)
        {
            joueur.Broadcast(5, "Il n'y a pas de morts actuellement.", Broadcast.BroadcastFlags.Normal, true);
            Exiled.CustomItems.API.Features.CustomItem.TryGive(joueur, 20);
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
                closestDeadPlayer.Health = 10;

                closestDeadPlayer.Teleport(joueur.Position);

                closestDeadPlayer.Broadcast(5, joueur.Nickname + " t'as réanimé !", Broadcast.BroadcastFlags.Normal, true);
                joueur.Broadcast(5, "Tu as réanimé " + closestDeadPlayer.Nickname + " !", Broadcast.BroadcastFlags.Normal, true);

                yield return Timing.WaitForSeconds(1);

                closestDeadPlayer.IsGodModeEnabled = false;
            }
        }
    }
}