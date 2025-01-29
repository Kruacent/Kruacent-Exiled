using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.Abilities
{
    [CustomAbility]
    internal class OpenDoor : ActiveAbility
    {
        public override string Name { get; set; } = "OpenDoor";

        public override string Description { get; set; } = "Open a lock door at the cost of your health";

        public override float Duration { get; set; } = 0f;

        public override float Cooldown { get; set; } = 45f;
        private List<Player> _players = new List<Player>();

        protected override void AbilityUsed(Player player)
        {
            player.ShowHint("interact with a door to open it",5f);
            _players.Add(player);
            Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
            base.UnsubscribeEvents();
        }

        private void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (!_players.Contains(ev.Player)) return;
            if (ev.Door.IsOpen) return;
            if (!ev.Door.IsKeycardDoor) return;
            if(ev.Door.IsCheckpoint) return;
            if(ev.Door.IsLocked) return;

            ev.IsAllowed = false;
            ev.Player.ShowHint("The door will open in 5 seconds",5f);
            ev.Player.Hurt(ev.Player.MaxHealth / 10,Exiled.API.Enums.DamageType.Strangled);
            _players.Remove(ev.Player);
            Timing.CallDelayed(5f, () =>
            {
                ev.Door.IsOpen = true;
            });
        }
    }
}
