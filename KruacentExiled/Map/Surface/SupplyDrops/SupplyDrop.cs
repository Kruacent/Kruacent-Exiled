using Achievements.Handlers;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using KE.Utils.API;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using KruacentExiled.Map;
using KruacentExiled.Map.Surface.SupplyDrops.SupplyDropTimeModifier;
using LabApi.Events.Handlers;
using MEC;
using PlayerRoles;
using ProjectMER.Features.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace KruacentExiled.Map.Surface.SupplyDrops
{
    public class SupplyDrop : IPosition
    {

        public static bool IsActivated => MainPlugin.Configs.SupplyDropEnabled;

        /// <summary>
        /// The maximum <see cref="SupplyDrop"/> that can spawn in one round
        /// </summary>
        public const byte MaxSupplyDrop = 10;
        
        /// <summary>
        /// Time between each random spawn
        /// </summary>
        public static readonly TimeSpan TimeSpawn = new TimeSpan(0, 5, 0);



        public static readonly TimeSpan DefaultTimeSpawn = new TimeSpan(0, 5, 0);
        public static readonly TimeSpan MinimumTimeSpawn = new TimeSpan(0, 0, 30);
        public static readonly TimeSpan MaxTimeSpawn = new TimeSpan(1, 0, 0);
        public static readonly TimeSpan CurrentTimeSpawn = new TimeSpan(0, 5, 0);

        /// <summary>
        /// The time after someone pickup the <see cref="SupplyDrop"/> before it explodes
        /// </summary>
        public static readonly TimeSpan TimeStay = new TimeSpan(0, 0, 20);
        public const float Radius = 7f;
        public const float RefreshRate = 5f;
        
        /// <summary>
        /// The position of the <see cref="SupplyDrop"/>
        /// </summary>
        public Vector3 Position { get; }
        public int NumberDrop => list.FindIndex(s => s == this);

        private static Stopwatch _spawnTime;
        private static TimeSpan _nextSpawn;



        public static TimeSpan Clamp(TimeSpan value, TimeSpan min, TimeSpan max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }

            return value;
        }

        public static void ChangeTime(TimeSpan delta)
        {
            _nextSpawn = Clamp(_nextSpawn + delta, MinimumTimeSpawn, MaxTimeSpawn);
        }


        private static List<SupplyDrop> list = new List<SupplyDrop>();
        public static IReadOnlyCollection<Vector3> SpawnPositions = new List<Vector3>()
        {
            new Vector3(-15,292,-39), //spawn chaos
            new Vector3(40,301,-52), // above the central gate
            new Vector3(138,295,-64), //behind mtf spawn at the unopenable gate
            new Vector3(124,289,22) //escape
        };
        private HashSet<Primitive> primitives;

        public RoleTypeId SideClaimed { get; private set; } = RoleTypeId.None;
        public Player PlayerClaimed { get; private set; }

        private SupplyCollisionHandler handler;


        /// <summary>
        /// Current spawned <see cref="SupplyDrop"/>
        /// </summary>
        private static SupplyDrop CurrentDrop = null;
        private static CoroutineHandle _handle;

        
        /// <summary>
        /// Create and spawn a <see cref="SupplyDrop"/> at a position
        /// </summary>
        /// <param name="position"></param>
        private SupplyDrop(Vector3 position)
        {
            list.Add(this);
            Position = position;
            primitives = new HashSet<Primitive>();
            //Model
            primitives.Add(Primitive.Create(PrimitiveType.Cube,position,null,Vector3.one,false));
            //radius of pickup
            var pr = Primitive.Create(PrimitiveType.Sphere, Position, null, new Vector3(Radius, Radius, Radius), false, new Color(0, 1, 0, .30f));
            pr.Collidable = false;
            primitives.Add(pr);


            foreach (var p in primitives)
            {
                p.Spawn();
            }

            GameObject customEscapePrimitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            customEscapePrimitive.isStatic = true;
            customEscapePrimitive.transform.position = Position;
            customEscapePrimitive.transform.localScale = Vector3.one * Radius;
            customEscapePrimitive.GetComponent<Collider>().isTrigger = true;
            handler = customEscapePrimitive.AddComponent<SupplyCollisionHandler>();

            handler.Init(this);


            CurrentDrop = this;

        }




        public static void SubscribeEvents()
        {
            if (!IsActivated) return;

            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            SupplyDropTimeModifierBase.InternalSubscribeEvents();
        }

        public static void UnsubscribeEvents()
        {
            if (!IsActivated) return;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            SupplyDropTimeModifierBase.InternalUnsubscribeEvents();

            if (_handle.IsValid)
            {
                Timing.KillCoroutines(_handle);
            }
        }

        public static void OnRoundStarted()
        {
            _spawnTime = Stopwatch.StartNew();
            _nextSpawn = DefaultTimeSpawn;

            _handle = Timing.RunCoroutine(Loop());
        }

        private static IEnumerator<float> Loop()
        {

            bool notmax = true;
            while (notmax)
            {
                if (_spawnTime.Elapsed > _nextSpawn)
                {
                    _nextSpawn += TimeSpawn;
                    if(CurrentDrop == null)
                    {
                        SpawnRandom();
                    }
                        
                    Log.Info("next spawn " + _nextSpawn);
                }
                notmax = list.Count <= MaxSupplyDrop;
                yield return Timing.WaitForSeconds(RefreshRate);
            }
        }

        /// <summary>
        /// Spawn a <see cref="SupplyDrop"/> at a random position at Surface
        /// </summary>
        public static SupplyDrop SpawnRandom()
        {
            Vector3 spawnloc = SpawnPositions.GetRandomValue();
            Log.Info($"spawning drop at {spawnloc}");
            return new SupplyDrop(spawnloc);

        }


        /// <summary>
        /// Destroy this <see cref="SupplyDrop"/>
        /// </summary>
        /// <param name="spawnGrenade">whenever if it spawned a primed grenade when destroyed</param>
        public void Destroy(bool spawnGrenade = true)
        {
            if (spawnGrenade)
            {
                float timeExplode = 10;
                var grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                grenade.ScpDamageMultiplier = 1;
                grenade.BurnDuration = timeExplode;
                grenade.SpawnActive(Position + Vector3.up);

            }

            foreach (var p in primitives)
            {
                p.Destroy();
            }

            handler.Destroy();
            
            CurrentDrop = null;


        }


        /// <summary>
        /// Check if a <see cref="Player"/> can interact with the <see cref="SupplyDrop"/>
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool CheckPlayer(Player player)
        {
            return player.IsAlive && !alreadyUsed;
        }

        private bool alreadyUsed;

        public void TryEffect(Player p)
        {
            if (!CheckPlayer(p))
            {
                return;
            }
            Log.Debug($"Player {p.Id} got the supply drop");
            //todo add trapped drop (explode)



            SideClaimed = p.Role;
            PlayerClaimed = p;

            if (!SCPTeam.IsSCP(p.ReferenceHub))
            {
                SpawnLoot(p);
            }

            alreadyUsed = true;


            Timing.CallDelayed((float)TimeStay.TotalSeconds, () =>
            {
                Destroy();
            });

        }

        private void SpawnLoot(Player p)
        {
            Faction playerFaction = p.Role.Team.GetFaction();
            Respawn.GrantInfluence(playerFaction, 20);
            Respawn.AdvanceTimer(playerFaction, 10);

            Log.Debug("human got it!");
            if (!p.HasItem(ItemType.GunCrossvec))
            {
                p.AddItem(ItemType.GunCrossvec);
            }

            p.AddAmmo(AmmoType.Nato9, 50);
        }





    }
}
