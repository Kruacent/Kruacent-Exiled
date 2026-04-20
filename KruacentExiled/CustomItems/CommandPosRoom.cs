using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using KE.Utils.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static KruacentExiled.CustomItems.API.Features.SpawnPoints.PoseRoomSpawnPointHandler;

namespace KruacentExiled.CustomItems
{

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class CommandPosRoom : ICommand
    {
        public string Command => "roompos";

        public string[] Aliases => new string[0];

        public string Description => "position";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var p = Player.Get(sender);
            response = string.Empty;

            if(p != null)
            {
                Room room = p.CurrentRoom;

                if(room is null)
                {
                    response = "no room";
                    return false;
                }

                ShowPoses(room.Type);
                response= "ok";
                return true;
            }
            response = "no";
            return false;
        }

        public static void ShowPoses(RoomType roomType)
        {
            List<Primitive> primitives = new List<Primitive>();
            Room room = Room.Get(roomType);
            foreach (ItemSpawn pose in AllPoses.Where(p => p.roomType == roomType))
            {


                KELog.Debug(pose.localposition);
                KELog.Debug(pose.Position);
                Color color = Color.red;

                if (UsablePoses.Contains(pose))
                {
                    color = Color.green;
                }


                Primitive prim = Primitive.Create(pose.Position, null, Vector3.one * .1f, false, color);
                prim.Collidable = false;
                prim.Spawn();
                primitives.Add(prim);


            }


            Timing.CallDelayed(5, delegate
            {
                foreach (Primitive primive in primitives)
                {
                    primive.Destroy();
                }
            });
        }
    }
}
