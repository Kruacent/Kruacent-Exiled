using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features
{
    internal class VoteStart : MiscFeature
    {

        public int minvote = 9999;


        public HashSet<Player> Voted = new();
        private bool voteCasted = false;

        public override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.VoiceChatting += OnVoiceChatting;
            minvote = MainPlugin.Instance.Config.MinPlayerVote;

            base.SubscribeEvents();
        }


        public override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.VoiceChatting -= OnVoiceChatting;
            base.UnsubscribeEvents();
        }



        private void OnVoiceChatting(VoiceChattingEventArgs ev)
        {
            if (!Round.IsLobby) return;
            if (voteCasted) return;

            if (Voted.Add(ev.Player))
            {
                if(Voted.Count >= minvote)
                {
                    Round.IsLobbyLocked = false;
                    voteCasted = true;
                }
            }
        }
        private void OnWaitingForPlayers()
        {
            Round.IsLobbyLocked = true;
        }


    }
}
