using Exiled.API.Features;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.CR.Human
{
    public class Pacifist : KECustomRoleMultipleRole
    {
        public override string Description { get; set; } = "T'es idées empêche quelconque violence. S'enlève quand tu t'échappes et ramène plus de renfort";
        public override string PublicName { get; set; } = "Pacifiste";
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = false;
        public override float SpawnChance { get; set; } = 100;

        public override HashSet<RoleTypeId> Roles => [RoleTypeId.Scientist,RoleTypeId.ClassD];


        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Item.DisruptorFiring += OnDisruptorFiring;
            Exiled.Events.Handlers.Item.Swinging += OnSwinging;
            Exiled.Events.Handlers.Item.ChargingJailbird += OnChargingJailbird;
            Exiled.Events.Handlers.Player.Shooting += OnShooting;
            Exiled.Events.Handlers.Player.ThrowingRequest += OnThrowingRequest;
            Exiled.Events.Handlers.Player.Escaped += OnEscaped;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {

            base.UnsubscribeEvents();
        }
        private void OnThrowingRequest(ThrowingRequestEventArgs ev)
        {
            if (Check(ev.Player))
            {
                ev.RequestType = Exiled.API.Enums.ThrowRequest.CancelThrow;
            }

        }

        private void OnDisruptorFiring(DisruptorFiringEventArgs ev)
        {
            if (Check(ev.Attacker))
            {
                ev.IsAllowed = false;
            }

        }
        private void OnSwinging(SwingingEventArgs ev)
        {
            if (Check(ev.Player))
            {
                ev.IsAllowed = false;
            }
        }
        private void OnChargingJailbird(ChargingJailbirdEventArgs ev)
        {
            if (Check(ev.Player))
            {
                ev.IsAllowed = false;
            }
        }

        private void OnShooting(ShootingEventArgs ev)
        {
            if (Check(ev.Player))
            {
                ev.IsAllowed = false;
            }
        }


        private void OnEscaped(EscapedEventArgs ev)
        {
            Player escape = ev.Player;
            if (Check(escape))
            {
                RemoveRole(escape);
                if(Player.Enumerable.Count(p => p.Role == RoleTypeId.Spectator) > 0)
                {
                    Player respawned = Player.Enumerable.First();

                    respawned.Role.Set(escape.Role, Exiled.API.Enums.SpawnReason.Escaped, RoleSpawnFlags.All);
                }

            }
        }



    }
}
