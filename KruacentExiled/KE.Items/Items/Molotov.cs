using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using KE.Items.Interface;
using Player = Exiled.API.Features.Player;
using MEC;
using UnityEngine;
using PlayerRoles;

namespace KE.Items.Items
{
    [CustomItem(ItemType.GrenadeFlash)]
    public class Molotov : CustomGrenade, ILumosItem
    {
        public override uint Id { get; set; } = 1049;
        public override string Name { get; set; } = "Cocktail Molotov";
        public override string Description { get; set; } = "ARSON";
        public override float Weight { get; set; } = 0.65f;
        public override float FuseTime { get; set; } = 5f;
        public override bool ExplodeOnCollision { get; set; } = true;
        public float DamageModifier { get; set; } = 0f;
        public UnityEngine.Color Color { get; set; } = UnityEngine.Color.yellow;
        private const float RefreshRate = 0.5f;
        private const float Duration = 20f;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 2,
            LockerSpawnPoints = new List<LockerSpawnPoint>
            {
                new LockerSpawnPoint()
                {
                    Chance = 75,
                    UseChamber = true,
                    Type = LockerType.Misc,
                    Zone = ZoneType.Entrance,
                },
                new LockerSpawnPoint()
                {
                    Chance = 50,
                    UseChamber = true,
                    Type = LockerType.Misc,
                    Zone = ZoneType.LightContainment,
                },
                new LockerSpawnPoint()
                {
                    Chance = 50,
                    UseChamber = true,
                    Type = LockerType.Misc,
                    Zone = ZoneType.HeavyContainment,
                },
            },

            RoomSpawnPoints = new List<RoomSpawnPoint>
            {
                new RoomSpawnPoint()
                {
                    Chance = 75,
                    Room = RoomType.LczGlassBox,
                },
                new RoomSpawnPoint()
                {
                    Chance = 50,
                    Room = RoomType.HczNuke,
                },
            },
        };

        protected override void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            float cylinderSize = 5;

            ev.TargetsToAffect.Clear();

            Player playerThrowingGrenade = ev.Player;
            Vector3 molotovPosition = ev.Position;
            Primitive wall = Primitive.Create(PrimitiveType.Cylinder, molotovPosition, null, new Vector3(cylinderSize, 0.01f, cylinderSize), true);
            wall.Collidable = false;
            wall.Visible = true;

            wall.Color = Color.red;

            var coroutineHandler = Timing.RunCoroutine(DamageInMolotovZone(wall.Position, cylinderSize, playerThrowingGrenade));

            Timing.CallDelayed(Duration, () => {
                wall.UnSpawn();
                Timing.KillCoroutines(coroutineHandler);
                wall.Destroy();
            });          
        }

        private IEnumerator<float> DamageInMolotovZone(Vector3 wallPosition, float cylinderSize, Player playerThrowingGrenade)
        {
            // Dictionary that stores the time each player has spent inside the zone (in seconds).
            Dictionary<Player, float> playerTimeInZone = new Dictionary<Player, float>();

            while (true)
            {
                foreach (Player player in Player.List)
                {
                    if (IsPlayerInZone(player, wallPosition, cylinderSize))
                    {
                        if (Exiled.API.Features.Server.FriendlyFire || playerThrowingGrenade.Role.Team != player.Role.Team || playerThrowingGrenade == player)
                        {
                            if (player.IsHuman || player.Role == RoleTypeId.Scp0492)
                            {
                                if (playerTimeInZone.ContainsKey(player))
                                {
                                    // increase time each frame.
                                    playerTimeInZone[player] += Time.deltaTime; 
                                }
                                else
                                {
                                    // Init the time in dictionnary of the player.
                                    playerTimeInZone[player] = Time.deltaTime; 
                                }

                                // time of player spend inside of molotov zone.
                                float timeInZone = playerTimeInZone[player];

                                // Beginning it willbe 5dm/s, after it will be linearly higher the damage until 20dm/s. 
                                float damage = Mathf.Lerp(2.5f, 10f, timeInZone / 20f);

                                // double damage if it's zombie cuz it has more hp.
                                if (player.Role == RoleTypeId.Scp0492)
                                {
                                    damage *= 2.5f;
                                }

                                player.Hurt(damage, DamageType.Bleeding);
                            }
                            else if (player.IsScp)
                            {
                                player.Hurt(player.Health / 150, DamageType.Bleeding);
                            }

                        }
                    }
                }

                yield return Timing.WaitForSeconds(RefreshRate);
            }
        }


        private bool IsPlayerInZone(Player player, Vector3 zonePosition, float radius)
        {
            float distance = Vector3.Distance(new Vector3(player.Position.x, 0, player.Position.z),
                                               new Vector3(zonePosition.x, 0, zonePosition.z));
            return distance <= (radius/2) ;
        }
    }
}