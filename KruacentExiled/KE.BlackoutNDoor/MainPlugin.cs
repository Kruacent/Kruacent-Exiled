using BlackoutKruacent.API.Features;
using BlackoutKruacent.Handlers;
using Exiled.API.Features;
using System.ComponentModel;
using Server = Exiled.Events.Handlers.Server;

namespace BlackoutKruacent
{
    public class MainPlugin : Plugin<Config>
    {
        public override string Name => "BlackOutNDoors";
        internal static MainPlugin Instance;
        private Controller Controller;
        public ServerHandler ServerHandler { get; private set; }

        public override void OnEnabled()
        {
            Instance = this;
            Controller = new Controller();
            this.RegisterEvent();
        }
        public override void OnDisabled()
        {

            Instance = null;
            Controller = null;
            this.UnregisterEvent();
        }

        private void RegisterEvent() 
        {
            ServerHandler = new ServerHandler(Controller);
            Server.RoundStarted += ServerHandler.OnRoundStarted;

        }
        private void UnregisterEvent() 
        {
            Server.RoundStarted -= ServerHandler.OnRoundStarted;

            ServerHandler = null;
        }





    }
}
