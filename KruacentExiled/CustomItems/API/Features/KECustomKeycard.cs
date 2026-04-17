using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Features.SpawnPoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KruacentExiled.CustomItems.API.Features.SpawnPoints.PoseRoomSpawnPointHandler;

namespace KruacentExiled.CustomItems.API.Features
{
    public abstract class KECustomKeycard : CustomKeycard
    {

        public override KeycardPermissions Permissions { get; set; } = KeycardPermissions.None;


        public override uint Spawn(IEnumerable<SpawnPoint> spawnPoints, uint limit)
        {
            Log.Debug($"spawning {Name}");
            HashSet<SpawnPoint> spawns = spawnPoints.ToHashSet();
            uint num = 0;
            foreach (SpawnPoint spawnpoint in spawnPoints.Where(sp => sp is RoomSpawnPoint))
            {
                Pickup pickup;
                if (Exiled.Loader.Loader.Random.NextDouble() * 100.0 >= (double)spawnpoint.Chance || limit != 0 && num >= limit)
                {
                    continue;
                }
                spawns.Remove(spawnpoint);
                RoomSpawnPoint room = spawnpoint as RoomSpawnPoint;
                ItemSpawn spawn = UseRandomPose(room.Room);

                Log.Debug($"spawning {Name} in {room.Room}");
                Log.Debug(room.Room + " : " + UsablePoses.Count(p => p.roomType == room.Room));

                if (spawn != null)
                {
                    Log.Debug($"spawning custom pos");
                    pickup = Spawn(spawn.Position);
                }
                else
                {
                    Log.Error($"can't spawn ({Name}) in custom ({room.Room})");
                    pickup = Spawn(spawnpoint.Position);
                }



                if (pickup != null)
                {
                    num++;
                }


            }

            return base.Spawn(spawns, limit - num);
        }



        protected override void ShowPickedUpMessage(Player player)
        {
            KECustomItem.Message(this, player, true);
        }

        protected override void ShowSelectedMessage(Player player)
        {
            KECustomItem.Message(this, player);
        }

    }
}
