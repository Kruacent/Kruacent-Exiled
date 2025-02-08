using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using MEC;
using Exiled.Events.EventArgs.Player;
using Player = Exiled.Events.Handlers.Player;
using Exiled.API.Features;
using Exiled.API.Extensions;
using UnityEngine;
using CustomPlayerEffects;
using KE.Items.Interface;
using System.Linq;

/// <inheritdoc />
[CustomItem(ItemType.Adrenaline)]
public class AdrenalineDrogue : CustomItem, ILumosItem
{
    /// <inheritdoc/>
    public override uint Id { get; set; } = 1042;

    /// <inheritdoc/>
    public override string Name { get; set; } = "DA-020";

    /// <inheritdoc/>
    public override string Description { get; set; } = "you need to test it !";

    /// <inheritdoc/>
    public override float Weight { get; set; } = 0.65f;
    public UnityEngine.Color Color { get; set; } = UnityEngine.Color.yellow;


    public List<Exiled.API.Features.Player> joueursSCP = new List<Exiled.API.Features.Player>();

    /// <inheritdoc/>
    public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
    {
        Limit = 1,
        DynamicSpawnPoints = new List<DynamicSpawnPoint>
        {
            new DynamicSpawnPoint()
            {
                Chance = 100,
                Location = SpawnLocationType.Inside079Secondary,
            },
            new DynamicSpawnPoint()
            {
                Chance = 2,
                Location = SpawnLocationType.Inside173Gate,
            },
        },

        LockerSpawnPoints = new List<LockerSpawnPoint>
        {
            new LockerSpawnPoint()
            {
                Chance = 20,
                UseChamber = true,
                Type = LockerType.Misc,
                Zone = ZoneType.Entrance,
            },
            new LockerSpawnPoint()
            {
                Chance = 25,
                UseChamber = true,
                Type = LockerType.Medkit,
                Zone = ZoneType.HeavyContainment,
            },
        },
    };

    /// <inheritdoc/>
    protected override void SubscribeEvents()
    {
        Player.UsedItem += OnUsingItem;
        base.SubscribeEvents();
    }

    /// <inheritdoc/>
    protected override void UnsubscribeEvents()
    {
        Player.UsedItem -= OnUsingItem;
        base.UnsubscribeEvents();
    }

    private void OnUsingItem(UsedItemEventArgs ev)
    {
        if (TryGet(ev.Item, out var result))
        {
            if (result.Id == Id)
            {
                Timing.CallDelayed(0.5f, () =>
                {
                    Timing.RunCoroutine(EffectAttribution(ev.Player));
                });
            }

        }
    }

    private IEnumerator<float> EffectAttribution(Exiled.API.Features.Player joueur)
    {
        bool gasgas = false;

        /* EFFET DE LA DROGUE */
        joueur.ShowHint("Vous êtes actuellement sous effet de la cocaïne liquide !");

        var movementBoostEffect = joueur.ActiveEffects.FirstOrDefault(e => e is MovementBoost) as MovementBoost;

        if (movementBoostEffect != null)
        {
            float currentIntensity = movementBoostEffect.Intensity;
            joueur.EnableEffect<MovementBoost>(currentIntensity+50, true);
            gasgas = true;
        }
        else
        {
            joueur.EnableEffect<MovementBoost>(50, true);
        }


        joueur.EnableEffect<InsufficientLighting>(30, true);
        joueur.EnableEffect<BodyshotReduction>(40, true);
        joueur.EnableEffect<Ghostly>(30, true);
        joueur.Health = 169;

        yield return Timing.WaitForSeconds(30);

        joueur.ShowHint("Mince vous êtes perdu chez le papi Rian !");
        joueur.Health = 9420;

        joueur.IsGodModeEnabled = true;
        joueur.EnableEffect(EffectType.Flashed, 2, 2);
        joueur.Teleport(RoomType.Pocket);
        yield return Timing.WaitForSeconds(6);


        joueur.EnableEffect(EffectType.Flashed, 2, 2);
        joueur.Teleport(Room.Random());
        joueur.Handcuff();
        yield return Timing.WaitForSeconds(15);

        joueur.Health = 1;

        joueur.EnableEffect(EffectType.SeveredHands, 4);
        yield return Timing.WaitForSeconds(4);
        joueur.DisableAllEffects();


        foreach (Exiled.API.Features.Player unJoueur in Exiled.API.Features.Player.List)
        {
            if (unJoueur.IsScp)
            {
                joueursSCP.Add(unJoueur);
            }
        }

        joueur.EnableEffect(EffectType.Flashed, 2, 2);
        if (joueursSCP.Count > 0)
        {
            joueur.Teleport(joueursSCP[UnityEngine.Random.Range(0, joueursSCP.Count)]);
        }
        else
        {
            joueur.Teleport(Room.Random());
        }

        yield return Timing.WaitForSeconds(10);

        joueur.Teleport(Room.Random());

        joueur.RemoveHandcuffs();
        joueur.IsGodModeEnabled = false;
        joueur.MaxHealth = 65;
        joueur.Heal(joueur.MaxHealth);
        joueur.DisplayNickname = "Sou Hiyori";

        joueur.DisableAllEffects();
        joueur.EnableEffect<SilentWalk>(10);
        if (gasgas)
        {
            joueur.EnableEffect<MovementBoost>(130, true);
        } else
        {
            joueur.EnableEffect<MovementBoost>(30, true);
        }


        yield return Timing.WaitForSeconds(UnityEngine.Random.Range(60, 120));

        if (joueur.IsAlive)
        {
            int randomNumber = UnityEngine.Random.Range(1, 6);
            switch (randomNumber)
            {
                case 1:
                    Log.Debug(joueur.Nickname + " changed his skin !");
                    joueur.PlayShieldBreakSound();

                    joueur.ChangeAppearance(joueursSCP[0].Role);
                    joueur.DisplayNickname = joueursSCP[0].Nickname;

                    Exiled.API.Features.Server.FriendlyFire = true;

                    joueur.Mute();
                    yield return Timing.WaitForSeconds(15);
                    joueur.UnMute();
                    break;
                case 2:
                    Log.Debug("Muet");
                    joueur.ShowHint("You lost your ability to talk, (git good)");
                    joueur.Mute();
                    yield return Timing.WaitForSeconds(UnityEngine.Random.Range(30, 100));
                    joueur.ShowHint("I think you found your ability");
                    joueur.UnMute();
                    break;
                case 3:
                    joueur.ShowHint("You are caoutchouc man");
                    Exiled.API.Features.TeslaGate.IgnoredPlayers.Add(joueur);
                    joueur.SetScale(new Vector3(1.5f, 0.5f, 1.7f), Exiled.API.Features.Player.List);
                    break;
                case 4:
                    Log.Debug("Let's go party");
                    foreach (var player in Exiled.API.Features.Player.List)
                    {
                        player.ShowHint("It's " + joueur.Nickname + " birthday !");
                    }

                    float duration2 = 30f;
                    float interval2 = 0.7f;

                    float elapsedTime2 = 0f;

                    while (elapsedTime2 < duration2)
                    {
                        float r = UnityEngine.Random.Range(0f, 1f);
                        float g = UnityEngine.Random.Range(0f, 1f);
                        float b = UnityEngine.Random.Range(0f, 1f);

                        Exiled.API.Features.Map.ChangeLightsColor(new UnityEngine.Color(r, g, b));

                        yield return Timing.WaitForSeconds(interval2);

                        elapsedTime2 += interval2;
                    }

                    Exiled.API.Features.Map.ResetLightsColor();
                    break;
                case 5:
                    Log.Debug("Paper");
                    joueur.ShowHint("You are a paper ! Yippee !");
                    joueur.SetScale(new Vector3(1f, 0.5f, 1f), Exiled.API.Features.Player.List);
                    break;
            }
        }
    }
}