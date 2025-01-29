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

/// <inheritdoc />
[CustomItem(ItemType.Adrenaline)]
public class AdrenalineDrogue : CustomItem
{
    /// <inheritdoc/>
    public override uint Id { get; set; } = 1402;

    /// <inheritdoc/>
    public override string Name { get; set; } = "DA-020";

    /// <inheritdoc/>
    public override string Description { get; set; } = "La bonne drogue là, si vous le prenez vous êtes ienb pendant 20 secondes puis vous vous sentez pas bien !";

    /// <inheritdoc/>
    public override float Weight { get; set; } = 0.65f;

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
            if (result.Id == 19)
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
        /* EFFET DE LA DROGUE */
        joueur.ShowHint("Vous êtes actuellement sous effet de la cocaïne liquide !");
        joueur.EnableEffect<MovementBoost>(40, true);
        joueur.EnableEffect<InsufficientLighting>(30, true);
        joueur.EnableEffect<BodyshotReduction>(30, true);
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
        joueur.EnableEffect<MovementBoost>(35);


        yield return Timing.WaitForSeconds(UnityEngine.Random.Range(180, 300));

        if (joueur.IsAlive)
        {
            int randomNumber = UnityEngine.Random.Range(1, 6);
            switch (randomNumber)
            {
                case 1:
                    Log.Debug(joueur.Nickname + " a changé d'apparence !");
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
                    joueur.ShowHint("Vous avez perdu votre langue ! (esperons que celle-ci repousse)");
                    joueur.Mute();
                    yield return Timing.WaitForSeconds(UnityEngine.Random.Range(30, 100));
                    joueur.ShowHint("Je crois que c'est bon, ça a repoussé !");
                    joueur.UnMute();
                    break;
                case 3:
                    joueur.ShowHint("Vous êtes devenu du caoutchouc !");
                    Exiled.API.Features.TeslaGate.IgnoredPlayers.Add(joueur);
                    joueur.SetScale(new Vector3(1.5f, 0.5f, 1.7f), Exiled.API.Features.Player.List);
                    break;
                case 4:
                    Log.Debug("Let's go party");
                    foreach (var player in Exiled.API.Features.Player.List)
                    {
                        player.ShowHint(joueur.Nickname + " à commencer une fête d'anniversaire !");
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
                    joueur.ShowHint("Bienvenue dans le monde des papiers. Évite les ciseaux !");
                    joueur.SetScale(new Vector3(1f, 0.5f, 1f), Exiled.API.Features.Player.List);
                    break;
            }
        }
    }
}