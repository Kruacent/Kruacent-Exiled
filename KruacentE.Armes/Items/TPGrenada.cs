
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Pools;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using PlayerRoles;
using UnityEngine;

namespace ArmeKruacent.Items
{
    //grenade qui tp
    public class TPGrenada : CustomGrenade
    {
        private List<Player> effectedPlayers = new List<Player>();
        public override uint Id { get; set; } = 20;
        public override string Name { get; set; } = "Teleportation Grenade";
        public override string Description { get; set; } = "This grenade does 0 damage but teleport nearby players in a random place (does work in other dimension ;3 )";
        public override float Weight { get; set; } = 0.65f;
        public override float FuseTime { get; set; } = 1.5f;
        public override bool ExplodeOnCollision { get; set; } = true;
        public float DamageModifier { get; set; } = 0.05f;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 5,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new DynamicSpawnPoint()
                {
                    Chance = 50,
                    Location = SpawnLocationType.InsideLocker,
                },
                new DynamicSpawnPoint()
                {
                    Chance = 50,
                    Location = SpawnLocationType.InsideHczArmory,
                },
                new DynamicSpawnPoint()
                {
                    Chance =2,
                    Location = SpawnLocationType.Inside914,
                },
                new DynamicSpawnPoint()
                {
                    Chance=50,
                    Location = SpawnLocationType.Inside049Armory,
                },
                new DynamicSpawnPoint()
                {
                    Chance=50,
                    Location = SpawnLocationType.InsideLczArmory,
                }
            },

        };

        [Description("What roles will not be able to be affected by Implosion Grenades. Keeping SCP-173 on this list is highly recommended.")]
        public HashSet<RoleTypeId> BlacklistedRoles { get; set; } = new HashSet<RoleTypeId>() { RoleTypeId.Scp173, RoleTypeId.Scp106,RoleTypeId.Scp049, RoleTypeId.Scp096,RoleTypeId.Scp3114,RoleTypeId.Scp0492,RoleTypeId.Scp939 };

        protected override void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            ev.IsAllowed = false;
            List<Player> copiedList = new List<Player>();
            foreach (Player player in ev.TargetsToAffect)
            {
                copiedList.Add(player);
            }

            ev.TargetsToAffect.Clear();

            effectedPlayers = ListPool<Player>.Pool.Get();
            foreach (Player player in copiedList)
            {
                if (BlacklistedRoles.Contains(player.Role))
                    continue;
                try
                {

                    bool line = Physics.Linecast(ev.Projectile.Transform.position, player.Position); 
                    if (line)
                    {
                        effectedPlayers.Add(player);
                        player.Teleport(RandomRoom());
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
            Room room = Room.Random();
            if (Warhead.IsDetonated)
            {
                return Room.Random(ZoneType.Surface);
            }

            if(Map.IsLczDecontaminated)
            {
                float random = UnityEngine.Random.value;
                Log.Debug($"random={random}");
                if (random <= 0.33f)
                {
                    return Room.Random(ZoneType.HeavyContainment);
                }
                if(random > 0.33f && random <= 0.66f)
                {
                    return Room.Random(ZoneType.Entrance);
                }
                return Room.Random(ZoneType.Surface);
            }
            Log.Debug($"roomZone={room.Zone}");
            return room;
        }
    }

    //bonbon quand tu manges ça donne une armes random
    /*public class RandomWeaponCandy : CustomItem
    {

    }*/
}
