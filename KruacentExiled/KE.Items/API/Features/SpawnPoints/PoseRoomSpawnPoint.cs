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

            
            /// <summary>
            /// don't use the corridor
            /// </summary>
            /// <param name="roomType"></param>
            /// <param name="position"></param>
            /// <param name="rotation"></param>
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
                    return Room.List.FirstOrDefault(r => r.Type == roomType);
                }
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


            public bool Equals(ItemSpawn other)
            {
                return other.roomType == roomType && other.localposition == localposition && other.localrotation == localrotation;
            }
        }

        public static readonly HashSet<ItemSpawn> AllPoses = new HashSet<ItemSpawn>();
        private static HashSet<ItemSpawn> usablePoses = new HashSet<ItemSpawn>();
        public static IReadOnlyCollection<ItemSpawn> UsablePoses => usablePoses;

        public static ItemSpawn UseRandomPose(RoomType roomType)
        {

            if (UsablePoses.Count(r => r.roomType == roomType) <= 0)
            {
                return null;
            }
            Log.Debug("count before =" + UsablePoses.Count(r => r.roomType == roomType));
            ItemSpawn result = UsablePoses.GetRandomValue(r => r.roomType == roomType);
            usablePoses.Remove(result);
            Log.Debug("count after =" + UsablePoses.Count(r => r.roomType == roomType));
            return result;

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
                usablePoses.Add(item);
            }

        }

        public static void Reset()
        {
            usablePoses = AllPoses.ToHashSet();
        }

        

    }


    
}
