using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Items.API.Events;
using KE.Items.API.Features.SpawnPoints;
using KE.Utils.API.Features;
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
            ExplodeEvent.ExplodeDestructible += OnExplodeDestructible;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            ExplodeEvent.ExplodeDestructible -= OnExplodeDestructible;
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
                Log.Debug(room.Room + " : " + PoseRoomSpawnPointHandler.UsablePoses.Count(p => p.roomType == room.Room));

                if (spawn is not null)
                {
                    Log.Debug($"spawning custom pos");
                    pickup = Spawn(spawn.Position);
                }
                else
                {
                    Log.Error($"can't spawn ({Name}) in custom ({room.Room})");
                    pickup = Spawn(spawnpoint.Position);
                }



                if (pickup is not null)
                {
                    num++;
                }


            }

            return base.Spawn(spawns, limit - num);
        }


        private void OnExplodeDestructible(OnExplodeDestructibleEventsArgs ev)
        {

            Player player = Player.Get(ev.Destructible.NetworkId);
            KELog.Debug("damage =" + ev.Damage);
            if (!Check(Pickup.Get(ev.ExplosionGrenade))) return;
            if (ev.Damage < 0f) return;
            ev.Damage *= DamageModifier;
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
