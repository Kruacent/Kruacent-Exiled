using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.DamageHandlers;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.API.Features.Pools;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using HintServiceMeow.Core.Models.HintContent;
using HintServiceMeow.Core.Utilities;
using KE.Items.API.Features;
using KE.Items.API.Interface;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using MapGeneration;
using MEC;
using PlayerRoles.FirstPersonControl;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KE.Items.Items.ShieldBelt
{
    [CustomItem(ItemType.KeycardCustomSite02)]
    public class ShieldBelt : KECustomKeycard
    {

        public override uint Id { get; set; } = 5982;
        public override string Name { get; set; } = "Shield belt";
        public override string Description { get; set; } = "A projectile-repulsion device.\nIt will attempt to stop incoming projectiles or shrapnel, but does nothing against melee attacks or heat.\nIt prevents the wearer from firing out.\n(works in the inventory) ";
        public override float Weight { get; set; } = 0.65f;
        public override SpawnProperties SpawnProperties { get; set; } = null;

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DroppedItem += OnDroppedItem;
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Player.Dying += OnDying;
            Exiled.Events.Handlers.Player.Shooting += OnShooting;
            InventorySystem.Items.ThrowableProjectiles.Scp2176Projectile.OnServerShattered += OnServerShattered;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DroppedItem -= OnDroppedItem;
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            Exiled.Events.Handlers.Player.Shooting -= OnShooting;
            InventorySystem.Items.ThrowableProjectiles.Scp2176Projectile.OnServerShattered -= OnServerShattered;
            base.UnsubscribeEvents();
        }

        public override bool Check(Player player)
        {

            if (player is null) return false;

            return player.Items.Any(Check);
        }
        private static HintPosition HintPosition = new ShieldBeltPosition();
        protected override void OnAcquired(Player player, Item item, bool displayMessage)
        {
            if (!Check(item)) return;
            var comp = player.GameObject.AddComponent<ShieldBeltStat>();
            Log.Debug("player got shield");


            
            Log.Debug("no hints");
            if(!DisplayHandler.Instance.HasHint(player, HintPosition.HintPlacement))
            {
                DisplayHandler.Instance.CreateAuto(player, (arg) => GetContent(player), HintPosition.HintPlacement);
            }
            base.OnAcquired(player, item, displayMessage);
        }


        private string GetContent(Player player)
        {
            if (player.GameObject.TryGetComponent<ShieldBeltStat>(out var stat))
            {
                StringBuilder sb =  StringBuilderPool.Pool.Get();

                if (stat.IsActive)
                {
                    sb.Append("<color=#00FF00>");
                }
                else
                {
                    sb.Append("<color=#FF0000>");
                }


                sb.Append("ShieldBelt Status : ");
                sb.Append(stat.CurrentCharge);
                sb.Append("/");
                sb.Append(ShieldBeltStat.MaxCharge);
                sb.Append("HP");
                sb.Append("</color>");
                return StringBuilderPool.Pool.ToStringReturn(sb);
            }


            return " ";
        }

        private void OnDroppedItem(DroppedItemEventArgs ev)
        {
            if (!Check(ev.Pickup)) return;

            if(ev.Player.GameObject.TryGetComponent<ShieldBeltStat>(out var comp))
            {
                comp.Destroy();
            }
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
                if (player.GameObject.TryGetComponent<ShieldBeltStat>(out var comp))
                {
                    comp.Break();
                }
            }
            

        }

        private void OnDying(DyingEventArgs ev)
        {
            if (ev.ItemsToDrop.Any(Check))
            {
                if (ev.Player.GameObject.TryGetComponent<ShieldBeltStat>(out var comp))
                {
                    comp.Destroy();
                }
            }
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (!Check(ev.Player))
                return;
            if (ev.IsInstantKill)
                return;
            if (!(ev.DamageHandler.CustomBase is FirearmDamageHandler))
                return;
            if (!ev.Player.GameObject.TryGetComponent<ShieldBeltStat>(out var stat))
            {
                return;
            }
            if (!stat.IsActive) return;
            ev.Amount = stat.Damage(ev.Amount);
            


        }




    }
}
