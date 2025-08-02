using System.Linq;
using Exiled.API.Features;
using MEC;

namespace KE.Misc.Handlers
{
    internal class ServerHandler
    {
        private CoroutineHandle coroutineHandle;
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
        }
    }
}
