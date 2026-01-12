using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.DamageHandlers;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Items.API.Features;
using MapGeneration;
using MEC;
using PlayerRoles.FirstPersonControl;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KE.Items.Items
{
    [CustomItem(ItemType.KeycardJanitor)]
    public class ShieldBelt : KECustomItem
    {

        public class ShieldBeltStat
        {
            public static readonly float MaxCharge = 110;
            public static readonly float RechargeRatePerS = 13;
            public static readonly float TimeBroken = 50;
            public static readonly float Base = 20;
            public static readonly Vector3 MaxSize = Vector3.one * 2;

            private float currentCharge;
            private float timeRemaining;
            private bool recharging = false;

            private Player player;
            private Primitive primitive;

            private int i = 0;
            public void RechargeTick()
            {
                if (i % 50 == 0)
                {
                    Log.Debug("cur=" + currentCharge);
                    Log.Debug("time=" + timeRemaining);
                }
                i++;

                if (timeRemaining <= 0 && recharging)
                {
                    Log.Debug("recharged");
                    currentCharge = 20;
                    recharging = false;
                }

                if(currentCharge <= 0)
                {
                    Break();
                }

                if(primitive is not null)
                {
                    float percent = currentCharge / MaxCharge;

                    primitive.Scale = percent * MaxSize;

                }


                if (!recharging)
                {
                    if (!primitive.Visible)
                    {
                        primitive.Visible = true;
                    }

                    if (currentCharge != MaxCharge)
                    {
                        
                        float tempcharge = currentCharge + RechargeRatePerS * Time.deltaTime;
                        currentCharge = Mathf.Clamp(tempcharge, 0, MaxCharge);
                    }

                }
                else
                {
                    timeRemaining -= Time.deltaTime;
                    if(timeRemaining < 0)
                    {
                        timeRemaining = 0;
                    }
                    if (primitive.Visible)
                    {
                        primitive.Visible = false;
                    }
                }

                    
            }


            /// <summary>
            /// 
            /// </summary>
            /// <param name="damage"></param>
            /// <returns>remaining damage</returns>
            public float Damage(float damage)
            {
                

                currentCharge = Mathf.Clamp(currentCharge-damage, 0, MaxCharge);
                Log.Debug("cur=" + currentCharge);
                Log.Debug("time=" + timeRemaining);
                if (currentCharge == 0)
                {
                    return damage;
                }

                return 0;

            }


            public void Break()
            {
                
                if (!recharging)
                {
                    Log.Debug("breakign");
                    timeRemaining = TimeBroken;
                    currentCharge = 0;
                    recharging = true;
                    player.PlayShieldBreakSound();
                }
                
            }


            public bool IsActive
            {
                get
                {
                    return currentCharge > 0;
                }
            }
            public bool IsRecharging
            {
                get
                {
                    return recharging;
                }
            }
            private Primitive CreatePrimitive(Player player)
            {
                Primitive prim = Primitive.Create(null, null, null, false);

                prim.Collidable = false;
                prim.Visible = true;
                prim.Transform.parent = player.ReferenceHub.transform;
                prim.Transform.localPosition = Vector3.zero;
                prim.Scale = MaxSize;
                prim.Color = new Color32(50, 50, 50, 100);
                prim.Spawn();
                return prim;
            }
            public ShieldBeltStat(Player player)
            {
                this.player = player;
                primitive = CreatePrimitive(player);
                currentCharge = Base;
                timeRemaining = 0;
            }

            public void Destroy()
            {
                primitive.Destroy();
                primitive = null;
            }
        }

        public override uint Id { get; set; } = 5982;
        public override string Name { get; set; } = "Shield belt";
        public override string Description { get; set; } = "A projectile-repulsion device.\nIt will attempt to stop incoming projectiles or shrapnel, but does nothing against melee attacks or heat.\nIt prevents the wearer from firing out.\n(works in the inventory) ";
        public override float Weight { get; set; } = 0.65f;
        public override SpawnProperties SpawnProperties { get; set; } = null;



        private Dictionary<Player, ShieldBeltStat> stats = new();
        private CoroutineHandle handle;

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DroppedItem += OnDroppedItem;
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Player.Dying += OnDying;
            Exiled.Events.Handlers.Player.Shooting += OnShooting;
            InventorySystem.Items.ThrowableProjectiles.Scp2176Projectile.OnServerShattered += OnServerShattered;
            handle = Timing.RunCoroutine(Tick());
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DroppedItem -= OnDroppedItem;
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            Exiled.Events.Handlers.Player.Shooting -= OnShooting;
            InventorySystem.Items.ThrowableProjectiles.Scp2176Projectile.OnServerShattered -= OnServerShattered;
            Timing.KillCoroutines(handle);
            base.UnsubscribeEvents();
        }

        public override bool Check(Player player)
        {

            if (player is null) return false;

            return player.Items.Any(Check);
        }

        protected override void OnAcquired(Player player, Item item, bool displayMessage)
        {
            if (!Check(item)) return;
            stats.Add(player, new(player));
            Log.Debug("player got shield");
            base.OnAcquired(player, item, displayMessage);
        }

        private void OnDroppedItem(DroppedItemEventArgs ev)
        {
            if (!Check(ev.Pickup)) return;

            stats[ev.Player].Destroy();
            stats.Remove(ev.Player);
            Log.Info("player lost shilde");

        }
        private void OnShooting(ShootingEventArgs ev)
        {
            if (!Check(ev.Player)) return;


            ev.IsAllowed = false;
        }
        private void OnServerShattered(InventorySystem.Items.ThrowableProjectiles.Scp2176Projectile projectile,RoomIdentifier roomid)
        {
            Room room = Room.Get(roomid);

            foreach(Player player in room.Players.Where(Check))
            {
                stats[player].Break();
            }
            

        }

        private void OnDying(DyingEventArgs ev)
        {
            if (ev.ItemsToDrop.Any(Check))
            {
                stats[ev.Player].Destroy();
                stats.Remove(ev.Player);
            }
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (!Check(ev.Player))
                return;
            if (ev.IsInstantKill)
                return;
            if (ev.DamageHandler.CustomBase is not FirearmDamageHandler)
                return;


            ShieldBeltStat stat = stats[ev.Player];
            if (!stat.IsActive) return;



            ev.Amount = stat.Damage(ev.Amount);
            


        }



        private IEnumerator<float> Tick()
        {
            while (true)
            {

                foreach(Player p in stats.Keys)
                {
                    stats[p].RechargeTick();
                }


                yield return Timing.WaitForOneFrame;
            }
        }
        



    }
}
