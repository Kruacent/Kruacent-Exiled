using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Interfaces;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using KE.Items.Features;
using KE.Items.Items.Models;
using MEC;
using System.Collections.Generic;
using System.Diagnostics;
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
        public float TimeActive { get; set; } = 5;
        public static float Radius { get; set; } = 5;
        public override SpawnProperties SpawnProperties { get; set; } = null;
        private HashSet<Player> _affectedPlayers = new HashSet<Player>();


        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Shooting += OnShooting;
            Exiled.Events.Handlers.Player.UsingMicroHIDEnergy += OnUsingMicroHIDEnergy;
            Exiled.Events.Handlers.Player.Dying += OnDying;
            Exiled.Events.Handlers.Scp049.Attacking += OnAttacking049;
            Exiled.Events.Handlers.Scp106.Attacking += OnAttacking106;
            Exiled.Events.Handlers.Item.ChargingJailbird += OnChargingJailbird;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Shooting -= OnShooting;
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            Exiled.Events.Handlers.Player.UsingMicroHIDEnergy -= OnUsingMicroHIDEnergy;
            Exiled.Events.Handlers.Scp049.Attacking -= OnAttacking049;
            Exiled.Events.Handlers.Scp106.Attacking -= OnAttacking106;
            Exiled.Events.Handlers.Item.ChargingJailbird -= OnChargingJailbird;
            base.UnsubscribeEvents();
        }

        private void Disallow(IDeniableEvent ev)
        {
            if(ev is IPlayerEvent p)
            {
                if (_affectedPlayers.Contains(p.Player))
                {
                    ev.IsAllowed = false;
                }
            }
            
        }

        private void OnUsingMicroHIDEnergy(UsingMicroHIDEnergyEventArgs ev)
        {
            Disallow(ev);
        }
        private void OnChargingJailbird(ChargingJailbirdEventArgs ev)
        {
            if (_affectedPlayers.Contains(ev.Player))
                ev.IsAllowed = false;
        }
        private void OnAttacking106(Exiled.Events.EventArgs.Scp106.AttackingEventArgs ev)
        {
            Disallow(ev);
        }

        private void OnAttacking049(Exiled.Events.EventArgs.Scp049.AttackingEventArgs ev)
        {
            Disallow(ev);
        }
        private void OnDying(DyingEventArgs ev)
        {
            Disallow(ev);
                
        }
        private void OnShooting(ShootingEventArgs ev)
        {
            Disallow(ev);
        }
        protected override void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (ev.IsThrown)
            {
                return;
            }
            ev.Item.Destroy();
            Vector3 pos = ev.Player.Position;
            Scp514Model cage = new();

            cage.Create(pos,new());

            Timing.RunCoroutine(DoEffectIfInside(Radius,pos,cage));

            

        }

        
        private IEnumerator<float> DoEffectIfInside(float radius,Vector3 spawnPos, Scp514Model model)
        {
            var time= Stopwatch.StartNew();
            Log.Debug("starting effect");
            while(time.Elapsed.TotalSeconds <= TimeActive)
            {
                foreach(Player p in Player.List)
                {
                    if (IsPlayerInZone(p, spawnPos, radius))
                    {
                        if (_affectedPlayers.Add(p))
                        {
                            p.IsGodModeEnabled = true;
                            Log.Debug("adding " + p.Id);
                        }
                        
                        
                    }
                    else
                    {
                        if (_affectedPlayers.Remove(p))
                        {
                            p.IsGodModeEnabled = false;
                            Log.Debug("removing" + p.Id);
                        }
                    }
                }
                yield return Timing.WaitForOneFrame;
            }
            Log.Debug("ending effects");
            time.Stop();

            foreach(var p in _affectedPlayers)
            {
                p.IsGodModeEnabled = false;
            }
            _affectedPlayers.Clear();
            model.Destroy();
        }



        private bool IsPlayerInZone(Player player, Vector3 zonePosition, float radius)
        {
            float distance = Vector3.Distance(new Vector3(player.Position.x, 0, player.Position.z),
                                               new Vector3(zonePosition.x, 0, zonePosition.z));
            return distance <= (radius / 2);
        }
    }
}
