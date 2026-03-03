using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Pools;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp049;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.EventArgs.Scp106;
using Exiled.Events.EventArgs.Scp939;
using KE.Items.API.Features;
using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace KE.Items.Items
{
    public class MScan : KECustomItem
    {
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "M-Scan",
                    [TranslationKeyDesc] = "Detect movement",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "M-Scan",
                    [TranslationKeyDesc] = "Détecte les mouvements des personnes passant devant",
                },
            };
        }
        public override ItemType ItemType => ItemType.Flashlight;
        public override string Name { get; set; } = "MScan";
        public override string Description { get; set; } = "Détecte les mouvements des personnes passant devant";
        public override float Weight { get; set; } = 1.5f;
        public Color Color { get; set; } = Color.cyan;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 2,
            RoomSpawnPoints = new List<RoomSpawnPoint>
            {
                new RoomSpawnPoint()
                {
                    Chance = 25,
                    Room = RoomType.LczGlassBox,
                },
                new RoomSpawnPoint()
                {
                    Chance = 25,
                    Room = RoomType.HczIncineratorWayside,
                },
                new RoomSpawnPoint()
                {
                    Chance = 25,
                    Room = RoomType.LczCafe,
                },
            },
        };

        private Dictionary<Pickup, Player> ActiveSensors;
        private Dictionary<Pickup, float> Cooldowns = new Dictionary<Pickup, float>();

        private Dictionary<Pickup, float> BatteryLife = new Dictionary<Pickup, float>();

        private CoroutineHandle SensorRoutine;

        protected override void SubscribeEvents()
        {

            
            ActiveSensors = DictionaryPool<Pickup, Player>.Pool.Get();
            Exiled.Events.Handlers.Player.Shot += OnShot;
            Exiled.Events.Handlers.Player.DroppedItem += OnDroppedItem;

            Exiled.Events.Handlers.Scp049.Attacking += OnSCP049Attacking;
            Exiled.Events.Handlers.Scp106.Teleporting += OnSCP106Teleporting;
            Exiled.Events.Handlers.Scp939.Clawed += OnSCP939Attacked;
            Exiled.Events.Handlers.Scp096.Enraging += OnSCP096Enraging;

            SensorRoutine = Timing.RunCoroutine(MotionDetector());
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Shot -= OnShot;
            Exiled.Events.Handlers.Player.DroppedItem -= OnDroppedItem;

            Exiled.Events.Handlers.Scp049.Attacking -= OnSCP049Attacking;
            Exiled.Events.Handlers.Scp106.Teleporting -= OnSCP106Teleporting;
            Exiled.Events.Handlers.Scp939.Clawed -= OnSCP939Attacked;
            Exiled.Events.Handlers.Scp096.Enraging -= OnSCP096Enraging;

            Timing.KillCoroutines(SensorRoutine);
            DictionaryPool<Pickup, Player>.Pool.Return(ActiveSensors);
            base.UnsubscribeEvents();
        }



        private void OnSCP049Attacking(Exiled.Events.EventArgs.Scp049.AttackingEventArgs ev)
        {
            CheckDestruction(ev.Player.Position, 2f);
        }
        private void OnSCP106Teleporting(TeleportingEventArgs ev)
        {
            CheckDestruction(ev.Player.Position, 2f);
        }
        private void OnSCP939Attacked(ClawedEventArgs ev)
        {
            CheckDestruction(ev.Player.Position, 2f);
        }

        private void OnSCP096Enraging(EnragingEventArgs ev)
        {
            CheckDestruction(ev.Player.Position, 2f);
        }

        public const float TimeUp = 120;

        private void OnDroppedItem(DroppedItemEventArgs ev)
        {
            if (!Check(ev.Pickup)) return;


            Player player = ev.Player;
            Pickup pickup = ev.Pickup;

            if (!ActiveSensors.ContainsKey(pickup))
            {
                ActiveSensors.Add(pickup, player);
                BatteryLife[pickup] = Time.time + TimeUp;

                KECustomItem.ItemEffectHint(player, $"<color=#00ff00>SCANNER DÉPLOYÉ</color>\nBatterie: {TimeUp} secondes");
            }
        }

        protected override void OnPickingUp(PickingUpItemEventArgs ev)
        {
            Player player = ev.Player;

            if (ActiveSensors.ContainsKey(ev.Pickup))
            {
                ActiveSensors.Remove(ev.Pickup);
                BatteryLife.Remove(ev.Pickup);
                KECustomItem.ItemEffectHint(player, "Scanner récupéré.");
            }
        }

        private void OnShot(ShotEventArgs ev)
        {
            CheckDestruction(ev.Position, 0.5f);
        }

        private void CheckDestruction(Vector3 hitPos, float radius)
        {
            List<Pickup> toDestroy = ListPool<Pickup>.Pool.Get();

            foreach (var kvp in ActiveSensors)
            {
                Pickup sensor = kvp.Key;
                Player owner = kvp.Value;
                if (!sensor.IsSpawned) continue;

                if (Vector3.Distance(hitPos, sensor.Position) <= radius)
                {
                    toDestroy.Add(sensor);
                    if (owner != null) KECustomItem.ItemEffectHint(owner, "<color=red>SCANNER DÉTRUIT</color>");
                }
            }

            foreach (var p in toDestroy)
            {
                ActiveSensors.Remove(p);
                BatteryLife.Remove(p);
                p.Destroy();
            }

            ListPool<Pickup>.Pool.Return(toDestroy);
        }



        private void CheckBattery()
        {
            List<Pickup> invalid = ListPool<Pickup>.Pool.Get();
            foreach (var key in ActiveSensors.Keys)
            {
                if (BatteryLife.ContainsKey(key) && Time.time > BatteryLife[key])
                {
                    if (ActiveSensors[key] != null)
                        KECustomItem.ItemEffectHint(ActiveSensors[key], "<color=yellow>Scanner: Batterie épuisée.</color>");
                    invalid.Add(key);
                }
            }

            foreach (var i in invalid)
            {
                ActiveSensors.Remove(i);
                BatteryLife.Remove(i);
                if (!i.IsSpawned) i.Destroy();
            }
            ListPool<Pickup>.Pool.Return(invalid);
        }

        private IEnumerator<float> MotionDetector()
        {
            while (true)
            {
                
                float currentTime = Time.time;

                CheckBattery();


                foreach (var kvp in ActiveSensors)
                {
                    Pickup sensor = kvp.Key;
                    Player owner = kvp.Value;

                    if (sensor.Base == null) continue;

                    if (Cooldowns.ContainsKey(sensor) && currentTime < Cooldowns[sensor]) continue;

                    bool detected = false;

                    foreach (Player target in Player.List)
                    {
                        if (target == owner) continue;
                        if (!target.IsAlive || target.IsNoclipPermitted) continue;
                        if (owner != null && target.Role.Side == owner.Role.Side) continue;
                        

                        if (Vector3.Distance(sensor.Position, target.Position) < 3.5f)
                        {
                            detected = true;

                            if (owner != null)
                            {
                                string color = target.Role.Color.ToHex();
                                KECustomItem.ItemEffectHint(owner, $"<color={color}>M-SCAN: {target.Role.Name} ({target.Nickname})</color>");
                            }

                            break;
                        }
                    }

                    if (detected)
                    {
                        Cooldowns[sensor] = currentTime + 3.0f;
                    }
                }

                yield return Timing.WaitForSeconds(0.5f);
            }
        }
    }
}