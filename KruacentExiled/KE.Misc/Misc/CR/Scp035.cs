using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Misc.CR
{
    [CustomRole(RoleTypeId.Tutorial)]
    public class Scp035 : CustomRole
    {
        internal static HashSet<Player> _trackedPlayers = new ();
        public override uint Id { get; set; } = 10;
        public override string Description { get; set; } = "t'es un humain dans la team des scp en gros ";
        public override RoleTypeId Role { get; set; } = RoleTypeId.Tutorial;
        public override int MaxHealth { get; set; } = 600;
        public override string Name { get; set; } = "SCP-035";
        public override string CustomInfo { get; set; } = string.Empty;
        public override bool IgnoreSpawnSystem { get; set; } = true;
        public override SpawnProperties SpawnProperties { get; set; } = new()
        {
            DynamicSpawnPoints = new()
            {
                new()
                {
                    Location = Exiled.API.Enums.SpawnLocationType.Inside096
                }
            }
        };
        protected override void RoleAdded(Player player)
        {
            _trackedPlayers.Add(player);
        }

        protected override void RoleRemoved(Player player)
        {
            _trackedPlayers.Remove(player);
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.EnteringPocketDimension += OnEnteringPocketDimension;
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Player.Shot += OnShot;
            Exiled.Events.Handlers.Player.ActivatingGenerator += OnActivatingGenerator;
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.EnteringPocketDimension -= OnEnteringPocketDimension;
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Player.Shot -= OnShot;
            Exiled.Events.Handlers.Player.ActivatingGenerator -= OnActivatingGenerator;
        }


        private void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
        {
            if (Check(ev.Player))
                ev.IsAllowed = false;
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Attacker is null) return;
            if ((Check(ev.Player) || Check(ev.Attacker)) && (ev.Player.IsScp || ev.Attacker.IsScp))
                ev.IsAllowed = false;
        }

        private void OnShot(ShotEventArgs ev)
        {
            if (ev.Target != null && ev.Target.IsScp && Check(ev.Player))
                ev.CanHurt = false;
        }

        private void OnActivatingGenerator(ActivatingGeneratorEventArgs ev)
        {
            if (Check(ev.Player))
                ev.IsAllowed = false;
        }
    }
}
