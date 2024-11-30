using BlackoutKruacent.API.Features;
using BlackoutKruacent.Handlers;
using Exiled.API.Features;
using System.ComponentModel;
using Server = Exiled.Events.Handlers.Server;

namespace BlackoutKruacent
{
    public class MainPlugin : Plugin<Config>
    {
        internal static MainPlugin Instance;
        private Controller _c;
        private ServerHandler _server;

        public override void OnEnabled()
        {
            Instance = this;
            _c = new Controller();
            this.RegisterEvent();
        }
        public override void OnDisabled()
        {

            Instance = null;
            _c = null;
            this.UnregisterEvent();
        }

        private void RegisterEvent() 
        {
            _server = new ServerHandler(_c);
            Server.RoundStarted += _server.OnRoundStarted;

        }
        private void UnregisterEvent() 
        {
            Server.RoundStarted -= _server.OnRoundStarted;

            _server = null;
        }





    }
}
