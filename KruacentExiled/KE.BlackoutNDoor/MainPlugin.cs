using KE.BlackoutNDoor.API.Features;
using KE.BlackoutNDoor.Handlers;
using Exiled.API.Features;
using System;
using System.ComponentModel;
using Server = Exiled.Events.Handlers.Server;

namespace KE.BlackoutNDoor
{
    public class MainPlugin : Plugin<Config>
    {
        public override string Author => "Patrique";
        public override Version Version => new Version(1,0,0);
        public override string Name => "KE.BlackoutDoor";
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
