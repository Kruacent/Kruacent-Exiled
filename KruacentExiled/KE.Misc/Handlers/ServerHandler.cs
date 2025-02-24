using System.Linq;
using Exiled.API.Features;
using MEC;

namespace KE.Misc.Handlers
{
    internal class ServerHandler
    {
        CoroutineHandle coroutineHandle;
        public void OnRoundStarted()
        {
            if(MainPlugin.Instance.Config.ChanceFF >= 0 && MainPlugin.Instance.Config.ChanceFF <= 100)
                MainPlugin.Instance.RandomFF();
            if (MainPlugin.Instance.Config.ChanceClassDDoorGoesBoom >= 0 && MainPlugin.Instance.Config.ChanceClassDDoorGoesBoom <= 100)
                MainPlugin.Instance.ClassDDoor.ClassDDoorGoesBoom();
            if(MainPlugin.Instance.Config.AutoNukeAnnoucement)
                Timing.RunCoroutineSingleton(MainPlugin.Instance.NukeAnnouncement(), coroutineHandle,SingletonBehavior.Abort);
            if(MainPlugin.Instance.Config.PeanutLockDown && Player.List.Where(p => p.Role.Type == PlayerRoles.RoleTypeId.Scp173).Count() > 0)
                Timing.RunCoroutine(MainPlugin.Instance.PeanutLockdown());
            if(MainPlugin.Instance.Config.AutoElevator)
                Timing.RunCoroutine(MainPlugin.Instance.AutoElevator.StartElevator());
            if (MainPlugin.Instance.Config.SurfaceLight && UnityEngine.Random.value < .75f)
                MainPlugin.Instance.SurfaceLight.ChangeSurfaceLight();
            MainPlugin.Instance.SCPBuff.StartBuff();
        }
    }
}
