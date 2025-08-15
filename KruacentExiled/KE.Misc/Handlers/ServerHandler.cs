using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using KE.Misc.Features.Auto079;
using MEC;

namespace KE.Misc.Handlers
{
    internal class ServerHandler
    {
        private CoroutineHandle coroutineHandle;
        NPC079 a;
        public void OnRoundStarted()
        {
            if (MainPlugin.Instance.Config.ChanceClassDDoorGoesBoom >= 0 && MainPlugin.Instance.Config.ChanceClassDDoorGoesBoom <= 100)
                MainPlugin.Instance.ClassDDoor.ClassDDoorGoesBoom();
            if(MainPlugin.Instance.Config.AutoNukeAnnoucement)
                Timing.RunCoroutineSingleton(MainPlugin.Instance.NukeAnnouncement(), coroutineHandle,SingletonBehavior.Abort);
            if (MainPlugin.Instance.Config.PeanutLockDown)
            {
                //Timing.RunCoroutine(MainPlugin.Instance.PeanutLockdown());
            }

            if (MainPlugin.Instance.Config.AutoElevator)
                MainPlugin.Instance.AutoElevator.StartLoop();
            MainPlugin.Instance.SCPBuff.StartBuff();


            new NPC079();


            foreach(Player p in Player.List.Where(p => !p.IsNPC))
            {
                p.Teleport(RoomType.Hcz939);
            }

        }
    }
}
