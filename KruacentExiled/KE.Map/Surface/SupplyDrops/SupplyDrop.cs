using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using KE.Utils.API;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace KE.Map.Surface.SupplyDrops
{
    public class SupplyDrop : IPosition
    {

        public static bool IsActivated => MainPlugin.Configs.SupplyDropEnabled;

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
        private static List<SupplyDrop> list = new List<SupplyDrop>();
        public static IReadOnlyCollection<Vector3> SpawnPositions = new List<Vector3>()
        {
            new Vector3(-15,292,-39), //spawn chaos
            new Vector3(40,301,-52), // above the gate
            new Vector3(138,295,-64), //behind mtf spawn at the unopenable gate
            new Vector3(124,289,22) //escape
        };
        private HashSet<Primitive> primitives = new HashSet<Primitive>();
        private bool _detectingSomeone = false;

        public RoleTypeId SideClaimed { get; private set; } = RoleTypeId.None;
        public Player PlayerClaimed { get; private set; }

        public static readonly string CassieMessageDrop = "Drop in surface";
        public static readonly string CassieTooMuchStealing = "Too Much Supplys Taken By SCPs";
        public static readonly string CassieTooMuchStealingSub = "Too Much Supplies Taken By SCPs";

        private static SupplyDrop CurrentDrop = null;
        private static CoroutineHandle _handle;
        public SupplyDrop(Vector3 position)
        {
            list.Add(this);
            Position = position;

            //Model
            primitives.Add(Primitive.Create(PrimitiveType.Cube,position,null,Vector3.one,false));
            //radius of pickup
            var pr = Primitive.Create(PrimitiveType.Sphere, Position, null, new Vector3(Radius, Radius, Radius), false, new Color(0, 1, 0, .30f));
            pr.Collidable = false;
            primitives.Add(pr);


            if (Show)
            {
                //Cassie.Message(CassieMessageDrop);
            }

            foreach (var p in primitives)
            {
                p.Spawn();
            }
            CurrentDrop = this;
            Timing.RunCoroutine(Detecting());
        }




        public static void SubscribeEvents()
        {
            if (!IsActivated) return;

            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }

        public static void UnsubscribeEvents()
        {
            if (!IsActivated) return;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        }

        public static void OnRoundStarted()
        {
            _spawnTime = Stopwatch.StartNew();
            _nextSpawn = TimeSpawn;

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

        private static void SpawnRandom()
        {
            //Todo random lol
            Vector3 spawnloc = SpawnPositions.GetRandomValue();
            Log.Info($"spawning drop at {spawnloc}");
            SupplyDrop soup = new SupplyDrop(spawnloc);


        }

        public void Destroy()
        {
            float timeExplode = 10;
            var grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
            grenade.ScpDamageMultiplier = 1;
            grenade.BurnDuration = timeExplode;
            grenade.SpawnActive(Position + Vector3.up);

            foreach (var p in primitives)
            {
                p.Destroy();
            }
            CurrentDrop = null;


        }

        private IEnumerator<float> Detecting()
        {
            var s = Stopwatch.StartNew();
            var playerAlreadyIn = Player.List.Where(p => InRadius(p)).ToHashSet();

            _detectingSomeone = true;
            while (_detectingSomeone && s.Elapsed < TimeStaying)
            {
                yield return Timing.WaitForSeconds(RefreshRate);
                foreach (Player p in Player.List.Where(p=> p.IsAlive && p.Role != RoleTypeId.Scp106))
                {
                    if (!playerAlreadyIn.Contains(p) && InRadius(p))
                    {
                        Effect(p);
                        _detectingSomeone = false;
                        
                    }
                    if (playerAlreadyIn.Contains(p))
                    {
                        if (!InRadius(p))
                        {
                            playerAlreadyIn.Remove(p);
                        }
                    }
                }
            }
            _detectingSomeone = false;
            Destroy();
        }

        private bool InRadius(Player p) => OtherUtils.IsInCircle(p.Position, Position, Radius / 2);
         

        private void Effect(Player p)
        {
            Log.Debug($"Player {p.Id} got the supply drop");
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

        private void BuffScps()
        {
            Log.Debug("scps got it!");
            foreach(Player p in Player.List.Where(p => p.IsScp && p.Role != RoleTypeId.Scp106))
            {
                float healthAdded = p.MaxHealth * 1.2f;
                p.MaxHealth += healthAdded;
                p.Health += healthAdded;
                
            }
            _scpSteal++;

            if (_scpSteal >= ScpStealLimit)
            {
                //Cassie.MessageTranslated(CassieTooMuchStealing,CassieTooMuchStealingSub);
                Warhead.Start(true, true);
                _spawnTime.Stop();
                Timing.KillCoroutines(_handle);
            }
        }



    }
}
