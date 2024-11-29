using BlackoutKruacent.API.Features;
using BlackoutKruacent.Handlers;
using Exiled.API.Features;
using System.ComponentModel;
using Server = Exiled.Events.Handlers.Server;

namespace BlackoutKruacent
{
    public class MainPlugin : Plugin<Config>
    {
        private Controller _c;
        private ServerHandler _server;

        public override void OnEnabled()
        {
            base.OnEnabled();
            //_c = new Controller();
            this.RegisterEvent();
        }
        public override void OnDisabled()
        {
            base.OnDisabled();
            //_c = null;
            this.UnregisterEvent();
        }

        private void RegisterEvent() 
        {
            //_server = new ServerHandler(this.Config);
            _server = new ServerHandler(this.Config);
            Server.RoundStarted += _server.OnRoundStarted;
            Server.WaitingForPlayers += _server.OnWaitingForPlayers;
            Server.EndingRound += _server.OnEndingRound;
            Server.RoundEnded += _server.OnRoundEnded;
        }
        private void UnregisterEvent() 
        {
            Server.RoundStarted -= _server.OnRoundStarted;
            Server.WaitingForPlayers -= _server.OnWaitingForPlayers;
            Server.EndingRound -= _server.OnEndingRound;
            Server.RoundEnded -= _server.OnRoundEnded;
            _server = null;
        }





    }
}
