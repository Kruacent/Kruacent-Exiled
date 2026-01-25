using System.Drawing;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.Misc.Features.Auto079;
using KE.Misc.Utils;
using MEC;
using PlayerRoles;
using UnityEngine;
using TextToy = LabApi.Features.Wrappers.TextToy;

namespace KE.Misc.Handlers
{
    internal class ServerHandler
    {
        public void OnRoundStarted()
        {
            if (MainPlugin.Instance.Config.AutoElevator)
                MainPlugin.Instance.AutoElevator.StartLoop();

            MainPlugin.Instance.AutoTesla.StartLoop();
            MainPlugin.Instance.SCPBuff.StartBuff();


            /*
            string test = "test.png";
            string kel = "nokel.png";
            string tenna = "tennasmooving.gif";
            string smol = "smol.gif";

            Image img = Image.FromFile(Paths.Configs + $"/{kel}");
            Image gif = Image.FromFile(Paths.Configs + $"/{tenna}");
            

            TextImage text = new TextImage(img);
            AnimatedTextImage giftext = new(gif);
            Vector3 basePos = RoleTypeId.Scp049.GetRandomSpawnLocation().Position;

            Round.IsLocked = true;
            var pl = Player.List.FirstOrDefault();
            Timing.CallDelayed(2f, delegate
            {
                pl.Role.Set(RoleTypeId.NtfSpecialist);
            });


            Timing.CallDelayed(5f, delegate
            {
                giftext.Spawn(pl?.Position + Vector3.up ?? basePos, Quaternion.Euler(Vector3.zero));
                //text.Spawn(pl?.Position+Vector3.up ?? basePos, .05f);
            });

            
            text.Spawn(basePos + Vector3.back);
            foreach(Player p in Player.List.Where(p => !p.IsNPC))
            {
                p.Teleport(RoomType.Hcz939);
            }
            */
        }
    }
}
