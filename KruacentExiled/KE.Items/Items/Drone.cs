using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.API.Features;
using KE.Items.API.Features;
using MEC;
using PlayerRoles.FirstPersonControl;
using System.Collections.Generic;
using UnityEngine;

namespace KE.Items.Items
{
    public class Drone : KECustomItem
    {
        public override string Name { get; set; } = "Drone";
        public override string Description { get; set; } = "Drone de reconnaissance militaire (lancer pour l'utiliser)";
        public override float Weight { get; set; } = 3f;
        public Color Color { get; set; } = Color.blue;
        public override ItemType ItemType => ItemType.KeycardChaosInsurgency;

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Drone",
                    [TranslationKeyDesc] = "Military drone (drop to use)",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "Drone",
                    [TranslationKeyDesc] = "Drone de reconnaissance militaire (lancer pour l'utiliser)",
                },
            };
        }
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
            RoomSpawnPoints = new List<RoomSpawnPoint>
            {
                new RoomSpawnPoint() { Chance = 25, Room = RoomType.HczCornerDeep, },
                new RoomSpawnPoint() { Chance = 25, Room = RoomType.HczIncineratorWayside, },
                new RoomSpawnPoint() { Chance = 25, Room = RoomType.LczAirlock, },
            },
        };

        private Dictionary<ushort, float> DroneBatteries = new Dictionary<ushort, float>();
        private Dictionary<Player, DroneController> ActiveDrones = new Dictionary<Player, DroneController>();
        private const float MaxBattery = 100f;
        // Battery drain rate per second
        public const float BatteryDrainRate = 2.5f;

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Died += OnDied;
            Exiled.Events.Handlers.Player.PickingUpItem += OnPicking;
            Exiled.Events.Handlers.Player.Shooting += OnShooting;
            Exiled.Events.Handlers.Player.InteractingDoor += OnInteracting;
            Exiled.Events.Handlers.Player.UsingItem += OnUsingItem;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Player.PickingUpItem -= OnPicking;
            Exiled.Events.Handlers.Player.Shooting -= OnShooting;
            Exiled.Events.Handlers.Player.InteractingDoor -= OnInteracting;
            Exiled.Events.Handlers.Player.UsingItem -= OnUsingItem;
            base.UnsubscribeEvents();
        }

        private void OnDied(DiedEventArgs ev)
        {
            if(ev.Player.GameObject.TryGetComponent<DroneController>(out DroneController dc)) {
                UnityEngine.Object.Destroy(dc);
            }
        }

        protected override void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (ev.IsThrown) return;
            ev.IsAllowed = false;

            if (KEAbilities.PlayersAbility.TryGetValue(ev.Player, out List<KEAbilities> abilities))
            {
                foreach (KEAbilities ability in abilities)
                {
                    if (ability.IsAbilityActive(ev.Player))
                    {
                        KECustomItem.ItemEffectHint(ev.Player, "Tu ne peux pas utiliser cette item avec une abilité active");
                        return;
                    }
                }
            }

            if (ActiveDrones.ContainsKey(ev.Player))
            {
                StopDrone(ev.Player);
            }
            else
            {
                StartDrone(ev.Player, ev.Item.Serial);
            }
        }

        private void OnInteracting(InteractingDoorEventArgs ev)
        {
            if (!ev.IsAllowed) return;
            ev.IsAllowed = DroneDontHaveArms(ev.Player);
        }

        private void OnPicking(PickingUpItemEventArgs ev)
        {
            if (!ev.IsAllowed) return;
            ev.IsAllowed = DroneDontHaveArms(ev.Player);
        }

        private void OnShooting(ShootingEventArgs ev)
        {
            if (!ev.IsAllowed) return;
            ev.IsAllowed = DroneDontHaveArms(ev.Player);
        }
        private void OnUsingItem(UsingItemEventArgs ev)
        {
            if (!ev.IsAllowed) return;
            ev.IsAllowed = DroneDontHaveArms(ev.Player);
        }

        private bool DroneDontHaveArms(Player player)
        {
            if (ActiveDrones.ContainsKey(player))
            {
                KECustomItem.ItemEffectHint(player, "Le drone n'a pas de bras pour faire ça");
                return false;
            }
            return true;
        }

        private void StartDrone(Player p, ushort serial)
        {
            if (!DroneBatteries.ContainsKey(serial)) DroneBatteries[serial] = MaxBattery;

            if(DroneBatteries[serial] <= 0)
            {
                KECustomItem.ItemEffectHint(p, "Plus de batterie");
                return;
            }

            DroneController dController = p.GameObject.AddComponent<DroneController>();
            dController.DroneItem = this;
            dController.Serial = serial;
            dController.Battery = DroneBatteries[serial];

            ActiveDrones.Add(p, dController);
            
            KEAbilities.TemporaryRemoveAbilities(p);
        }

        public void StopDrone(Player p)
        {
            if (ActiveDrones.TryGetValue(p, out DroneController dc))
            {
                DroneBatteries[dc.Serial] = dc.Battery;
                UnityEngine.Object.Destroy(dc);
                ActiveDrones.Remove(p);
                KEAbilities.ReaffectRemovedAbilities(p);
                p.RemoveItem(dc.Serial, true);
            }
        }
    }

    internal class DroneController : MonoBehaviour
    {
        public Drone DroneItem;
        public ushort Serial;
        public float Battery;

        private Player player;
        private Primitive drone;
        private Npc npc;
        private bool npcIsDead = false;

        private Dictionary<ItemType, ushort> playerSavedAmmo = new Dictionary<ItemType, ushort>();
        private List<ItemType> playerSavedItems = new List<ItemType>();
        private Vector3 originalSize;

        private CoroutineHandle update;
        private bool isDestroying = false;

        private void Awake()
        {
            this.player = Player.Get(base.gameObject);
            this.originalSize = player.Scale;

            SpawnNpc();
            PutPlayerOnDrone();

            this.drone = Primitive.Create(PrimitiveType.Cube, player.Position, player.Rotation.eulerAngles, Vector3.one, false);
            this.drone.Collidable = false;
            this.drone.Spawn();
            this.drone.Transform.parent = player.Transform;
            this.drone.Transform.localPosition = Vector3.zero;
            this.drone.Transform.localRotation = Quaternion.identity;
        }

        private void Start()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            this.update = Timing.RunCoroutine(DroneUpdate());
        }

        private void OnDestroy()
        {
            if (isDestroying) return;
            isDestroying = true;

            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Timing.KillCoroutines(update);

            if (this.player != null && this.player.IsAlive)
            {
                this.player.Scale = this.originalSize;

                if (this.player.Role is FpcRole fpc)
                {
                    fpc.Gravity = FpcGravityController.DefaultGravity;
                }

                if (this.npc != null) this.player.Position = this.npc.Position;
                RestorePlayerInventory();
            }

            if (this.player != null && this.player.Role.Type.IsDead())
            {
                if (this.npc != null) { 
                    this.npc.ClearInventory(false); 
                    this.npc.Destroy(); 
                }
            }
            else
            {
                if (this.npc != null) this.npc.Destroy();
            }

            if (this.drone != null) this.drone.Destroy();
        }

        private IEnumerator<float> DroneUpdate()
        {
            while (true)
            {
                this.Battery -= Drone.BatteryDrainRate * Time.deltaTime;
                if (this.Battery <= 0)
                {
                    KECustomItem.ItemEffectHint(player, "<color=red>Batterie épuisée</color>");
                    DroneItem.StopDrone(player);
                    yield break;
                }

                KECustomItem.ItemEffectHint(player, $"Batterie: <color={(Battery <= 20 ? "red" : "green")}>{Battery:F1}%</color>");

                if (player.Role is FpcRole fpc)
                {
                    float pitch = player.CameraTransform.forward.y;
                    bool isMoving = fpc.MovementDetected;
                    bool isCrouching = fpc.IsCrouching;

                    float targetVelY = 0f;

                    if (isCrouching)
                    {
                        targetVelY = -3f;
                    }
                    else if (isMoving && Mathf.Abs(pitch) > 0.1f)
                    {
                        targetVelY = pitch * 3f;
                    }

                    float currentVelY = fpc.Velocity.y;
                    float diff = targetVelY - currentVelY;

                    Vector3 newGravity = new Vector3(0, diff * 15f, 0);

                    if (Vector3.Distance(fpc.Gravity, newGravity) > 0.2f)
                    {
                        fpc.Gravity = newGravity;
                    }

                    if (targetVelY > 0 && Physics.Raycast(player.Position, Vector3.down, 0.3f, 1 << 0))
                    {
                        player.Position += Vector3.up * 0.2f;
                    }
                }

                yield return Timing.WaitForOneFrame;
            }
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (isDestroying) return;

            if (this.npc != null && ev.Player == this.npc)
            {
                if(this.npc.Health - ev.Amount <= 0 || ev.IsInstantKill)
                {
                    this.npcIsDead = true;
                }

                this.player.Hurt(ev.Amount, ev.DamageHandler.Type);
            }

            if (ev.Player == this.player)
            {
                if ((ev.Player.Health - ev.Amount) <= 0 || ev.IsInstantKill)
                {
                    if (this.npcIsDead)
                    {
                        this.DroneItem.StopDrone(ev.Player);
                    } else
                    {
                        ev.IsAllowed = false;
                        this.player.Position = this.npc.Position;
                        this.player.Health = this.npc.Health;
                        RestorePlayerInventory();
                        this.DroneItem.StopDrone(ev.Player);
                    }
                }
            }
        }

        private void SpawnNpc()
        {
            this.npc = Npc.Spawn(this.player.Nickname, this.player.Role, this.player.Position);
            this.npc.Health = this.player.Health;
            this.npc.Scale = this.player.Scale;

            foreach(Item item in this.player.Items)
            {
                this.npc.AddItem(item.Clone());
            }

            foreach (var ammo in this.player.Ammo)
            {
                this.npc.SetAmmo((AmmoType)ammo.Key, ammo.Value);
            }
        }

        private void PutPlayerOnDrone()
        {
            playerSavedItems.Clear();
            foreach (Item item in this.player.Items) playerSavedItems.Add(item.Type);

            playerSavedAmmo.Clear();
            foreach (var ammo in this.player.Ammo) playerSavedAmmo.Add(ammo.Key, ammo.Value);

            this.player.ClearInventory(true);
            this.player.Scale = Vector3.one * 0.1f;

            if (this.player.Role is FpcRole fpc)
            {
                fpc.Gravity = Vector3.zero;
            }
        }

        private void RestorePlayerInventory()
        {
            this.player.ClearInventory(true);
            foreach (ItemType type in playerSavedItems)
            {
                this.player.AddItem(type);
            }

            foreach (var ammo in playerSavedAmmo)
            {
                this.player.SetAmmo((AmmoType)ammo.Key, ammo.Value);
            }
        }
    }
}