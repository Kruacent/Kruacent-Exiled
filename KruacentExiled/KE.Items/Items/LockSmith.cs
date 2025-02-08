
using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Door = Exiled.API.Features.Doors.Door;
using MEC;
using PlayerRoles.PlayableScps.Scp079.Overcons;
using KE.Items.Interface;

namespace KE.Items.Items
{
    [CustomItem(ItemType.KeycardJanitor)]
    public class LockSmith : CustomKeycard, ILumosItem
    {
        public override uint Id { get; set; } = 1056;
        public override string Name { get; set; } = "LockSmith Card";
        public override string Description { get; set; } = "You can lock a door in 2 seconds, the door will move to the opposite state and will be locked for 30 seconds";
        public override float Weight { get; set; } = 0.65f;

        private Dictionary<Player, DateTime> lastUsed = new Dictionary<Player, DateTime>();

        // Item cooldown in seconds
        private int Cooldown { get; set; } = 120; 

        public UnityEngine.Color Color { get; set; } = UnityEngine.Color.gray;

        private float LockTime = 30f;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 2,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new DynamicSpawnPoint()
                {
                    Chance = 20,
                    Location = SpawnLocationType.InsideHidChamber,
                },
                new DynamicSpawnPoint()
                {
                    Chance= 30,
                    Location = SpawnLocationType.InsideEscapePrimary,
                },
                new DynamicSpawnPoint()
                {
                    Chance= 20,
                    Location = SpawnLocationType.InsideSurfaceNuke,
                },
            },
        };


        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.InteractingDoor += OnUsing;
            base.SubscribeEvents();
        }

        /// <inheritdoc/>
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.InteractingDoor -= OnUsing;
            base.UnsubscribeEvents();
        }

        private void OnUsing(InteractingDoorEventArgs ev)
        {
            List<DoorType> doors = new List<DoorType>
            {
                DoorType.GateA,
                DoorType.GateB,
            };

            if (!Check(ev.Player.CurrentItem))
                return;

            if (lastUsed.ContainsKey(ev.Player))
            {

                DateTime usableTime = lastUsed[ev.Player] + TimeSpan.FromSeconds(Cooldown);
                if (DateTime.Now > usableTime)
                {
                    ev.Player.ShowHint($"You must wait another {Math.Round((DateTime.Now - usableTime).TotalSeconds, 2)} seconds to use {Name}");
                    return;
                }
                else
                {
                    lastUsed.Remove(ev.Player);
                }
            }

            if (doors.Contains(ev.Door.Type) || ev.Door.IsLocked)
            {
                ev.Player.ShowHint("This door can't be locked down by Lock Smith");
                return;
            }

            ev.IsAllowed = true;
            ev.Door.IsOpen = !ev.Door.IsOpen;
            ev.Door.Lock(LockTime, DoorLockType.Isolation);
            lastUsed.Add(ev.Player, DateTime.UtcNow);
        }
    }
}
