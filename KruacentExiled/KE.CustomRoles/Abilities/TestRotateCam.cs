using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.Abilities
{
    [CustomAbility]
    internal class TestRotateCam : ActiveAbility
    {
        public override string Name { get; set; } = "TestRotateCam";

        public override string Description { get; set; } = "TestRotateCam";

        public override float Duration { get; set; } = 0f;

        public override float Cooldown { get; set; } = 45f;
        private List<Player> _players = new List<Player>();

        protected override void AbilityUsed(Player player)
        {
            //player.ShowHint("interact with a door to open it", 5f);
            player.CameraTransform.Rotate(new Vector3(0, 180, 0));



        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
            base.SubscribeEvents();
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
            if (ev.Door.IsCheckpoint) return;
            if (ev.Door.IsLocked) return;

            ev.IsAllowed = false;
            ev.Player.ShowHint("The door will open in 5 seconds", 5f);
            ev.Player.Hurt(ev.Player.MaxHealth / 10, Exiled.API.Enums.DamageType.Strangled);
            _players.Remove(ev.Player);
            Timing.CallDelayed(5f, () =>
            {
                ev.Door.IsOpen = true;
            });
        }
    }
}
