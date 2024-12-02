using Exiled.API.Enums;
using Exiled.API.Features;
using System.Collections.Generic;
using ServerHandle = Exiled.Events.Handlers.Server;
using MEC;

namespace KE.Misc
{
    public class MainPlugin : Plugin<Config>
    {
        internal static MainPlugin Instance { get; private set; }
        private ServerHandler serverHandler;

        public override void OnEnabled()
        {
            Instance = this;

            serverHandler = new ServerHandler();
            ServerHandle.RoundStarted += serverHandler.OnRoundStarted;
        }

        public override void OnDisabled()
        {
            
            ServerHandle.RoundStarted -= serverHandler.OnRoundStarted;


            serverHandler = null;

            Instance = null;
        }




        internal void RandomFF()
        {
            if(UnityEngine.Random.Range(0,101) < Instance.Config.ChanceFF)
            {
                Server.FriendlyFire = true;
            }
            else
            {
                Server.FriendlyFire = false;
            }
            Log.Debug($"Friendly Fire : {Server.FriendlyFire}");
        }


        internal IEnumerator<float> NukeAnnouncement()
        {
            yield return Timing.WaitUntilTrue(() => 25 <= Round.ElapsedTime.TotalMinutes);
            Cassie.MessageTranslated("Warning automatic warhead will detonate in 5 minutes", 
                "Warning automatic warhead will detonate in <color=#FF0000>5</color> minutes");
        }
        

    }
}
