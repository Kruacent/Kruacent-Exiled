using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using KE.Utils.API;
using KE.Utils.Extensions;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.Surface.SupplyDrops
{
    public class SupplyDrop : IPosition
    {
        private static byte _scpSteal = 0;

        public const byte ScpStealLimit = 3;
        public const byte MaxSupplyDrop = 10;
        public static readonly TimeSpan TimeStaying = new TimeSpan(0, 1, 0);
        public static readonly TimeSpan TimeSpawn = new TimeSpan(0, 5, 0);
        public const float Radius = 7f;
        public const float RefreshRate = .1f;
        
        public bool Show
        {
            get
            {
                DayOfWeek day = DateTime.Now.DayOfWeek;

                if (day == DayOfWeek.Saturday)
                    return true;
                //starting at monday
                int idDay = (int)day +1;

                if ( idDay % 2 == 0 && NumberDrop % 2 != 0)
                {
                    return true;
                }
                if (idDay % 2 != 0 && NumberDrop % 2 == 0)
                {
                    return true;
                }

                return false;
            }
        }
        public Vector3 Position { get; }
        public int NumberDrop => list.FindIndex(s => s == this);

        private static Stopwatch _spawnTime;
        private static TimeSpan _nextSpawn;
        private static List<SupplyDrop> list = new();
        private HashSet<Primitive> primitives = new();
        private bool _detectingSomeone = false;

        public RoleTypeId SideClaimed { get; private set; } = RoleTypeId.None;
        public Player PlayerClaimed { get; private set; }

        public static readonly string CassieMessageDrop = "Drop in surface";
        public static readonly string CassieTooMuchStealing = "";

        private static CoroutineHandle _handle;
        public SupplyDrop(Vector3 position)
        {
            list.Add(this);
            Position = position;

            //Model
            primitives.Add(Primitive.Create(PrimitiveType.Cube,position,null,Vector3.one,false));
            //radius of pickup
            var pr = Primitive.Create(PrimitiveType.Sphere, Position, null, new(Radius, Radius, Radius), false, new(0, 1, 0, .30f));
            pr.Collidable = false;
            primitives.Add(pr);
            if (Show)
            {
                Cassie.Message(CassieMessageDrop);
            }

            foreach (var p in primitives)
            {
                p.Spawn();
            }

            Timing.RunCoroutine(Detecting());
        }

        public static void OnRoundStarted()
        {
            _spawnTime = Stopwatch.StartNew();
            //_nextSpawn = TimeSpawn;

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
                    SpawnRandom();
                }
                notmax = list.Count <= MaxSupplyDrop;
                yield return Timing.WaitForSeconds(RefreshRate);
            }
        }

        private static void SpawnRandom()
        {
            //Todo random lol
            Vector3 spawnloc = new(19, 290, -44);


            Log.Debug($"spawning drop at {spawnloc}");
            SupplyDrop soup = new(spawnloc);


        }
        public void Destroy()
        {
            float timeExplode = 10;
            var grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
            grenade.ScpDamageMultiplier = 1;
            grenade.BurnDuration = timeExplode;
            grenade.SpawnActive(Position + Vector3.up);

            Timing.CallDelayed(timeExplode, () =>
            {
                foreach(var p in primitives)
                {
                    p.Destroy();
                }
            });

        }

        private IEnumerator<float> Detecting()
        {
            var s = Stopwatch.StartNew();
            _detectingSomeone = true;
            while (_detectingSomeone && s.Elapsed < TimeStaying)
            {
                yield return Timing.WaitForSeconds(RefreshRate);
                foreach (Player p in Player.List.Where(p=> p.IsAlive))
                {
                    if (OtherUtils.IsInCircle(p.Position,Position,Radius))
                    {
                        Effect(p);
                        _detectingSomeone = false;
                        Log.Debug($"Player {p.Id} got the supply drop");
                        //break;
                    }
                }
            }
            _detectingSomeone = false;
            Destroy();
        }



        private void Effect(Player p)
        {

            //todo add trapped drop (explode)
            SideClaimed = p.Role;
            PlayerClaimed = p;

            if (p.IsScp)
            {
                BuffScps();
            }
            else
            {
                SpawnLoot(p);
            }


        }

        private void SpawnLoot(Player p)
        {
            p.AddItem(ItemType.GunCrossvec);
            p.AddAmmo(AmmoType.Nato9, 50);
        }

        private void BuffScps()
        {
            foreach(Player p in Player.List.Where(p => p.IsScp))
            {
                float healthAdded = p.MaxHealth * 1.3f;
                p.MaxHealth += healthAdded;
                p.Health += healthAdded;
                
            }
            _scpSteal++;

            if (_scpSteal >= ScpStealLimit)
            {
                Cassie.Message(CassieTooMuchStealing);
                Warhead.Start(true, true);
                _spawnTime.Stop();
                Timing.KillCoroutines(_handle);
            }
        }



    }
}
