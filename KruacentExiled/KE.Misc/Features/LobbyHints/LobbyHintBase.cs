using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features.LobbyHints
{
    internal abstract class LobbyHintBase : IUsingEvents
    {


        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Verified += OnVerified;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Verified -= OnVerified;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        }
        
        

        private void OnVerified(VerifiedEventArgs ev)
        {
            if (IsNotLobby()) return;
            Init(ev.Player);

        }


        private void OnRoundStarted()
        {
            Destroy();
        }


        public abstract void Init(Player player);
        public abstract void Destroy();

        protected bool IsNotLobby()
        {
            return !Round.IsLobby;
        }

    }
}
