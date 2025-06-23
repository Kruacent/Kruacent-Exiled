using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.API.Features.Toys;
using KE.Map.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace KE.Map.GamblingZone
{
    public class GamblingRoom
    {
        private static readonly HashSet<GamblingRoom> _list = new HashSet<GamblingRoom>();
        public static HashSet<GamblingRoom> List => new(_list);



        private HashSet<Primitive> _model;
        private float _pickupTime = 30;
        private InteractiblePickup _pickup;
        private Vector3 _position;
        private Vector3 _scale;
        private LootTable _lootTable;

        internal GamblingRoom(Room room, LootTable lootTable, Vector3? offset = null)
        {
            Init(room.Position,   lootTable, offset);
        }


        internal GamblingRoom(Door door, LootTable lootTable, Vector3? offset = null)
        {
            Init(door.Position,   lootTable,offset);
        }

        internal GamblingRoom(Vector3 position, LootTable lootTable, Vector3? offset = null)
        {
            Init(position, lootTable, offset);
        }



        private void Init(Vector3 position,LootTable lootTable,Vector3? offset = null)
        {
            
            

            _position = position + (offset ?? new Vector3());
            _list.Add(this);
            
            _pickup = new InteractiblePickup(ItemType.Medkit, _position+ Vector3.up, new Vector3(1,0,1)*3, _pickupTime, new());
            
            _pickup.AddAction(OnPickup);

            //CreateModel(_position);
            _lootTable = lootTable;
        }


        private void CreateModel(Vector3 positionWithOffset)
        {
            _model = new()
            {
                Primitive.Create(PrimitiveType.Sphere,positionWithOffset,null,null,true,Color.red),
                Primitive.Create(PrimitiveType.Cube,positionWithOffset,null,new(2,.5f,2),true)
            };
            foreach(Primitive p in _model)
            {
                p.Collidable = true;
            }
        }


        public void SubscribeEvents()
        {
            
        }
        public void UnsubscribeEvents()
        {
            foreach (Primitive p in _model)
            {
                p.Destroy();
            }
            _pickup.Destroy();
        }


        public void OnPickup(Player player)
        {
            if (player.CurrentItem == null) return;
            if (player == null) return;
            Item item = _lootTable.GetRandomItem();
            player.CurrentItem.Destroy();
            player.AddItem(item);
            player.DropItem(item,false);
        }
    }
}
