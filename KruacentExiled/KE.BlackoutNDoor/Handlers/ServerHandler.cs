using KE.BlackoutNDoor.API.Features;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using MEC;
using PluginAPI.Events;
using System.Collections.Generic;

namespace KE.BlackoutNDoor.Handlers
{
    public class ServerHandler
    {
        public int Cooldown { get; set; } = -1 ;
        internal double ChanceBO { get; set; } = MainPlugin.Instance.Config.InitialChanceBO;
        private readonly Controller  Controller;
        private static CoroutineHandle Handle;
        internal ServerHandler(Controller con)
        {
            Controller = con;
        }
        internal void OnRoundStarted()
        {
            Log.Debug($"handle = {Handle}");
            Timing.KillCoroutines(Handle);
            Handle = Timing.RunCoroutine(Update());
            
        }
        

        private IEnumerator<float> Update()
        {
            yield return Timing.WaitUntilTrue(() => Round.InProgress);
            Log.Debug("startUpdate");
            int wait;
            if (Cooldown == -1)
                wait = UnityEngine.Random.Range(MainPlugin.Instance.Config.MinInterval, MainPlugin.Instance.Config.MaxInterval);
            else
                wait = Cooldown;
            Log.Debug($"waiting for {wait}");
            yield return Timing.WaitForSeconds(wait);
            while (Round.InProgress)
            {
                var a = UnityEngine.Random.value;
                Log.Debug("random =" + a);
                if (Round.InProgress)
                {
                    if (a <= ChanceBO)
                    {
                        Log.Debug("BlackOut");

                        CoroutineHandle coroutine = Timing.RunCoroutine(Controller.RandomBlackout());
                        yield return Timing.WaitUntilDone(coroutine);
                    }
                    else
                    {
                        Log.Debug("DoorStuck");
                        CoroutineHandle coroutine = Timing.RunCoroutine(Controller.RandomDoorStuck());
                        yield return Timing.WaitUntilDone(coroutine);
                    }
                }
                wait = UnityEngine.Random.Range(MainPlugin.Instance.Config.MinInterval, MainPlugin.Instance.Config.MaxInterval);
                Log.Debug($"waiting : {wait}");
                yield return Timing.WaitForSeconds(wait);
                yield return Timing.WaitUntilFalse(() => Warhead.IsInProgress);

                ChanceBO = -(1 / 60) * Round.ElapsedTime.TotalMinutes + 0.5;
                Log.Debug($"new ChanceBO = {ChanceBO}");
            }
        }
        
    }
}
