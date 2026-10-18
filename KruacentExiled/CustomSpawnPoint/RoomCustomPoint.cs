using Exiled.API.Enums;
using Exiled.API.Features;
using KE.Utils.API.Features;
using KruacentExiled.CustomSpawnPoint.Spawned;
using KruacentExiled.Extensions;
using MEC;
using ProjectMER.Features.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

namespace KruacentExiled.CustomSpawnPoint
{
    public class RoomCustomPoint
    {
        private static List<RoomCustomPoint> _all = null;
        public static bool IsPopulated => _all != null;

        public const string FileName = "RoomCustomPoints";
        private static string Path => System.IO.Path.Combine(Paths.Configs,FileName);
        public static IReadOnlyList<RoomCustomPoint> All => IsPopulated ? _all.AsReadOnly() : null;
        public const char Separator = '_';

        public readonly RoomType roomType;
        public readonly Vector3 localPosition;
        public readonly Quaternion localRotation;
        public readonly RoomCustomPointType type;



        /// <summary>
        /// Create an instance of <see cref="RoomCustomPoint"/> using local coordinate
        /// </summary>
        /// <param name="roomType"></param>
        /// <param name="localPosition"></param>
        /// <param name="localRotation"></param>
        /// <param name="type"></param>
        internal RoomCustomPoint(RoomType roomType,Vector3 localPosition, Quaternion localRotation,RoomCustomPointType type)
        {
            this.roomType = roomType;
            this.localPosition = localPosition;
            this.localRotation = localRotation;
            this.type = type;

        }


        internal RoomCustomPoint(Room room, Vector3 position, Quaternion rotation, RoomCustomPointType type)
        {


            this.roomType = room.Type;
            this.localPosition = GetLocalPosition(room, position);
            this.localRotation = GetLocalRotation(room, rotation);
            this.type = type;
        }

        public void Spawn()
        {
            foreach (Room room in Room.List.Where(r => r.Type == roomType))
            {
                RoomSpawnedCustomPoint.Create(this, room);

            }

        }

        public static Vector3 GetLocalPosition(Room room,Vector3 position)
        {
            return Quaternion.Inverse(room.Rotation) * (position - room.Position);
        }
        public static Quaternion GetLocalRotation(Room room, Quaternion rotation)
        {
            return rotation * Quaternion.Inverse(room.Rotation);
        }


        private string Parse()
        {
            return roomType.ToString() + Separator + localPosition.ToString() + Separator + localRotation.ToString() + Separator + type.ToString();
        }

        private static bool _dirty = false;
        public static void Add(Room room, Vector3 position, Quaternion rotation, RoomCustomPointType type)
        {
            if(room == null)
            {
                throw new ArgumentNullException(nameof(room));
            }
            
            _all.Add(new RoomCustomPoint(room, position, rotation, type));


            _dirty = true;
        }
        internal static void Add(RoomCustomPoint roomCustomPoint)
        {
            if(roomCustomPoint is null)
            {
                throw new ArgumentNullException(nameof(roomCustomPoint));
            }

            _all.Add(roomCustomPoint);


            _dirty = true;
        }


        public static bool _finishedPopulate = true;


        public static void Populate()
        {
            if (!_finishedPopulate) return;
            Timing.RunCoroutine(PopulateAsync(Path));
        }




        private static IEnumerator<float> PopulateAsync(string fileFullPath)
        {
            _finishedPopulate = false;
            _all = new List<RoomCustomPoint>();
            string[] data;
            RoomType roomType;
            Vector3 localPos;
            Quaternion localRot;
            RoomCustomPointType type;

            try
            {
                if (!File.Exists(fileFullPath))
                {
                    Log.Warn("Room Custom Points file doesn't exist");
                    yield break;
                }


                IEnumerable<string> lines = File.ReadLines(fileFullPath);


                int numberLines = lines.Count();
                int i = 1;

                foreach (string line in lines)
                {
                    data = line.Split(Separator);

                    if (data.Length != 4)
                    {
                        Log.Warn("wrong length at line " + i);
                        continue;
                    }


                    if (!Enum.TryParse(data[0], out roomType))
                    {
                        Log.Warn($"data malformed at RoomType at line ({i}/{numberLines})");
                        continue;
                    }
                    localPos = data[1].ToVector3();
                    localRot = data[2].ToQuaternion();


                    if (!Enum.TryParse(data[3], out type))
                    {
                        Log.Warn($"data malformed at RoomCustomPointType at line ({i}/{numberLines})");
                        continue;
                    }

                    KELog.Debug("registered rcp at " + roomType);

                    RoomCustomPoint roomCustomPoint = new RoomCustomPoint(roomType, localPos, localRot, type);

                    _all.Add(roomCustomPoint);
                    i++;

                }
            }
            catch(Exception e)
            {
                Log.Error(e);
            }
            finally
            {
                _finishedPopulate = true;
            }

            
            yield return 0;
        }


        public static void WriteToFile()
        {
            if (!IsPopulated) return;
            if (!_dirty) return;
            Timing.RunCoroutine(WriteToFileAsync(Path));
        }



        private static IEnumerator<float> WriteToFileAsync(string fileFullPath)
        {
            
            string[] lines = new string[All.Count];

            for(int i =0; i < lines.Length; i++)
            {
                lines[i] = All[i].Parse();
            }


            File.WriteAllLines(fileFullPath, lines);

            Log.Info("create file at " + fileFullPath);

            _dirty = false;


            yield return 0;
        }
    }
}
