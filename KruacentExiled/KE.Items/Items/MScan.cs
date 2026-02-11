using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.Items.API.Features;
using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace KE.Items.Items
{
    [CustomItem(ItemType.Flashlight)]
    public class MScan : KECustomItem
    {
        public override uint Id { get; set; } = 2090;
        public override string Name { get; set; } = "M-Scan";
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

        private Dictionary<Pickup, Player> ActiveSensors = new Dictionary<Pickup, Player>();
        private Dictionary<Pickup, float> Cooldowns = new Dictionary<Pickup, float>();

        private Dictionary<Pickup, float> BatteryLife = new Dictionary<Pickup, float>();

        private CoroutineHandle SensorRoutine;

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DroppingItem += OnDropping;
            Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUp;
            Exiled.Events.Handlers.Player.Shot += OnShot;

            Exiled.Events.Handlers.Scp049.Attacking += (ev) => CheckDestruction(ev.Player.Position, 2f);
            Exiled.Events.Handlers.Scp096.Enraging += (ev) => CheckDestruction(ev.Player.Position, 2f);
            Exiled.Events.Handlers.Scp939.Clawed += (ev) => CheckDestruction(ev.Player.Position, 2f);
            Exiled.Events.Handlers.Scp106.Teleporting += (ev) => CheckDestruction(ev.Player.Position, 2f);

            SensorRoutine = Timing.RunCoroutine(MotionDetector());
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DroppingItem -= OnDropping;
            Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUp;
            Exiled.Events.Handlers.Player.Shot -= OnShot;

            Timing.KillCoroutines(SensorRoutine);
            ActiveSensors.Clear();
            base.UnsubscribeEvents();
        }

        private new void OnDropping(DroppingItemEventArgs ev)
        {
            if (!Check(ev.Item)) return;

            Timing.CallDelayed(0.5f, () =>
            {
                foreach (Pickup p in Pickup.List)
                {
                    if (p.Serial == ev.Item.Serial)
                    {
                        if (!ActiveSensors.ContainsKey(p))
                        {
                            ActiveSensors.Add(p, ev.Player);
                            BatteryLife[p] = Time.time + 300f;

                            ev.Player.ShowHint("<color=#00ff00>SCANNER DÉPLOYÉ</color>\nBatterie: 5 minutes", 3f);
                        }
                        break;
                    }
                }
            });
        }

        private new void OnPickingUp(PickingUpItemEventArgs ev)
        {
            if (!Check(ev.Pickup)) return;

            if (ActiveSensors.ContainsKey(ev.Pickup))
            {
                ActiveSensors.Remove(ev.Pickup);
                BatteryLife.Remove(ev.Pickup);
                ev.Player.ShowHint("Scanner récupéré.", 2f);
            }
        }

        private void OnShot(ShotEventArgs ev) => CheckDestruction(ev.Position, 0.5f);

        private void CheckDestruction(Vector3 hitPos, float radius)
        {
            List<Pickup> toDestroy = new List<Pickup>();

            foreach (var kvp in ActiveSensors)
            {
                Pickup sensor = kvp.Key;
                Player owner = kvp.Value;
                if (sensor == null) continue;

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
        }

        private IEnumerator<float> MotionDetector()
        {
            while (true)
            {
                List<Pickup> invalid = new List<Pickup>();
                float currentTime = Time.time;

                foreach (var key in ActiveSensors.Keys)
                {
                    if (key == null || key.GameObject == null) invalid.Add(key);
                    else if (BatteryLife.ContainsKey(key) && currentTime > BatteryLife[key])
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
                    if (i != null) i.Destroy();
                }

                foreach (var kvp in ActiveSensors)
                {
                    Pickup sensor = kvp.Key;
                    Player owner = kvp.Value;

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
                                string color = "orange";
                                if (target.Role.Side == Side.Scp) color = "red";
                                else if (target.Role.Side == Side.Mtf) color = "blue";
                                else if (target.Role.Side == Side.ChaosInsurgency) color = "green";

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