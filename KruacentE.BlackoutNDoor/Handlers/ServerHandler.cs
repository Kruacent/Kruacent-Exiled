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
        internal double ChanceBO { get; set; } = Config.InitialChanceBO;
        private Config conf;
        private Controller controller;
        internal ServerHandler(Config c,Controller con)
        {
            conf = c;
            controller = con;
        }
        internal ServerHandler(Config c)
        {
            conf = c;
        }
        public void OnWaitingForPlayers()
        {
            Timing.KillCoroutines();
        }

        public void OnRoundStarted()
        {
            //Timing.RunCoroutine(Update());
        }
        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            Log.Debug("autre end round");
            Timing.KillCoroutines();
        }

        public void OnEndingRound(EndingRoundEventArgs ev)
        {
            Log.Debug("end round");
            Timing.KillCoroutines();
        }

        private IEnumerator<float> Update()
        {
            Log.Debug("startUpdate");
            //yield return Timing.WaitForSeconds(UnityEngine.Random.Range(Config.MinInterval, Config.MaxInterval));
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
                
                yield return Timing.WaitForSeconds(UnityEngine.Random.Range(Config.MinInterval, Config.MaxInterval));
                yield return Timing.WaitUntilFalse(() => Warhead.IsInProgress);
                ChanceBO = -(1 / 60) * Round.ElapsedTime.TotalMinutes + 0.5;
            }
        }
    }
}
