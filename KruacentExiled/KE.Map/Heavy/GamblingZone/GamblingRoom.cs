using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Server;
using KE.Map.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace KE.Map.Heavy.GamblingZone
{
    public class GamblingRoom
    {
        private static readonly HashSet<GamblingRoom> _list = new HashSet<GamblingRoom>();
        public static IReadOnlyCollection<GamblingRoom> List => _list;



        private HashSet<Primitive> _model;
        public float PickupTime { get; } = 30;
        private InteractiblePickup _pickup;
        private Vector3 _position;
        private LootTable _lootTable;

        internal GamblingRoom(Room room, LootTable lootTable, Vector3? offset = null)
        {
            Init(room.Position, lootTable, offset);
        }


        internal GamblingRoom(Door door, LootTable lootTable, Vector3? offset = null)
        {
            Init(door.Position, lootTable, offset);
        }

        internal GamblingRoom(Vector3 position, LootTable lootTable, Vector3? offset = null)
        {
            Init(position, lootTable, offset);
        }



        private void Init(Vector3 position, LootTable lootTable, Vector3? offset = null)
        {



            _position = position + (offset ?? new Vector3());
            _list.Add(this);

            _pickup = new InteractiblePickup(ItemType.Medkit, _position, new Vector3(4, 4, 4), PickupTime, new());

            _pickup.AddAction(OnPickup);

            CreateModel(_position);
            _lootTable = lootTable;

        }


        private void CreateModel(Vector3 positionWithOffset)
        {


            Vector3 width = new Vector3(.1f, 1, .1f);


            _model = new()
            {
                Primitive.Create(PrimitiveType.Cube,positionWithOffset , null,new(1,.8f,1),true,Color.black),
                Primitive.Create(PrimitiveType.Cube,positionWithOffset + Vector3.forward/2,null, Vector3.right+width,true,Color.white),
                Primitive.Create(PrimitiveType.Cube,positionWithOffset + Vector3.back/2, null, Vector3.right+width,true,Color.white),
                Primitive.Create(PrimitiveType.Cube,positionWithOffset + Vector3.right/2, null, Vector3.forward+width,true,Color.white),
                Primitive.Create(PrimitiveType.Cube,positionWithOffset + Vector3.left/2, null, Vector3.forward+width,true,Color.white),

            };
            foreach (Primitive p in _model)
            {
                p.Collidable = false;
            }
        }




        public void Destroy()
        {
            foreach (Primitive p in _model)
            {
                p.Destroy();
            }
            _pickup.Destroy();
            _list.Remove(this);
        }


        public void OnPickup(Player player)
        {
            if (player == null) return;
            if (player.CurrentItem == null) return;
            Item item = _lootTable.GetRandomItem();
            player.CurrentItem.Destroy();
            player.AddItem(item);

            player.DropItem(item, false);
        }
        public static void DestroyAll()
        {
            foreach (GamblingRoom gamblingRoom in List)
            {
                gamblingRoom.Destroy();
            }
        }

        public static void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        }

        public static void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        }


        private static void OnRoundEnded(RoundEndedEventArgs ev)
        {
            DestroyAll();
        }
    }
}
