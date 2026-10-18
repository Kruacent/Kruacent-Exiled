using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using KE.Utils.API.Features;
using KruacentExiled.Commands.DebugCommands.RoomCustomPoints;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KruacentExiled.CustomSpawnPoint.Spawned
{

    /// <summary>
    /// Spawned variant of <see cref="RoomCustomPoint"/>
    /// </summary>
    public abstract class RoomSpawnedCustomPoint : IPosition
    {


        private static Dictionary<Room, List<RoomSpawnedCustomPoint>> _dict = new Dictionary<Room, List<RoomSpawnedCustomPoint>>();

        public static IReadOnlyDictionary<Room, List<RoomSpawnedCustomPoint>> Dictionary => _dict;

        private static List<RoomSpawnedCustomPoint> _all = new List<RoomSpawnedCustomPoint>();

        public static IReadOnlyList<RoomSpawnedCustomPoint> All => _all;

        public readonly Vector3 localposition;
        public readonly Quaternion localrotation;
        public Room Room { get; }

        public readonly RoomCustomPoint basePoint;

        public readonly RoomCustomPointType type;

        private Primitive debugPrimitive;

        public Color32 DebugColor { get; private set; }


        /// <summary>
        /// Check if the instance can be used and if it was not destroyed
        /// </summary>
        public bool IsValid { get; private set; }

        private bool _spawned;


        public bool IsDebugPrimitiveVisible
        {
            get
            {
                return _spawned;
            }
            set
            {
                if (value != _spawned)
                {
                    if (value)
                    {
                        debugPrimitive.Spawn();
                    }
                    else
                    {
                        debugPrimitive.UnSpawn();
                    }

                    _spawned = value;
                }
            }
        }


        public Pickup SpawnItem(ItemType itemType)
        {
            return Pickup.CreateAndSpawn(itemType, Position, null);
        }

        public Vector3 Position
        {
            get
            {
                return Room.Position + Room.Rotation * localposition;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return Room.Rotation * localrotation;
            }
        }
        public void Destroy()
        {

            if (_all.Contains(this))
            {
                _all.Remove(this);
            }
            if(_dict.ContainsKey(Room) && _dict[Room].Contains(this))
            {
                _dict[Room].Remove(this);
            }
            OnDestroy();
            

            
            if (debugPrimitive.GameObject != null)
            {
                debugPrimitive?.Destroy();
            }

            IsValid = false;

        }

        protected virtual void OnDestroy()
        {
            
        }
        protected RoomSpawnedCustomPoint(RoomCustomPoint roomCustomPoint,Room room,bool temporary=false)
        {
            if (roomCustomPoint == null)
            {
                throw new ArgumentNullException(nameof(roomCustomPoint));
            }

            if (room == null)
            {
                throw new ArgumentNullException(nameof(room));
            }

            if (room.Type != roomCustomPoint.roomType)
            {
                throw new ArgumentException("wrong roomtype", nameof(room));
            }
            basePoint = roomCustomPoint;
            Room = room;
            type = roomCustomPoint.type;
            localposition = roomCustomPoint.localPosition;
            localrotation = roomCustomPoint.localRotation;

            DebugColor = GetColor(type);

            if (temporary)
            {
                DebugColor = new Color32(DebugColor.r, DebugColor.g, DebugColor.b,100);
            }

            

            //KELog.Debug($"spawned RSCP in {Room.Type} at {localposition} ({localrotation.eulerAngles})");

            debugPrimitive = CreatePrimitive();
            _spawned = false;
            IsValid = true;



            if (temporary || ShowDebugPrimitives.AreCustomPointShowed)
            {
                IsDebugPrimitiveVisible = true;
            }
        }


        internal void SetPermanentColor()
        {
            DebugColor = new Color32(DebugColor.r, DebugColor.g, DebugColor.b, 255);
            debugPrimitive.Color = DebugColor;
        }


        internal static RoomSpawnedCustomPoint Create(RoomCustomPoint roomCustomPoint, Room room,bool register=true)
        {
            RoomSpawnedCustomPoint result;

            switch (roomCustomPoint.type)
            {
                case RoomCustomPointType.Teleport:
                    result = new RoomTeleportPoint(roomCustomPoint, room,!register);
                    break;
                case RoomCustomPointType.CustomItemSpawn:
                    result = new RoomCustomItemPoint(roomCustomPoint, room, !register);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(roomCustomPoint), "type not set");
            }

            



            if (register)
            {
                Register(result);
            }


            
            return result;
        }


        internal static void Register(RoomSpawnedCustomPoint roomSpawnedCustomPoint)
        {
            if(roomSpawnedCustomPoint == null)
            {
                throw new ArgumentNullException(nameof(roomSpawnedCustomPoint));
            }


            Room room = roomSpawnedCustomPoint.Room;
            _all.Add(roomSpawnedCustomPoint);
            if (!_dict.ContainsKey(room))
            {
                _dict[room] = new List<RoomSpawnedCustomPoint>()
                {
                    roomSpawnedCustomPoint
                };
            }
            else
            {
                _dict[room].Add(roomSpawnedCustomPoint);
            }

            roomSpawnedCustomPoint.SetPermanentColor();
        }

        private Primitive CreatePrimitive()
        {

            Primitive result = Primitive.Create(Position, Rotation.eulerAngles, Vector3.one * .1f, false, DebugColor);

            result.Collidable = false;

            return result;
        }


        public static Color32 GetColor(RoomCustomPointType type)
        {
            return type switch
            {
                RoomCustomPointType.CustomItemSpawn => Color.yellow,
                RoomCustomPointType.Teleport => Color.blue,
                _ => Color.grey,
            };
        }



        public static void SpawnAll()
        {
            if (!RoomCustomPoint.IsPopulated) throw new InvalidOperationException("Room Custom Point not populated");

            foreach(RoomCustomPoint point in RoomCustomPoint.All)
            {
                point.Spawn();
            }
        }



        

        public static void DestroyAll()
        {
            foreach (RoomSpawnedCustomPoint point in All.ToList())
            {
                point.Destroy();
            }
        }

        public static int NumberInRoom(Room room)
        {
            int result = 0;
            
            if (_dict.ContainsKey(room))
            {
                result = _dict[room].Count;
            }

            return result;
        }

    }
}
