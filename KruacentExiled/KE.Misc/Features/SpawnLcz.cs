using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using LabDoor = LabApi.Features.Wrappers.Door;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Security;

namespace KE.Misc.Features
{
    internal class SpawnLcz : MiscFeature
    {
        public float Time { get; } = 198;

        public static readonly Dictionary<RoleTypeId, RoomType> RoomTypes = new()
        {
            { RoleTypeId.Scp939,RoomType.Lcz330 },
            { RoleTypeId.Scp096,RoomType.LczGlassBox },
            { RoleTypeId.Scp173,RoomType.LczGlassBox },
        };
        private float chance = .33f;
        public float Chance
        {
            get
            {
                return chance;
            }
            set
            {
                chance = Mathf.Clamp01(value);
            }
        }

        private HashSet<ushort> serials = new();
        private HashSet<Door> locked = new();

        public override void SubscribeEvents()
        {
            //Chance = 1;
            LabApi.Events.Handlers.ServerEvents.RoundStarted += OnRoundStarted;
            LabApi.Events.Handlers.PlayerEvents.DroppingItem += OnDroppingItem;
            base.SubscribeEvents();
        }



        public override void UnsubscribeEvents()
        {
            LabApi.Events.Handlers.ServerEvents.RoundStarted -= OnRoundStarted;
            LabApi.Events.Handlers.PlayerEvents.DroppingItem -= OnDroppingItem;
            base.UnsubscribeEvents();
        }
        private void OnRoundStarted()
        {
            LockDoors();
            Timing.CallDelayed(.5f, delegate
            {
                foreach (Player player in Player.List.Where(p => RoomTypes.ContainsKey(p.Role)))
                {
                    if (UnityEngine.Random.value < Chance)
                    {
                        player.Teleport(Room.Get(RoomTypes[player.Role]));
                        GiveEntertainment(player);
                    }
                }
            });
            

            
            Timing.CallDelayed(Time, delegate
            {
                foreach(Door door in locked)
                {
                    LabDoor d = LabDoor.Get(door.Base);
                    d.Lock(Interactables.Interobjects.DoorUtils.DoorLockReason.SpecialDoorFeature, false);
                }
                foreach(ushort serial in serials)
                {
                    Item item =Item.Get(serial);
                    item?.Destroy();
                }
            });
        }
        private void OnDroppingItem(LabApi.Events.Arguments.PlayerEvents.PlayerDroppingItemEventArgs ev)
        {
            if (!serials.Contains(ev.Item.Serial)) return;
            ev.IsAllowed = false;
        }

        public void LockDoors()
        {
            foreach (RoomType room in RoomTypes.Values)
            {
                Room r = Room.Get(room);
                foreach (Door door in r.Doors)
                {
                    LockDoor(door);
                    locked.Add(door);
                }
            }
        }
        


        public void GiveEntertainment(Player player)
        {
            Keycard key = Item.Create<Keycard>(ItemType.KeycardChaosInsurgency);
            key.Give(player);
            serials.Add(key.Serial);
            player.CurrentItem = key;
        }

        private void LockDoor(Door door)
        {
            LabDoor d = LabDoor.Get(door.Base);
            d.Lock(Interactables.Interobjects.DoorUtils.DoorLockReason.SpecialDoorFeature, true);
        }





    }
}
