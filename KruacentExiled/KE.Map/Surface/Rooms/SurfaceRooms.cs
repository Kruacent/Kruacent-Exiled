using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.Utils.API.Interfaces;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.Surface.Rooms
{
    public class SurfaceRooms : IUsingEvents
    {

        public static GameObject gameObject;
        public static readonly HashSet<SurfaceRoom> Rooms = new HashSet<SurfaceRoom>()
        {
            new SurfaceArmory()
        };
        public void SubscribeEvents()
        {
            //Exiled.Events.Handlers.Map.Generated += OnGenerated;
        }

        public void UnsubscribeEvents()
        {
            //Exiled.Events.Handlers.Map.Generated -= OnGenerated;
            
        }

        private void OnGenerated()
        {


            Vector3 roomPos =  Room.List.FirstOrDefault(x => x.Type == Exiled.API.Enums.RoomType.Surface).Position- new Vector3(-137, 4, 60);
            SpawnRandomRoom(roomPos);


        }


        private void SpawnRandomRoom(Vector3 position)
        {
            SurfaceRoom bp = Rooms.GetRandomValue();

            NetworkServer.Spawn(gameObject);
        }

    }
}
