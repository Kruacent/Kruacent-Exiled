using BlackoutKruacent.API.Features;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using MEC;
using PluginAPI.Events;
using System.Collections.Generic;

namespace BlackoutKruacent.Handlers
{
    internal class ServerHandler
    {
        internal double ChanceBO { get; set; } = MainPlugin.Instance.Config.InitialChanceBO;
        private Controller controller;
        internal ServerHandler(Controller con)
        {
            controller = con;
        }
        public void OnRoundStarted()
        {
            Timing.RunCoroutine(Update());
        }

        private IEnumerator<float> Update()
        {
            Log.Debug("startUpdate");
            var wait = UnityEngine.Random.Range(MainPlugin.Instance.Config.MinInterval, MainPlugin.Instance.Config.MaxInterval);
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
            Log.Debug("end");
        }
        
    }
}
