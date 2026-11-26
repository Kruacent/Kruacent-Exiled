using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using HarmonyLib;
using InventorySystem.Items;
using KE.Utils.API.GifAnimator;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using KruacentExiled.CustomRoles.API.Interfaces.Ability;
using LabApi.Events.Arguments.Scp049Events;
using MEC;
using Mirror;
using PlayerRoles.PlayableScps.Scp049;
using PlayerRoles.Ragdolls;
using PlayerStatsSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TargetCountHider = KruacentExiled.Misc.Utils.TargetCountHider;

namespace KruacentExiled.CustomRoles.Abilities
{
    public class SimulateDeath : KEAbilities, ICustomIcon, IDuration
    {
        public override string Name { get; } = "SimulateDeath";

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Play dead",
                    [TranslationKeyDesc] = "Simulate your own death",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Faire le mort",
                    [TranslationKeyDesc] = "T'es talent de mime te permettent de simuler la mort",
                }
            };
        }

        public TextImage IconName => MainPlugin.Instance.icons[Name];
        public override float Cooldown => 60f;
        public float Duration => 10f;

        private sealed class FakeDeathState
        {
            public Ragdoll Ragdoll;
            public Vector3 Position;
            public Vector3 Scale;
            public List<Pickup> DroppedItems;
            public CoroutineHandle CameraMovement;
        }

        private readonly Dictionary<Player, FakeDeathState> fakeDeaths = new Dictionary<Player, FakeDeathState>();
        private static readonly HashSet<BasicRagdoll> FakeBody = new HashSet<BasicRagdoll>();

        protected override bool AbilityUsed(Player player)
        {
            Ragdoll ragdoll = SpawnRagdoll(player);

            if (ragdoll == null)
            {
                return base.AbilityUsed(player);
            }

            FakeDeathState fakeDeath = new FakeDeathState
            {
                Ragdoll = ragdoll,
                Position = player.Position,
                Scale = player.Scale,
                DroppedItems = DropAllItems(player),
            };
            fakeDeaths[player] = fakeDeath;
            TrackFakeRagdoll(ragdoll.Base);

            HidePlayer(player);
            fakeDeath.CameraMovement = Timing.RunCoroutine(FollowRagdollMove(player, ragdoll));

            return base.AbilityUsed(player);
        }
        
        private static List<Pickup> DropAllItems(Player player)
        {
            List<Pickup> droppedItems = new List<Pickup>();

            foreach (Item item in player.Items.ToList())
            {
                Pickup pickup = player.DropItem(item);
                if (pickup != null)
                {
                    droppedItems.Add(pickup);
                }
            }

            return droppedItems;
        }
       
        public void ActionAfterAbility(Player player)
        {
            ApplyOldPlayerState(player);
        }

        protected override void AbilityRemoved(Player player)
        {
            ApplyOldPlayerState(player);
            base.AbilityRemoved(player);
        }

        private static Ragdoll SpawnRagdoll(Player player)
        {
            List<DeathTranslation> deathReasons = DeathTranslations.TranslationsById.Values.ToList();
            DeathTranslation randomReason = deathReasons[Random.Range(0, deathReasons.Count)];

            Vector3 velocity = player.Velocity;
            if (velocity.y < 0f)
            {
                velocity.y = 0f;
            }

            CustomReasonDamageHandler cs = new CustomReasonDamageHandler(randomReason.DeathscreenTranslation)
            {
                StartVelocity = velocity,
            };

            BasicRagdoll ragdoll = RagdollManager.ServerSpawnRagdoll(
                player.ReferenceHub,
                cs
            );

            return Ragdoll.Get(ragdoll);
        }

        private void HidePlayer(Player player)
        {
            player.Scale = Vector3.one * 0.2f;
            player.IsGodModeEnabled = true;

            player.EnableEffect(EffectType.Invisible, Duration);
            player.EnableEffect(EffectType.Ensnared, Duration);

            TargetCountHider.Hide(player);
        }

        private IEnumerator<float> FollowRagdollMove(Player player, Ragdoll ragdoll)
        {
            Transform newCameraPosition = FindHead(ragdoll);
            float elapsed = 0f;

            while (elapsed < Duration)
            {
                if (!player.IsAlive || ragdoll.Base == null)
                {
                    yield break;
                }

                player.Position = newCameraPosition.position;
                player.Rotation = newCameraPosition.rotation;

                yield return Timing.WaitForOneFrame;
                elapsed += Time.deltaTime;
            }
        }

        private static Transform FindHead(Ragdoll ragdoll)
        {
            if (ragdoll.Base is DynamicRagdoll dynamicRagdoll)
            {
                foreach (Transform bodyPart in dynamicRagdoll.LinkedRigidbodiesTransforms)
                {
                    if (bodyPart.name == "Head")
                    {
                        return bodyPart;
                    }
                }
            }

            return ragdoll.Base.CenterPoint;
        }

        private static void GiveBackItems(Player player, List<Pickup> droppedItems)
        {
            foreach (Pickup pickup in droppedItems)
            {
                if (!pickup.IsSpawned || pickup.InUse)
                {
                    continue;
                }

                player.AddItem(pickup, ItemAddReason.PickedUp);
                pickup.Destroy();
            }
        }

        private void ApplyOldPlayerState(Player player)
        {
            if (!fakeDeaths.TryGetValue(player, out FakeDeathState fakeDeath))
            {
                return;
            }

            fakeDeaths.Remove(player);
            Timing.KillCoroutines(fakeDeath.CameraMovement);
            TargetCountHider.Show(player);

            Vector3 standUpPosition = fakeDeath.Position;
            if (fakeDeath.Ragdoll.Base != null)
            {
                ForgetFakeRagdoll(fakeDeath.Ragdoll.Base);
                standUpPosition = fakeDeath.Ragdoll.Base.CenterPoint.position + Vector3.up;
                fakeDeath.Ragdoll.Destroy();
            }

            if (!player.IsConnected)
            {
                return;
            }

            player.IsGodModeEnabled = false;
            player.Scale = fakeDeath.Scale;

            if (player.IsAlive)
            {
                GiveBackItems(player, fakeDeath.DroppedItems);
                player.Position = standUpPosition;
            }
        }

        private static void TrackFakeRagdoll(BasicRagdoll ragdoll)
        {
            if (FakeBody.Count == 0)
            {
                LabApi.Events.Handlers.Scp049Events.StartingResurrection += OnStartingResurrection;
                LabApi.Events.Handlers.Scp049Events.ResurrectingBody += OnResurrectingBody;
            }

            FakeBody.Add(ragdoll);
        }

        private static void ForgetFakeRagdoll(BasicRagdoll ragdoll)
        {
            if (!FakeBody.Remove(ragdoll))
            {
                return;
            }

            if (FakeBody.Count == 0)
            {
                LabApi.Events.Handlers.Scp049Events.StartingResurrection -= OnStartingResurrection;
                LabApi.Events.Handlers.Scp049Events.ResurrectingBody -= OnResurrectingBody;
            }
        }

        private static void OnStartingResurrection(Scp049StartingResurrectionEventArgs ev)
        {
            if (FakeBody.Contains(ev.Ragdoll.Base))
            {
                ev.CanResurrect = true;
            }
        }

        private static void OnResurrectingBody(Scp049ResurrectingBodyEventArgs ev)
        {
            BasicRagdoll ragdoll = ev.Ragdoll.Base;
            if (!FakeBody.Contains(ragdoll))
            {
                return;
            }

            ev.IsAllowed = false;
            ForgetFakeRagdoll(ragdoll);
            NetworkServer.Destroy(ragdoll.gameObject);
        }

        [HarmonyPatch(typeof(Scp049ResurrectAbility), nameof(Scp049ResurrectAbility.ServerValidateAny))]
        private static class KeepFakeRagdollRevivablePatch
        {
            private static void Postfix(Scp049ResurrectAbility __instance, ref bool __result)
            {
                if (__result || FakeBody.Count == 0)
                {
                    return;
                }

                BasicRagdoll ragdoll = __instance.CurRagdoll;
                if (ragdoll == null || !FakeBody.Contains(ragdoll))
                {
                    return;
                }

                __result = true;
            }
        }

    }
}