using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Utils.Extensions;
using KruacentExiled.CustomItems.API.Interface;
using PlayerRoles;
using System;
using System.Collections.Generic;
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
    }
}
