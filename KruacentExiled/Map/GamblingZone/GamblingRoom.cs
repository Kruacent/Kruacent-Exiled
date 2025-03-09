using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map.GamblingZone
{
    public class GamblingRoom
    {
        internal static HashSet<GamblingRoom> List { get; }= new HashSet<GamblingRoom>();

        private Room _room;
        private LootTable _lootTable;

        internal GamblingRoom(Room room, LootTable lootTable)
        {
            _room = room;
            List.Add(this);
            _lootTable = lootTable;
        }

        public bool IsInGamblingRoom(Player p)
        {
            return _room.Players.Contains(p);
        }

        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DroppedItem += OnDropped;
        }
        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DroppedItem -= OnDropped;
        }


        public void OnDropped(DroppedItemEventArgs ev)
        {
            if (!IsInGamblingRoom(ev.Player)) return;
            Vector3 pos = ev.Pickup.Position;
            ev.Pickup.Destroy();
            _lootTable.GetRandomItem().CreatePickup(pos);
        }
    }
}
