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

namespace KE.Items.Items
{
    [CustomItem(ItemType.GrenadeFlash)]
    public class HealZone : CustomGrenade, ILumosItem
    {
        public override uint Id { get; set; } = 1411;
        public override string Name { get; set; } = "Heal Zone";
        public override string Description { get; set; } = "Allow to heal you and your ally";
        public override float Weight { get; set; } = 0.65f;
        public override float FuseTime { get; set; } = 5f;
        public override bool ExplodeOnCollision { get; set; } = true;
        public float DamageModifier { get; set; } = 0f;
        public UnityEngine.Color Color { get; set; } = UnityEngine.Color.green;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 3,
            LockerSpawnPoints = new List<LockerSpawnPoint>
            {
                new LockerSpawnPoint()
                {
                    Chance = 75,
                    UseChamber = true,
                    Type = LockerType.Medkit,
                    Zone = ZoneType.Entrance,
                },
                new LockerSpawnPoint()
                {
                    Chance = 50,
                    UseChamber = true,
                    Type = LockerType.Medkit,
                    Zone = ZoneType.LightContainment,
                },
                new LockerSpawnPoint()
                {
                    Chance = 100,
                    UseChamber = true,
                    Type = LockerType.Medkit,
                    Zone = ZoneType.HeavyContainment,
                },
            },

            RoomSpawnPoints = new List<RoomSpawnPoint>
            {
                new RoomSpawnPoint()
                {
                    Chance = 75,
                    Room = RoomType.HczHid,
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
            Vector3 healZonePosition = ev.Position;
            Primitive wall = Primitive.Create(PrimitiveType.Cylinder, healZonePosition, null, new Vector3(cylinderSize, 0.01f, cylinderSize), true);
            wall.Collidable = false;
            wall.Visible = true;

            wall.Color = Color.green;

            var coroutineHandler = Timing.RunCoroutine(HealZoneHeal(wall.Position, cylinderSize, playerThrowingGrenade));

            Timing.CallDelayed(20, () => {
                wall.UnSpawn();
                Timing.KillCoroutines(coroutineHandler);
                wall.Destroy();
            });          
        }

        private IEnumerator<float> HealZoneHeal(Vector3 wallPosition, float cylinderSize, Player playerThrowingGrenade)
        {
            while (true)
            {
                foreach (Player player in Exiled.API.Features.Player.List)
                {
                    // Check if a player is in the zone.
                    if (IsPlayerInZone(player, wallPosition, cylinderSize))
                    {
                        if(playerThrowingGrenade.Role.Team == player.Role.Team)
                        {
                            player.Heal(1);
                        }
                    }
                }

                // Waiting 0.5s before re-check.
                yield return Timing.WaitForSeconds(0.5f);
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