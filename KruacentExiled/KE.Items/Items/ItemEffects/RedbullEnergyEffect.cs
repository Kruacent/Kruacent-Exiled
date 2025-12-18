using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Items.API.Features;
using KE.Items.API.Interface;
using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace KE.Items.Items.ItemEffects
{
    public class RedBullEnergyEffect : CustomItemEffect
    {
        private const string ActiveKey = "KE.Items.Redbull.Active";
        private const string DoorExplodeKey = "KE.Items.Redbull.Immune";
        private const float ScanRadiusSqr = 6.40f;
        private const float BoostDuration = 20f;
        private const float CrashDuration = 30f;

        private readonly Dictionary<int, CoroutineHandle> _activeRoutines = new Dictionary<int, CoroutineHandle>();

        public override void Effect(UsedItemEventArgs ev) => TriggerEffect(ev.Player);

        public override void Effect(DroppingItemEventArgs ev) => TriggerEffect(ev.Player);

        public override void Effect(ExplodingGrenadeEventArgs ev) => TriggerEffect(ev.Player);

        private void TriggerEffect(Player player)
        {
            if (player.SessionVariables.ContainsKey(ActiveKey))
            {
                KECustomItem.ItemEffectHint(player, "<color=red>Ton cœur ne supporterait pas une autre canette !</color>");
                return;
            }

            if (_activeRoutines.TryGetValue(player.Id, out CoroutineHandle oldHandle))
                Timing.KillCoroutines(oldHandle);

            _activeRoutines[player.Id] = Timing.RunCoroutine(RunRedBullSequence(player));
        }

        private IEnumerator<float> RunRedBullSequence(Player p)
        {
            p.SessionVariables[ActiveKey] = true;

            p.EnableEffect(EffectType.Scp207, 5, BoostDuration);
            p.EnableEffect(EffectType.DamageReduction, 40, BoostDuration);

            float elapsed = 0f;
            while (elapsed < BoostDuration)
            {
                if (p == null || !p.IsAlive) yield break;

                p.Stamina = 1f;
                if (p.Health < p.MaxHealth) p.Health += 0.5f;

                Vector3 pPos = p.Position;
                foreach (Door door in Door.List)
                {
                    if (!door.IsGate && door is BreakableDoor breakable && !breakable.IsDestroyed)
                    {
                        if ((breakable.Position - pPos).sqrMagnitude < ScanRadiusSqr)
                        {
                            p.SessionVariables[DoorExplodeKey] = true;

                            ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                            grenade.FuseTime = 0.1f;
                            breakable.IsOpen = true;
                            grenade.SpawnActive(breakable.Position, p);

                            Timing.CallDelayed(0.2f, () => {
                                if (p != null) p.SessionVariables.Remove(DoorExplodeKey);
                            });
                        }
                    }
                }

                elapsed += 0.2f;
                yield return Timing.WaitForSeconds(0.2f);
            }

            if (p == null || !p.IsAlive) yield break;

            KECustomItem.ItemEffectHint(p, "Ton coeur va exploser !");

            p.DisableEffect(EffectType.Scp207);
            p.Stamina = 0.25f;

            p.EnableEffect(EffectType.Deafened, CrashDuration);
            p.EnableEffect(EffectType.Slowness, CrashDuration);
            p.EnableEffect(EffectType.Concussed, CrashDuration);
            p.EnableEffect(EffectType.Exhausted, CrashDuration);

            yield return Timing.WaitForSeconds(CrashDuration);

            Cleanup(p);
        }

        public void OnHurting(HurtingEventArgs ev)
        {
            if (ev.DamageHandler.Type == DamageType.Explosion && ev.Player.SessionVariables.ContainsKey(DoorExplodeKey))
                ev.Amount = 0;
        }

        public void Cleanup(Player p)
        {
            if (p == null) return;

            if (_activeRoutines.TryGetValue(p.Id, out CoroutineHandle handle))
            {
                Timing.KillCoroutines(handle);
                _activeRoutines.Remove(p.Id);
            }

            if (p.SessionVariables.ContainsKey(ActiveKey)) p.SessionVariables.Remove(ActiveKey);
            if (p.SessionVariables.ContainsKey(DoorExplodeKey)) p.SessionVariables.Remove(DoorExplodeKey);
        }
    }
}