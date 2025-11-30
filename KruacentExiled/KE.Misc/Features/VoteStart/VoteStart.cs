using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Player;
using HintServiceMeow.Core.Models.Hints;
using HintServiceMeow.Core.Utilities;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features.VoteStart
{
    internal class VoteStart : MiscFeature
    {

        public int minvote = 9999;
        public static HintPosition HintPosition = new VotePosition();

        public HashSet<Player> Voted = new();
        private bool voteCasted = false;
        private StringBuilder sb = null;
        public override void SubscribeEvents()
        {
            sb = StringBuilderPool.Pool.Get();
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.VoiceChatting += OnVoiceChatting;
            Exiled.Events.Handlers.Player.Joined += OnJoined;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;

            minvote = MainPlugin.Instance.Config.MinPlayerVote;

            base.SubscribeEvents();
        }


        public override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.VoiceChatting -= OnVoiceChatting;
            Exiled.Events.Handlers.Player.Joined -= OnJoined;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;

            if(sb is not null)
            {
                StringBuilderPool.Pool.Return(sb);
                sb = null;
            }

            base.UnsubscribeEvents();
        }



        private void OnVoiceChatting(VoiceChattingEventArgs ev)
        {
            if (!Round.IsLobby) return;
            if (voteCasted) return;

            Voted.Add(ev.Player);
            if (Voted.Count >= minvote)
            {
                Round.IsLobbyLocked = false;
                voteCasted = true;
            }
        }
        private void OnWaitingForPlayers()
        {
            Round.IsLobbyLocked = true;
        }

        private void OnJoined(JoinedEventArgs ev)
        {
            Player player = ev.Player;

            Timing.CallDelayed(.5f, () =>
            {
                if (player is not null && Round.IsLobby)
                {
                    PlayerDisplay dis = PlayerDisplay.Get(player);
                    DisplayHandler.Instance.CreateAuto(player, (args) => GetPlayers(), HintPosition.HintPlacement);
                }
            });
            
        }

        private string GetPlayers()
        {
            if (!Round.IsLobby) return string.Empty;
            sb.Clear();

            sb.Append("Players who voted (");

            sb.Append(Voted.Count);
            sb.Append("/");
            sb.Append(minvote);

            sb.AppendLine(") : ");
            foreach(Player player in Voted)
            {
                sb.Append(player.Nickname);
                sb.Append(" ");
            }

            return sb.ToString();

        }
        private void OnRoundStarted()
        {
            foreach(Player player in Player.List)
            {
                AbstractHint hint = DisplayHandler.Instance.GetHint(player, HintPosition.HintPlacement);
                if (hint is not null)
                {
                    hint.Hide = true;
                }
            }

            if (sb is not null)
            {
                StringBuilderPool.Pool.Return(sb);
                sb = null;
            }
        }


    }
}
