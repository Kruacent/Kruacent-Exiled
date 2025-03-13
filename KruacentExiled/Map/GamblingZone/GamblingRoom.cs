using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using PluginAPI.Roles;
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
            if (ev.Player == null) return;
            if(ev.Pickup == null) return;
            if (Vector3.Distance(ev.Player.Position, _room.Position) > 8.2f) return;
            if (!IsInGamblingRoom(ev.Player)) return;
            ev.Pickup.Destroy();
            Item item = _lootTable.GetRandomItem();
            ev.Player.AddItem(item);
            ev.Player.DropItem(item);
        }
    }
}
