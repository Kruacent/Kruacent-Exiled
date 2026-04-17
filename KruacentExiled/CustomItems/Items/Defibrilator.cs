using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles.Commands;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Displays.Feeds;
using KE.Utils.Extensions;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using MEC;
using PlayerRoles;
using PlayerRoles.Ragdolls;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YamlDotNet.Core.Tokens;

public class Defibrillator : KECustomItem, ILumosItem, IRevivingCustomItem
{

    public const string TranslationFailed = "DefibrillatorFailed";
    public const string TranslationExpired = "DefibrillatorExpired";
    public const string TranslationTooFar = "DefibrillatorTooFar";
    public const string TranslationRevived = "DefibrillatorRevived";
    public const string TranslationSucceed = "DefibrillatorSucceed";
    public const string TranslationInProgress = "DefibrillatorInProgress";
    protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
    {
        return new Dictionary<string, Dictionary<string, string>>()
        {
            ["en"] = new Dictionary<string, string>()
            {
                [TranslationKeyName] = "Defibrillator",
                [TranslationKeyDesc] = "Aim for a dead body to try to re<b>sus</b>citate him",
                [TranslationFailed] = "<color=red>Failed!</color>",
                [TranslationExpired] = "<color=red>Body expired.</color>",
                [TranslationTooFar] = "<color=yellow>Aim at a body.</color>",
                [TranslationRevived] = "<color=cyan>Revived by %MedicName%\n<b>you got brian damage</b></color>",
                [TranslationSucceed] = "<color=green>Successfully ressucitate %PatientName%!</color>",
                [TranslationInProgress] = "<color=yellow>In progress</color>",
            },
            ["fr"] = new Dictionary<string, string>()
            {
                [TranslationKeyName] = "Défibrillateur",
                [TranslationKeyDesc] = "Visez un cadavre de près pour tenter une réanimation.",
                [TranslationFailed] = "<color=red>Réanimation échouée !</color>",
                [TranslationExpired] = "<color=red>Mort trop ancienne.</color>",
                [TranslationTooFar] = "<color=yellow>Rapprochez-vous ou visez un cadavre.</color>",
                [TranslationRevived] = "<color=cyan>Réanimé par %MedicName%\n<b>vous avez des traumatisme crânien</b></color>",
                [TranslationSucceed] = "<color=green>Réanimation réussie sur %PatientName% !</color>",
                [TranslationInProgress] = "<color=yellow>Réanimation en cours...</color>",
            },
        };
    }
    public override ItemType ItemType => ItemType.SCP1853;
    public override string Name { get; set; } = "Defibrillator";
    public override float Weight { get; set; } = 1.0f;
    public UnityEngine.Color Color { get; set; } = UnityEngine.Color.magenta;

    public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
    {
        Limit = 4,
        DynamicSpawnPoints = new List<DynamicSpawnPoint>
        {
            new DynamicSpawnPoint() { Chance = 50, Location = SpawnLocationType.Inside079Secondary },
            new DynamicSpawnPoint() { Chance = 10, Location = SpawnLocationType.Inside096 },
            new DynamicSpawnPoint() { Chance = 10, Location = SpawnLocationType.Inside330 },
            new DynamicSpawnPoint() { Chance = 10, Location = SpawnLocationType.Inside939Cryo },
        },
        LockerSpawnPoints = new List<LockerSpawnPoint>
        {
             new LockerSpawnPoint(){ Chance= 50, Type = LockerType.Medkit, },
             new LockerSpawnPoint(){ Chance= 20, Type = LockerType.Misc, },
        }
    };

    private struct DeathData
    {
        public float Time;
        public RoleTypeId Role;
    }

    private Dictionary<Player, DeathData> _deathRecords;
    private const float MaxReviveTime = 60f;
    private const float RaycastDistance = 2.5f;

    protected override void SubscribeEvents()
    {
        _deathRecords = new Dictionary<Player, DeathData>();
        Exiled.Events.Handlers.Player.UsingItem += OnUsingItem;
        Exiled.Events.Handlers.Player.Dying += OnDying;
        base.SubscribeEvents();
    }

    protected override void UnsubscribeEvents()
    {
        Exiled.Events.Handlers.Player.UsingItem -= OnUsingItem;
        Exiled.Events.Handlers.Player.Dying -= OnDying;
        _deathRecords = null;
        base.UnsubscribeEvents();
    }

    private void OnDying(DyingEventArgs ev)
    {
        if (ev.Player == null) return;

        _deathRecords[ev.Player] = new DeathData
        {
            Time = Time.time,
            Role = ev.Player.Role.Type
        };
    }

    private void OnUsingItem(UsingItemEventArgs ev)
    {
        if (!Check(ev.Item)) return;
        ev.IsAllowed = false;
        Player player = ev.Player;

        Log.Debug($"[Defib] Tentative par {player.Nickname}");

        if (Physics.Raycast(player.CameraTransform.position, player.CameraTransform.forward, out RaycastHit hit, RaycastDistance))
        {
            Log.Debug($"[Defib] Raycast a touché : {hit.collider.name}");

            Collider[] colliders = Physics.OverlapSphere(hit.point, 2.5f);
            BasicRagdoll foundRagdoll = null;

            foreach (Collider col in colliders)
            {
                foundRagdoll = col.GetComponentInParent<BasicRagdoll>();
                if (foundRagdoll != null) break;
            }

            if (foundRagdoll != null)
            {
                Player target = Player.Get(foundRagdoll.NetworkInfo.OwnerHub);
                Log.Debug($"[Defib] Cadavre trouvé ! Owner: {(target != null ? target.Nickname : "Inconnu")}");

                if (target != null && target.Role.Type == RoleTypeId.Spectator && _deathRecords.TryGetValue(target, out DeathData data))
                {
                    if (Time.time - data.Time <= MaxReviveTime)
                    {
                        Log.Debug("[Defib] RÉANIMATION LANCÉE.");
                        player.RemoveItem(ev.Item);
                        Timing.RunCoroutine(ReviveSequence(player, target, foundRagdoll, data.Role));
                        return;
                    }
                    TranslationFeed(player, TranslationExpired);
                    return;
                }
            }
        }
        TranslationFeed(player, TranslationTooFar);
    }

    private IEnumerator<float> ReviveSequence(Player medic, Player patient, BasicRagdoll ragdoll, RoleTypeId previousRole)
    {
        Vector3 lightPos = ragdoll.CenterPoint.position + Vector3.up * 0.75f;
        var shockLight = LabApi.Features.Wrappers.LightSourceToy.Create(lightPos);
        shockLight.Color = UnityEngine.Color.cyan;
        shockLight.Intensity = 50f;
        shockLight.Range = 7f;
        shockLight.Spawn();

        TranslationFeed(medic, TranslationInProgress);

        float elapsed = 0f;
        while (elapsed < 1.0f)
        {
            if (Vector3.Distance(medic.Position, ragdoll.transform.position) > RaycastDistance + 1f)
            {
                TranslationFeed(medic, TranslationFailed);
                shockLight.Destroy();
                CustomItem.TryGive(medic, "Defibrillator",false);
                yield break;
            }

            shockLight.Intensity = (elapsed % 0.2f > 0.1f) ? 100f : 20f;

            elapsed += 0.1f;
            yield return Timing.WaitForSeconds(0.1f);
        }

        shockLight.Intensity = 200f;

        yield return Timing.WaitForSeconds(0.2f);
        shockLight.Destroy();

        patient.Role.Set(previousRole, SpawnReason.Revived, RoleSpawnFlags.All);

        yield return Timing.WaitForSeconds(0.4f);

        if (patient == null) yield break;

        if (patient.IsAlive)
        {
            patient.Position = ragdoll.transform.position + Vector3.up * 0.5f;

            patient.Health = patient.MaxHealth *.2f;
            patient.EnableEffect(EffectType.Flashed, 2f);
            patient.EnableEffect(EffectType.Concussed, 20f);
            patient.EnableEffect(EffectType.Deafened, 20f);


            string msgMedic = GetTranslation(medic, TranslationSucceed);
            string msgPatient = GetTranslation(medic, TranslationRevived);


            HintFeed.AddFeed(medic, msgMedic.Replace("%PatientName%",patient.Nickname));
            KECustomItem.ItemEffectHint(patient, msgPatient.Replace("%MedicName%", medic.Nickname));

            _deathRecords.Remove(patient);

            if (ragdoll != null)
            {
                Object.Destroy(ragdoll.gameObject);
            }
        }
        else
        {
            Log.Error($"[Defib] ÉCHEC : Le joueur {patient.Nickname} n'a pas spawn.");
        }
    }




}