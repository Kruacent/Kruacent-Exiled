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
        private Controller controller;
        internal ServerHandler(Controller con)
        {
            controller = con;
        }
        internal void OnRoundStarted()
        {
            Timing.RunCoroutine(Update());
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
            while (true)
            {
                var a = UnityEngine.Random.value;
                Log.Debug("random =" + a);
                if (a <= ChanceBO)
                {
                    Log.Debug("BlackOut");
                    CoroutineHandle coroutine = Timing.RunCoroutine(controller.RandomBlackout());
                    yield return Timing.WaitUntilDone(coroutine);
                }
                else
                { 
                    Log.Debug("DoorStuck");
                    CoroutineHandle coroutine = Timing.RunCoroutine(controller.RandomDoorStuck());
                    yield return Timing.WaitUntilDone(coroutine);
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
