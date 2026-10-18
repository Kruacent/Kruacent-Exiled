using Exiled.API.Enums;
using Exiled.API.Features.Spawn;
using System.Collections.Generic;
using UnityEngine;
using Exiled.Events.EventArgs.Player;
using KruacentExiled.CustomItems.API.Interface;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.Items.ItemEffects;
using Exiled.Events.EventArgs.Item;
using Exiled.API.Features.Items;
using KE.Utils.API.Features;
using InventorySystem.Items.Jailbird;
using Exiled.API.Features;
using KruacentExiled.Audio;
using System;
using System.Linq;
using MEC;
using Exiled.Events.EventArgs.Map;

namespace KruacentExiled.CustomItems.Items
{
    public class FlowerStick : KECustomItem
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Flower Stick",
                    [TranslationKeyDesc] = "It says \"Flowery\"",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Flower Stick",
                    [TranslationKeyDesc] = "It says \"Flowery\"",
                },
            };
        }
        public override ItemType ItemType => ItemType.Jailbird;
        public override string Name { get; set; } = "FlowerStick";
        public override float Weight { get; set; } = 0.65f;

        public CustomItemEffect Effect { get; set; }
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {

        };


        public float CooldownInspect { get; }
        private Dictionary<Player, DateTime> cooldownInspect;
        protected override void SubscribeEvents()
        {
            cooldownInspect = new Dictionary<Player, DateTime>();
            Exiled.Events.Handlers.Map.FillingLocker += OnFillingLocker;
            Exiled.Events.Handlers.Item.JailbirdChangedWearState += OnJailbirdChangedWearState;
            Exiled.Events.Handlers.Item.InspectingItem += OnInspectingItem;
            Exiled.Events.Handlers.Item.JailbirdChargeComplete += OnJailbirdChargeComplete;


            base.SubscribeEvents();
        }



        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Map.FillingLocker -= OnFillingLocker;
            Exiled.Events.Handlers.Item.JailbirdChangedWearState -= OnJailbirdChangedWearState;
            Exiled.Events.Handlers.Item.InspectingItem -= OnInspectingItem;
            Exiled.Events.Handlers.Item.JailbirdChargeComplete -= OnJailbirdChargeComplete;
            cooldownInspect = null;
            base.UnsubscribeEvents();
        }

        private void OnFillingLocker(FillingLockerEventArgs ev)
        {


            if (!ev.IsAllowed) return;

            if (ev.Locker.Type == LockerType.ExperimentalWeapon && ev.Pickup.Type == ItemType.Jailbird)
            {
                Log.Info("set jailbird");
                SetItem(ev.Pickup);
            }


        }

        public const string Jarona = "Flowery_voiceclip_Jarona_";
        public const string SanFrandiscoStrong = "Flowery_voiceclip_here_I_come_San_Frandisco";
        public const string SanFrandiscoWeak = "Snd_flowery_voiceclip_hereicomesanfrandisco_weak";
        public const string LastHit = "Flowery_voiceclip_one_more_for_the_fans";
        public const string Destroyed = "Flowery_voiceclip_I'm_falling";
        public const string Glue = "Flowery_voiceclip_glue";
        public const string Goodbye = "Flowery_voiceclip_goodbye";
        public const string Wind = "Flowery_voiceclip_mysterious_wind";
        private void OnJailbirdChargeComplete(JailbirdChargeCompleteEventArgs ev)
        {
            if (!Check(ev.Item)) return;

            
            if (ev.Item is Jailbird jailbird && jailbird.WearState == JailbirdWearState.AlmostBroken)
            {

                AudioHandler.Instance.PlayToAll(SoundType.Noise, LastHit, ev.Player.GameObject);
            }
            else
            {
                int random = UnityEngine.Random.Range(1, 5);
                AudioHandler.Instance.PlayToAll(SoundType.Noise, Jarona + random, ev.Player.GameObject);
                
            }

        }



        private void OnJailbirdChangedWearState(JailbirdChangedWearStateEventArgs ev)
        {
            if (!Check(ev.Item)) return;
            Player player = ev.Player;

            if (ev.NewWearState == JailbirdWearState.Broken)
            {
                Timing.CallDelayed(1f, () =>
                {
                    AudioHandler.Instance.PlayToAll(SoundType.Noise, Destroyed, ev.Player.GameObject);
                });

                
            }

            cooldownInspect.Remove(player);

        }


        private void OnInspectingItem(InspectingItemEventArgs ev)
        {

            if (!Check(ev.Item)) return;

            Player player = ev.Player;

            if (cooldownInspect.TryGetValue(player, out DateTime value))
            {
                if (value.AddSeconds(CooldownInspect) < DateTime.Now)
                {
                    return;
                }
            }

            cooldownInspect[player] = DateTime.Now;




            if (ev.Item is Jailbird jailbird)
            {
                JailbirdWearState wearState = JailbirdWearState.Broken;
                try
                {
                    wearState = jailbird.WearState;
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

                switch (wearState)
                {
                    case JailbirdWearState.Healthy:
                        AudioHandler.Instance.PlayToAll(SoundType.Noise, SanFrandiscoStrong, ev.Player.GameObject);
                        break;
                    case JailbirdWearState.LowWear:
                        AudioHandler.Instance.PlayToAll(SoundType.Noise, Glue, ev.Player.GameObject);
                        break;
                    case JailbirdWearState.MediumWear:
                        AudioHandler.Instance.PlayToAll(SoundType.Noise, Wind, ev.Player.GameObject);
                        break;
                    case JailbirdWearState.HighWear:
                        AudioHandler.Instance.PlayToAll(SoundType.Noise, SanFrandiscoWeak, ev.Player.GameObject);
                        break;
                    case JailbirdWearState.AlmostBroken:
                        AudioHandler.Instance.PlayToAll(SoundType.Noise, Goodbye, ev.Player.GameObject);
                        break;
                    default:
                        break;

                }





            }

            

        }

    }

}
