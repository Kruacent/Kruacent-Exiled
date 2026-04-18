using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Player;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using KruacentExiled.Map;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KruacentExiled.Map.Heavy.GamblingZone
{
    public class OldGamblingRoom : AbstractGambling
    {
        private static readonly HashSet<OldGamblingRoom> _list = new HashSet<OldGamblingRoom>();
        public static HashSet<OldGamblingRoom> List => new HashSet<OldGamblingRoom>(_list);


        private Vector3 _position;
        private Vector3 _scale;
        private LootTable _lootTable;

        internal OldGamblingRoom(Room room, Vector3 scale, LootTable lootTable, Vector3? offset = null)
        {
            Init(room.Position, scale, lootTable, offset);
        }


        internal OldGamblingRoom(Door door, Vector3 scale, LootTable lootTable, Vector3? offset = null)
        {
            Init(door.Position, scale, lootTable, offset);
        }

        internal OldGamblingRoom(Vector3 position, Vector3 scale, LootTable lootTable, Vector3? offset = null)
        {
            Init(position, scale, lootTable, offset);
        }



        private void Init(Vector3 position, Vector3 scale, LootTable lootTable, Vector3? offset = null)
        {
            Log.Debug("position w/out offset " + position);
            _position = position + (offset ?? new Vector3());
            Log.Debug("position w/ offset " + _position);
            _scale = scale;
            _list.Add(this);
            _lootTable = lootTable;
            Exiled.Events.Handlers.Player.DroppedItem += OnDropped;
            if (MainPlugin.Instance.Config.Debug)
            {
                var p = Primitive.Create(PrimitiveType.Cube, _position, null, _scale);
                p.Collidable = false;
            }
        }

        public bool IsInGamblingRoom(Player p)
        {
            Vector3 playerPosition = p.Position;
            Vector3 halfSize = _scale / 2;
            return playerPosition.x >= _position.x - halfSize.x &&
                   playerPosition.x <= _position.x + halfSize.x &&
                   playerPosition.y >= _position.y - halfSize.y &&
                   playerPosition.y <= _position.y + halfSize.y &&
                   playerPosition.z >= _position.z - halfSize.z &&
                   playerPosition.z <= _position.z + halfSize.z;
        }

        public override void Destroy()
        {
            Exiled.Events.Handlers.Player.DroppedItem -= OnDropped;
        }

        public void OnDropped(DroppedItemEventArgs ev)
        {
            Player player = ev.Player;
            if (player == null) return;
            if (ev.Pickup == null) return;
            if (SCPTeam.IsSCP(player.ReferenceHub)) return;

            if (!IsInGamblingRoom(player))
            {
                Log.Debug($"player ({player.CustomName}) not in room ({player.Position})");
                return;
            }
            ev.Pickup.Destroy();
            Item item = _lootTable.GetRandomItem();
            item.CreatePickup(player.Position);
        }

    }
}
