using Exiled.API.Features;
using Exiled.API.Features.Components;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Features;
using InventorySystem.Items.ThrowableProjectiles;
using KE.Items.API.Events;
using KE.Items.API.Features.SpawnPoints;
using KE.Utils.API.Features;
using System.Collections.Generic;
using System.Linq;
using static KE.Items.API.Features.SpawnPoints.PoseRoomSpawnPointHandler;

namespace KE.Items.API.Features
{
    public abstract class KECustomGrenade : KECustomItem
    {
        public virtual float DamageModifier { get; } = 1f;
        public abstract bool ExplodeOnCollision { get; }
        public abstract float FuseTime { get;  }
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ThrowingRequest += OnInternalThrowingRequest;
            Exiled.Events.Handlers.Player.ThrownProjectile += OnInternalThrownProjectile;
            Exiled.Events.Handlers.Map.ExplodingGrenade += OnInternalExplodingGrenade;
            Exiled.Events.Handlers.Map.ChangedIntoGrenade += OnInternalChangedIntoGrenade;
            ExplodeEvent.ExplodeDestructible += OnInternalExplodeDestructible;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ThrowingRequest -= OnInternalThrowingRequest;
            Exiled.Events.Handlers.Player.ThrownProjectile -= OnInternalThrownProjectile;
            Exiled.Events.Handlers.Map.ExplodingGrenade -= OnInternalExplodingGrenade;
            Exiled.Events.Handlers.Map.ChangedIntoGrenade -= OnInternalChangedIntoGrenade;
            ExplodeEvent.ExplodeDestructible -= OnInternalExplodeDestructible;
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

        public virtual bool Check(Projectile grenade)
        {
            if (grenade != null)
            {
                return base.TrackedSerials.Contains(grenade.Serial);
            }

            return false;
        }

        public virtual bool Check(ExplosionGrenade explosionGrenade)
        {
            return Check(Pickup.Get(explosionGrenade));
        }
        private void OnInternalExplodeDestructible(OnExplodeDestructibleEventsArgs ev)
        {

            if (Check(ev.ExplosionGrenade))
            {
                if(ev.Damage > 0f)
                {
                    ev.Damage *= DamageModifier;
                }

                OnExplodeDestructible(ev);
            }
        }


        private void OnInternalThrowingRequest(ThrowingRequestEventArgs ev)
        {
            if (Check(ev.Item))
            {
                OnThrowingRequest(ev);
            }
        }
        private void OnInternalThrownProjectile(ThrownProjectileEventArgs ev)
        {
            if (Check(ev.Throwable))
            {
                OnThrownProjectile(ev);
                if (ev.Projectile is TimeGrenadeProjectile timeGrenadeProjectile)
                {
                    timeGrenadeProjectile.FuseTime = FuseTime;
                }

                if (ExplodeOnCollision)
                {
                    ev.Projectile.GameObject.AddComponent<CollisionHandler>().Init((ev.Player ?? Server.Host).GameObject, ev.Projectile.Base);
                }
            }
        }
        private void OnInternalExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {
            if (Check(ev.Projectile))
            {
                OnExplodingGrenade(ev);
            }
        }
        private void OnInternalChangedIntoGrenade(ChangedIntoGrenadeEventArgs ev)
        {


            if (Check(ev.Pickup))
            {
                if (ev.Projectile is TimeGrenadeProjectile timeGrenadeProjectile)
                {
                    timeGrenadeProjectile.FuseTime = FuseTime;
                }

                OnChangedIntoGrenade(ev);
                if (ExplodeOnCollision)
                {
                    ev.Projectile.GameObject.AddComponent<CollisionHandler>().Init((ev.Pickup.PreviousOwner ?? Server.Host).GameObject, ev.Projectile.Base);
                }
            }

        }


        protected virtual void OnExplodeDestructible(OnExplodeDestructibleEventsArgs ev)
        {

        }
        protected virtual void OnThrowingRequest(ThrowingRequestEventArgs ev)
        {

        }
        protected virtual void OnThrownProjectile(ThrownProjectileEventArgs ev)
        {

        }
        protected virtual void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {

        }
        protected virtual void OnChangedIntoGrenade(ChangedIntoGrenadeEventArgs ev)
        {

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
