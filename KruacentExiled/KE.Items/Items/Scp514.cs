using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp173;
using Exiled.Events.EventArgs.Scp939;
using Exiled.Events.Handlers;
using KE.Items.Features;
using KE.Items.Items.Models;
using MEC;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Player = Exiled.API.Features.Player;

namespace KE.Items.Items
{
    [CustomItem(ItemType.Flashlight)]
    public class Scp514 : KECustomItem
    {
        public override uint Id { get; set; } = 1070;
        public override string Name { get; set; } = "SCP-514";
        public override string Description { get; set; } = "birb";
        public override float Weight { get; set; } = 0.65f;
        public float TimeActive { get; set; } = 3;
        public float Radius { get; set; } = 5;
        public override SpawnProperties SpawnProperties { get; set; } = null;
        private HashSet<Player> _affectedPlayers = new HashSet<Player>();


        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Shooting += OnShooting;
            Exiled.Events.Handlers.Player.Dying += OnDying;
            Exiled.Events.Handlers.Scp049.Attacking += OnAttacking049;
            Exiled.Events.Handlers.Scp106.Attacking += OnAttacking106;
            Exiled.Events.Handlers.Item.ChargingJailbird += OnChargingJailbird;
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Shooting -= OnShooting;
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            Exiled.Events.Handlers.Scp049.Attacking -= OnAttacking049;
            Exiled.Events.Handlers.Scp106.Attacking -= OnAttacking106;
            Exiled.Events.Handlers.Item.ChargingJailbird -= OnChargingJailbird;
        }
        private void OnChargingJailbird(ChargingJailbirdEventArgs ev)
        {
            if (_affectedPlayers.Contains(ev.Player))
                ev.IsAllowed = false;
        }
        private void OnAttacking106(Exiled.Events.EventArgs.Scp106.AttackingEventArgs ev)
        {
            if (_affectedPlayers.Contains(ev.Player))
                ev.IsAllowed = false;
        }

        private void OnAttacking049(Exiled.Events.EventArgs.Scp049.AttackingEventArgs ev)
        {
            if (_affectedPlayers.Contains(ev.Player))
                ev.IsAllowed = false;
        }
        private void OnDying(DyingEventArgs ev)
        {
            if(_affectedPlayers.Contains(ev.Player))
                ev.IsAllowed = false;
                
        }
        private void OnShooting(ShootingEventArgs ev)
        {
            if(_affectedPlayers.Contains(ev.Player))
                ev.IsAllowed = false;
        }
        protected override void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (ev.IsThrown)
            {
                return;
            }

            Vector3 pos = ev.Player.Position;
            Scp514Model cage = new();

            cage.Create(pos,new());

            Timing.RunCoroutine(DoEffectIfInside(Radius,pos));

        }

        
        private IEnumerator<float> DoEffectIfInside(float radius,Vector3 spawnPos)
        {
            while(true)
            {
                foreach(Player p in Player.List)
                {
                    if (IsPlayerInZone(p, spawnPos, radius))
                    {
                        _affectedPlayers.Add(p);
                        p.IsGodModeEnabled = true;
                    }
                    else
                    {
                        p.IsGodModeEnabled = false;
                        _affectedPlayers.Remove(p);
                    }
                }
                yield return Timing.WaitForSeconds(1);
            }
        }



        private bool IsPlayerInZone(Player player, Vector3 zonePosition, float radius)
        {
            float distance = Vector3.Distance(new Vector3(player.Position.x, 0, player.Position.z),
                                               new Vector3(zonePosition.x, 0, zonePosition.z));
            return distance <= (radius / 2);
        }
    }
}
