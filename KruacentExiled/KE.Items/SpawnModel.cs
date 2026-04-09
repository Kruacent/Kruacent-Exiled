using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using KE.Items.Items.PickupModels;
using KE.Utils.API.Features.Models;
using KE.Utils.Extensions;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Items
{

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SpawnModel : ICommand
    {
        public string Command => "spawnmodel";

        public string[] Aliases => [];

        public string Description => "spawnmodel";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var p = Player.Get(sender);
            response = string.Empty;

            if(p is not null)
            {
                Primitive prim = Primitive.Create(p.Position, null, Vector3.one, false);
                prim.Collidable = false;
                prim.Visible = false;
                prim.Spawn();
                TPGrenadaPModel m = new(null);
                

                Log.Info("position model=" + prim.Position);

                m.Create(prim.Transform);

                Timing.CallDelayed(float.Parse(arguments.At(0)),prim.Destroy);

                return true;
            }
            response = "no";
            return false;
        }
    }
}
