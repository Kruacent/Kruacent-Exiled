using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Utils.Extensions;
using KruacentExiled.CustomItems.API.Interface;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KruacentExiled.CustomItems.Items.ItemEffects
{
    public class TPGrenadaEffect : CustomItemEffect
    {
        private List<Player> effectedPlayers = new List<Player>();
        public HashSet<RoleTypeId> BlacklistedRoles { get; set; } = new HashSet<RoleTypeId>() { RoleTypeId.Scp173, RoleTypeId.Scp106, RoleTypeId.Scp049, RoleTypeId.Scp096, RoleTypeId.Scp3114, RoleTypeId.Scp0492, RoleTypeId.Scp939 };

        public HashSet<RoomType> BlacklistedRooms { get; } = new HashSet<RoomType>()
        {
            RoomType.HczTestRoom,
            RoomType.HczTesla,
            RoomType.Lcz173,
        };

        public override void Effect(UsedItemEventArgs ev)
        {
            OnExploding(new HashSet<Player>() { ev.Player });
        }
        public override void Effect(DroppingItemEventArgs ev)
        {
            OnExploding(new HashSet<Player>() { ev.Player });
        }

        public override void Effect(ExplodingGrenadeEventArgs ev)
        {
            OnExploding(ev.TargetsToAffect, ev.Projectile);
        }



        private void OnExploding(HashSet<Player> targets, EffectGrenadeProjectile projectile = null)
        {

            effectedPlayers = ListPool<Player>.Pool.Get();
            foreach (Player player in targets)
            {
                if (BlacklistedRoles.Contains(player.Role))
                    continue;
                try
                {
                    bool line;
                    if (projectile == null)
                    {
                        line = true;
                    }
                    else
                    {
                        line = Physics.Linecast(projectile.Transform.position, player.Position);
                    }
                        

                    if (line)
                    {
                        effectedPlayers.Add(player);
                        Room destinationRoom;

                        bool isLucky = KECustomRole.Get(player).OfType<ILucky>().Any(r => r.LuckProfile == LuckProfile.UltimateLucky);
                        Log.Debug(isLucky);

                        if (isLucky)
                        {
                            destinationRoom = GetLuckyRoom(player);
                        }
                        else
                        {
                            destinationRoom = RandomRoom();
                        }

                        player.Teleport(RandomRoom().GetValidPosition());
                    }
                }
                catch (Exception exception)
                {
                    Log.Error($"{nameof(OnExploding)} error: {exception}");
                }
            }
        }

        private Room RandomRoom()
        {
            Room room = Room.List.GetRandomValue((r) => !BlacklistedRooms.Contains(r.Type) && r.IsSafe());
            if (Warhead.IsDetonated)
            {
                return ZoneType.Surface.RandomSafeRoom();
            }

            if (Exiled.API.Features.Map.IsLczDecontaminated)
            {
                float random = UnityEngine.Random.value;
                Log.Debug($"random={random}");
                if (random <= 0.33f)
                {
                    room = ZoneType.HeavyContainment.RandomSafeRoom();
                }
                else if (random > 0.33f && random <= 0.66f)
                {
                    room = ZoneType.Entrance.RandomSafeRoom();
                }
                else
                {
                    room = ZoneType.Surface.RandomSafeRoom();
                }
                    
            }

            Log.Debug($"roomZone={room.Zone}");
            return room;
        }

        public Room GetLuckyRoom(Player luckyPlayer)
        {
            List<Room> validRooms = GetValidRooms();

            if (validRooms.Count == 0)
            {
                return Room.List.First(r => r.Zone == ZoneType.Surface);
            }

            List<Player> enemies = new List<Player>();
            List<Player> teammates = new List<Player>();

            foreach (Player p in Player.List)
            {
                if (!p.IsAlive) continue;
                if (p == luckyPlayer) continue;

                if (p.Role.Side != luckyPlayer.Role.Side)
                {
                    enemies.Add(p);
                }
                else
                {
                    teammates.Add(p);
                }
            }

            Room bestRoom = validRooms[0];
            float bestScore = -9999f;

            foreach (Room room in validRooms)
            {
                float score = 0f;
                float distanceToClosestEnemy = GetClosestDistance(room.Position, enemies);

                if (distanceToClosestEnemy < 15f && enemies.Count > 0)
                {
                    continue;
                }

                score += distanceToClosestEnemy;

                if (teammates.Count > 0)
                {
                    float distanceToTeammate = GetClosestDistance(room.Position, teammates);

                    score -= (distanceToTeammate * 2f);
                }

                if (score > bestScore)
                {
                    bestScore = score;
                    bestRoom = room;
                }
            }

            return bestRoom;
        }

        private List<Room> GetValidRooms()
        {
            List<Room> validRooms = new List<Room>();

            if (Warhead.IsDetonated)
            {
                validRooms.AddRange(Room.List.Where(r => r.Zone == ZoneType.Surface && r.IsSafe()));
                return validRooms;
            }

            foreach (Room r in Room.List)
            {
                if (BlacklistedRooms.Contains(r.Type))
                {
                    continue;
                }

                if (!r.IsSafe())
                {
                    continue;
                }

                if (Exiled.API.Features.Map.IsLczDecontaminated && r.Zone == ZoneType.LightContainment)
                {
                    continue;
                }

                validRooms.Add(r);
            }

            return validRooms;
        }

        private float GetClosestDistance(Vector3 roomPosition, List<Player> players)
        {
            if (players.Count == 0)
            {
                return 9999f;
            }

            float minDistance = float.MaxValue;
            foreach (Player p in players)
            {
                float distance = Vector3.Distance(roomPosition, p.Position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }

            return minDistance;
        }
    }
}
