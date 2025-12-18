using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Items.API.Features.SpawnPoints;
using System.Collections.Generic;
using System.Linq;
using static KE.Items.API.Features.SpawnPoints.PoseRoomSpawnPointHandler;

namespace KE.Items.API.Features
{
    public abstract class KECustomGrenade : CustomGrenade
    {
        public virtual float DamageModifier { get; set; } = 1f;


        protected override void SubscribeEvents()
        {

            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
        }

        public override uint Spawn(IEnumerable<SpawnPoint> spawnPoints, uint limit)
        {
            Log.Debug($"spawning {this.Name}");
            HashSet<SpawnPoint> spawns = spawnPoints.ToHashSet();
            uint num = 0;
            foreach (SpawnPoint spawnpoint in spawnPoints.Where(sp => sp is RoomSpawnPoint))
            {
                Pickup pickup;
                if (Exiled.Loader.Loader.Random.NextDouble() * 100.0 >= (double)spawnpoint.Chance || (limit != 0 && num >= limit))
                {
                    continue;
                }
                spawns.Remove(spawnpoint);
                RoomSpawnPoint room = spawnpoint as RoomSpawnPoint;
                ItemSpawn spawn = PoseRoomSpawnPointHandler.UseRandomPose(room.Room);

                Log.Debug($"spawning {this.Name} in {room.Room}");
                Log.Debug(room.Room + " : " + PoseRoomSpawnPointHandler.usablePose.Count(p => p.roomType == room.Room));

                if (spawn is not null)
                {
                    Log.Debug($"spawning custom pos");
                    pickup = Spawn(spawn.Position);
                }
                else
                {
                    Log.Error($"can't spawn in custom");
                    pickup = Spawn(spawnpoint.Position);
                }



                if (pickup is not null)
                {
                    num++;
                }


            }

            return base.Spawn(spawns, limit - num);
        }


        protected void InternalOnHurting(HurtingEventArgs ev)
        {

            //can't get the custom grenade
            /*if(ev.DamageHandler.Type == Exiled.API.Enums.DamageType.Explosion)
            {
                ev.Amount *= DamageModifier;
            }*/

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
