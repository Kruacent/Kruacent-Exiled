using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.EventArgs.Scp173;
using Exiled.Events.EventArgs.Scp939;
using KruacentExiled.GlobalEventFramework.GEFE.API.Enums;
using KruacentExiled.GlobalEventFramework.GEFE.API.Features;
using KruacentExiled.GlobalEventFramework.GEFE.API.Interfaces;
using MEC;
using System.Collections.Generic;
using PlayerHandler = Exiled.Events.Handlers.Player;

namespace KruacentExiled.GlobalEventFramework.Examples.GE
{
    /// <summary>
    /// Everyone has a movement boost effect (stackable)
    /// Maybe inspired by Dr Bright's Mayhem
    /// </summary>
    public class Speed : GlobalEvent, IAsyncStart, IEvent
    {
        public override ImpactLevel ImpactLevel => ImpactLevel.VeryHigh;

        ///<inheritdoc/>
        public override uint Id { get; set; } = 1042;
        ///<inheritdoc/>
        public override string Name { get; set; } = "Speed";
        ///<inheritdoc/>
        public override string Description { get; } = "Gas! gas! gas!";

        public override string[] AltDescription => new string[]
        {
            "Super instinct!",
            "Mayhem mode activated",
        };
        ///<inheritdoc/>
        public override int WeightedChance { get; set; } = 5;
        /// <summary>
        /// intensity of the movement boost effect
        /// </summary>
        public static byte MovementBoost { get; set; } = 100;
        ///<inheritdoc/>
        public IEnumerator<float> Start()
        {
            yield return Timing.WaitForSeconds(1);
            foreach (Player player in Player.Enumerable)
            {
                GiveEffect(player);
            }
            
        }
        ///<inheritdoc/>
        public void SubscribeEvent()
        {
            PlayerHandler.ChangingRole += ReactivateEffectSpawn;
            Exiled.Events.Handlers.Scp173.Blinking += SpeedyNut;
            Exiled.Events.Handlers.Scp939.Lunging += OnLunging;
            Exiled.Events.Handlers.Scp096.Charging += OnCharging;
            Exiled.Events.Handlers.Scp939.Clawed += OnClawed;
        }
        ///<inheritdoc/>
        public void UnsubscribeEvent()
        {
            PlayerHandler.ChangingRole -= ReactivateEffectSpawn;
            Exiled.Events.Handlers.Scp173.Blinking -= SpeedyNut;
            Exiled.Events.Handlers.Scp939.Lunging -= OnLunging;
            Exiled.Events.Handlers.Scp096.Charging -= OnCharging;
            Exiled.Events.Handlers.Scp939.Clawed -= OnClawed;

            foreach (Player player in Player.Enumerable)
            {
                player.DisableEffect<MovementBoost>();
            }
        }
        /// <summary>
        /// Decrease the blink cooldown of SCP-173
        /// </summary>
        public static void SpeedyNut(BlinkingEventArgs ev)
        {
            ev.BlinkCooldown = ev.BlinkCooldown/3;
        }

        /// <summary>
        /// Reactivate the effect at each role changement
        /// </summary>
        public static void ReactivateEffectSpawn(ChangingRoleEventArgs ev)
        {

            GiveEffect(ev.Player);
        }

        public static void GiveEffect(Player player)
        {
            Timing.CallDelayed(.1f, () => player.EnableEffect<MovementBoost>(MovementBoost, 999999999, true));
        }

        public static void OnClawed(ClawedEventArgs ev)
        {
            if(ev.Player.TryGetEffect<Invisible>(out _))
            {
                ev.Player.DisableEffect<Invisible>();
            }
        }


        public static void OnLunging(LungingEventArgs ev)
        {
            Timing.CallDelayed(.70f, delegate
            {
                ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                grenade.ScpDamageMultiplier = 0;
                grenade.FuseTime = .1f;
                grenade.SpawnActive(ev.Player.Position, ev.Player);
            });

        }


        public static void OnCharging(ChargingEventArgs ev)
        {
            if (!ev.IsAllowed) return;

            Timing.RunCoroutine(SpawnGrenade(ev.Player));
            
        }


        private static IEnumerator<float> SpawnGrenade(Player player)
        {
            int i = 0;
            int target = 5;

            while (i < target)
            {
                ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                grenade.ScpDamageMultiplier = 0;
                grenade.FuseTime = .1f;
                grenade.SpawnActive(player.Position, player);
                yield return Timing.WaitForSeconds(.25f);
                i++;
            }
        }
    }
}
