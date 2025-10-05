using Exiled.API.Enums;
using Exiled.API.Features;
using LabApi.Events.Arguments.ServerEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.Entrance
{
    public abstract class MoreRoom
    {
        private static HashSet<MoreRoom> all = new();
        public MoreRoom()
        {
            all.Add(this);
        }
        public abstract RoomType RoomType { get; }

        public abstract void Create(Vector3 position, Quaternion rotation);
        public abstract void Destroy();

        public abstract int Limit { get; }
        private int curr = 0;
        private static HashSet<Room> usedRooms = new();

        public static void CreateAll()
        {
            //new EzArmory();
            new Locked();
        }
        public static void SubscribeEvents()
        {
            Exiled.Events.Handlers.Map.Generated += OnMapGenerated;
            //LabApi.Events.Handlers.ServerEvents.MapGenerated += OnMapGenerated;
        }

        public static void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Map.Generated -= OnMapGenerated;
            //LabApi.Events.Handlers.ServerEvents.MapGenerated -= OnMapGenerated;
            DestroyAll();
        }
        public static void DestroyAll()
        {
            foreach(MoreRoom room in all)
            {
                room.Destroy();
            }
        }
        //MapGeneratedEventArgs _
        public static void OnMapGenerated()
        {
            foreach (MoreRoom room in all)
            {
                foreach (Room r in Room.List.Where(r => r.Type == room.RoomType && !usedRooms.Contains(r)))
                {
                    if(room.curr < room.Limit)
                    {
                        Log.Debug("spawn");
                        room.Create(r.Position, r.Rotation);
                        room.curr++;
                        usedRooms.Add(r);
                    }
                }
            }
        }

        






    }
}
