using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Items.API.Features.SpawnPoints
{
    public static class PoseRoomSpawnPointHandler
    {
        public class ItemSpawn : IEquatable<ItemSpawn>
        {
            public readonly RoomType roomType;

            //item position in the room at 0°
            public readonly Vector3 localposition;
            //item rotation
            public readonly Quaternion localrotation;

            
            private Room room;

            public ItemSpawn(RoomType roomType,Vector3 position,Quaternion rotation)
            {
                this.roomType = roomType;
                this.localposition = position;
                this.localrotation = rotation;
            }

            private Room Room
            {
                get
                {
                    if(room is null)
                    {
                        room = Room.List.FirstOrDefault(r => r.Type == roomType);
                        if (room is null) throw new Exception($"room ({roomType}) not found");
                    }
                    return room;
                }
            }

            public Vector3 Position
            {
                get
                {
                    return Room.Position + room.Rotation * localposition;
                }
            }

            public Quaternion Rotation
            {
                get
                {
                    return Room.Rotation * localrotation;
                }
            }

            public bool Equals(ItemSpawn other)
            {
                return other.roomType == roomType && other.localposition == localposition && other.localrotation == localrotation;
            }
        }

        public static readonly HashSet<ItemSpawn> AllPoses = new();
        private static HashSet<ItemSpawn> UsablePoses = new();
        public static IReadOnlyCollection<ItemSpawn> usablePose => UsablePoses;

        public static ItemSpawn UseRandomPose(RoomType roomType)
        {

            if (UsablePoses.Count(r => r.roomType == roomType) <= 0)
            {
                return null;
            }
            Log.Debug("count before =" + UsablePoses.Count(r => r.roomType == roomType));
            ItemSpawn result = UsablePoses.GetRandomValue(r => r.roomType == roomType);
            UsablePoses.Remove(result);
            Log.Debug("count after =" + UsablePoses.Count(r => r.roomType == roomType));
            return result;

        }

        public static Vector3 ConvertLocal(Pose pose, Quaternion newRot)
        {

            Vector3 originalLocal = pose.position;
            Quaternion originalRot = pose.rotation;
            Vector3 worldPos = originalRot * originalLocal;

            return Quaternion.Inverse(newRot) * worldPos;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="room"></param>
        /// <param name="poses"> add the poses should be from a zero rotation room </param>
        public static void AddRoomPoses(HashSet<ItemSpawn> poses)
        {



            foreach(ItemSpawn item in poses)
            {
                AllPoses.Add(item);
                UsablePoses.Add(item);
            }

        }

        public static void ShowPoses(RoomType roomType)
        {
            List<Primitive> primitives = new();
            Room room = Room.Get(roomType);
            foreach (ItemSpawn pose in AllPoses.Where(p => p.roomType == roomType))
            {


                Log.Info(pose.localposition);
                Log.Info(pose.Position);
                Color color = Color.red;

                if (UsablePoses.Contains(pose))
                {
                    color = Color.green;
                }


                Primitive prim = Primitive.Create(pose.Position, null, Vector3.one * .1f, false, color);

                prim.Spawn();
                primitives.Add(prim);

                
            }
           

            Timing.CallDelayed(5, delegate
            {
                foreach(Primitive primive in primitives)
                {
                    primive.Destroy();
                }
            });
        }

        
    }


    
}
