using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Player;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomRoles.API.Features;
using MEC;
using PlayerRoles.FirstPersonControl;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.CustomItems.Items
{
    public class Drone : KECustomItem
    {
        public override string Name { get; set; } = "Drone";
        public override float Weight { get; set; } = 3f;
        public Color Color { get; set; } = Color.blue;
        public override ItemType ItemType => ItemType.KeycardJanitor;// make a custom keycard

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Drone",
                    [TranslationKeyDesc] = "Military drone (drop to use)",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Drone",
                    [TranslationKeyDesc] = "Drone de reconnaissance militaire (lâcher pour l'utiliser)",
                },
            };
        }
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
            RoomSpawnPoints = new List<RoomSpawnPoint>
            {
                new RoomSpawnPoint() 
                { 
                    Chance = 25, Room = RoomType.HczCornerDeep, 
                },
                new RoomSpawnPoint() 
                { 
                    Chance = 25, Room = RoomType.HczIncineratorWayside, 
                },
                new RoomSpawnPoint() 
                { 
                    Chance = 25, Room = RoomType.LczAirlock,
                },
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
            if(ev.Player.GameObject.TryGetComponent(out DroneController dc)) 
            {
                Object.Destroy(dc);
                ActiveDrones.Remove(ev.Player);
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
                        ItemEffectHint(ev.Player, "Tu ne peux pas utiliser cette item avec une abilité active");
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
                ItemEffectHint(player, "Le drone n'a pas de bras pour faire ça");
                return false;
            }
            return true;
        }

        private void StartDrone(Player p, ushort serial)
        {
            if (!DroneBatteries.ContainsKey(serial)) DroneBatteries[serial] = MaxBattery;

            if(DroneBatteries[serial] <= 0)
            {
                ItemEffectHint(p, "Plus de batterie");
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
                Object.Destroy(dc);
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
            player = Player.Get(gameObject);
            originalSize = player.Scale;

            SpawnNpc();
            PutPlayerOnDrone();

            drone = Primitive.Create(PrimitiveType.Cube, player.Position, player.Rotation.eulerAngles, Vector3.one, false);
            drone.Collidable = false;
            drone.Spawn();
            drone.Transform.parent = player.Transform;
            drone.Transform.localPosition = Vector3.zero;
            drone.Transform.localRotation = Quaternion.identity;
        }

        private void Start()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            update = Timing.RunCoroutine(DroneUpdate());
        }

        private void OnDestroy()
        {
            if (isDestroying) return;
            isDestroying = true;

            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Timing.KillCoroutines(update);

            if (player != null && player.IsAlive)
            {
                player.Scale = originalSize;

                if (player.Role is FpcRole fpc)
                {
                    fpc.Gravity = FpcGravityController.DefaultGravity;
                }

                if (npc != null)
                {
                    player.Position = npc.Position;
                }
                RestorePlayerInventory();
            }

            if (player != null && player.Role.Type.IsDead())
            {
                if (npc != null)
                {
                    npc.ClearInventory(false);
                    npc.Destroy(); 
                }
            }
            else
            {
                if (npc != null)
                {
                    npc.Destroy();
                }
            }

            if (drone != null)
            {
                drone.Destroy();
            }
        }

        private IEnumerator<float> DroneUpdate()
        {
            while (true)
            {
                Battery -= Drone.BatteryDrainRate * Time.deltaTime;
                if (Battery <= 0)
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

            if (npc != null && ev.Player == npc)
            {
                if(npc.Health - ev.Amount <= 0 || ev.IsInstantKill)
                {
                    npcIsDead = true;
                }

                player.Hurt(ev.Amount, ev.DamageHandler.Type);
            }

            if (ev.Player == player)
            {
                if (ev.Player.Health - ev.Amount <= 0 || ev.IsInstantKill)
                {
                    if (npcIsDead)
                    {
                        DroneItem.StopDrone(ev.Player);
                    } 
                    else
                    {
                        ev.IsAllowed = false;
                        player.Position = npc.Position;
                        player.Health = npc.Health;
                        RestorePlayerInventory();
                        DroneItem.StopDrone(ev.Player);
                    }
                }
            }
        }

        private void SpawnNpc()
        {
            npc = Npc.Spawn(player.Nickname, player.Role, player.Position);
            npc.Health = player.Health;
            npc.Scale = player.Scale;

            foreach(Item item in player.Items)
            {
                npc.AddItem(item.Clone());
            }

            foreach (var ammo in player.Ammo)
            {
                npc.SetAmmo((AmmoType)ammo.Key, ammo.Value);
            }
        }

        private void PutPlayerOnDrone()
        {
            playerSavedItems.Clear();
            foreach (Item item in player.Items)
            {
                playerSavedItems.Add(item.Type);
            }

            playerSavedAmmo.Clear();
            foreach (var ammo in player.Ammo)
            {
                playerSavedAmmo.Add(ammo.Key, ammo.Value);
            }

            player.ClearInventory(true);
            player.Scale = Vector3.one * 0.1f;

            if (player.Role is FpcRole fpc)
            {
                fpc.Gravity = Vector3.zero;
            }
        }

        private void RestorePlayerInventory()
        {
            player.ClearInventory(true);
            foreach (ItemType type in playerSavedItems)
            {
                player.AddItem(type);
            }

            foreach (var ammo in playerSavedAmmo)
            {
                player.SetAmmo((AmmoType)ammo.Key, ammo.Value);
            }
        }
    }
}