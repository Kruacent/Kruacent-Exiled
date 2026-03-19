using AdminToys;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Pools;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Toys;
using Exiled.API.Structs;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles.Commands;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp049;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.EventArgs.Scp106;
using Exiled.Events.EventArgs.Scp939;
using KE.Items.API.Features;
using KE.Items.Utils;
using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace KE.Items.Items
{
    public class MScan : KECustomItem
    {


        public const string Deploy = "MScanDeploy";
        public const string PickUp = "MScanPickUp";
        public const string TranslationDestroy = "MScanDestroy";
        public const string NoBattery = "MScanNoBattery";
        public const string Detect = "MScanDetect";

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "M-Scan",
                    [TranslationKeyDesc] = "Detect movement",
                    [Deploy] = $"<color=#00ff00>MSCAN DEPLOYED</color> Battery: {TimeUp} seconds",
                    [PickUp] = "M-Scan picked up",
                    [TranslationDestroy] = "<color=red>M-SCAN DESTROYED</color>",
                    [NoBattery] = "<color=yellow>M-Scan : No battery left</color>",
                    [Detect] = "<color=%Color%>M-SCAN: %Name%</color>",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "M-Scan",
                    [TranslationKeyDesc] = "Détecte les mouvements des personnes passant devant",
                    [Deploy] = $"<color=#00ff00>SCANNER DÉPLOYÉ</color> Batterie: {TimeUp} secondes",
                    [PickUp] = "Scanner récupéré.",
                    [TranslationDestroy] = "<color=red>SCANNER DÉTRUIT</color>",
                    [NoBattery] = "<color=yellow>Scanner: Batterie épuisée.</color>",
                    [Detect] = "$<color=%Color%>M-SCAN: %Name%</color>",
                },
            };
        }
        public override ItemType ItemType => ItemType.Flashlight;
        public override string Name { get; set; } = "MScan";
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
        private Dictionary<Pickup, Primitive> Models = new();

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
                //Models[pickup] = CreateBaseModel(pickup);

                HintFeed.AddFeed(player, GetTranslation(player, Deploy));
            }
        }

        private Primitive CreateBaseModel(Pickup pickup)
        {
            Primitive primitive = Primitive.Create(null, null, null, false);
            primitive.Transform.localPosition = pickup.Position;
            primitive.Transform.localRotation = Quaternion.identity;
            primitive.Transform.localScale = Vector3.one;
            primitive.MovementSmoothing = 0;
            primitive.Flags = PrimitiveFlags.None;
            primitive.Spawn();
            return primitive;
        }

        protected override void OnPickingUp(PickingUpItemEventArgs ev)
        {
            Player player = ev.Player;
            Pickup pickup = ev.Pickup;

            if (ActiveSensors.ContainsKey(pickup))
            {
                
                Remove(pickup);
                HintFeed.AddFeed(player, GetTranslation(player, PickUp));
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
                    if (owner != null)
                    {
                        HintFeed.AddFeed(owner, GetTranslation(owner, TranslationDestroy));
                    }
                }
            }

            foreach (Pickup pickup in toDestroy)
            {
                Remove(pickup);
                Cooldowns.Remove(pickup);
                pickup.Destroy();
            }

            ListPool<Pickup>.Pool.Return(toDestroy);
        }

        private void Remove(Pickup pickup)
        {
            ActiveSensors.Remove(pickup);
            BatteryLife.Remove(pickup);
            //Models[pickup].Destroy();
        }


        private void CheckBattery()
        {
            List<Pickup> invalid = ListPool<Pickup>.Pool.Get();
            foreach (var key in ActiveSensors.Keys)
            {
                if (BatteryLife.ContainsKey(key) && Time.time > BatteryLife[key])
                {

                    Player player = ActiveSensors[key];
                    if (player != null)
                    {
                        HintFeed.AddFeed(player, GetTranslation(player, NoBattery));
                    }
                    invalid.Add(key);
                }
            }

            foreach (var i in invalid)
            {
                ActiveSensors.Remove(i);
                BatteryLife.Remove(i);
                i.Destroy();
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

                                string msg = GetTranslation(owner, Detect).Replace("%Color%", color).Replace("%Name%", target.Role.Name);

                                HintFeed.AddFeed(owner, msg);
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