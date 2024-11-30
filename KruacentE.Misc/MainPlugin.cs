using Exiled.API.Features;
using ServerHandle = Exiled.Events.Handlers.Server;

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
            Log.Debug($"Friendly Fire : {Exiled.API.Features.Server.FriendlyFire}");
        }
        

    }
}
