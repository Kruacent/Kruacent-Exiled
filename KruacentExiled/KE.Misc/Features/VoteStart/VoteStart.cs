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
using UnityEngine;

namespace KE.Misc.Features.VoteStart
{
    internal class VoteStart : MiscFeature
    {

        public static HintPosition HintPosition = new VotePosition();

        private List<Player> Voted = new();
        private bool voteCasted = false;
        public override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.VoiceChatting += OnVoiceChatting;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Player.Left += OnLeft;
            Exiled.Events.Handlers.Player.Joined += OnJoined;
            base.SubscribeEvents();
        }


        public override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.VoiceChatting -= OnVoiceChatting;
            Exiled.Events.Handlers.Player.Joined -= OnJoined;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Player.Left -= OnLeft;

            base.UnsubscribeEvents();
        }


        private void OnLeft(LeftEventArgs ev)
        {
            Player player = ev.Player;
            if (DidVote(player))
            {
                CancelVote(player);
            }
        }


        public bool DidVote(Player player)
        {
            return Voted.Contains(player);
        }

        public void CancelVote(Player player)
        {
            if (!Voted.Contains(player))
            {
                throw new ArgumentOutOfRangeException($"Player ({player}) didn't vote");
            }


            Voted.Remove(player);
            Round.IsLobbyLocked = true;
            voteCasted = false;
        }



        private void OnVoiceChatting(VoiceChattingEventArgs ev)
        {
            if (voteCasted) return;
            if (!Round.IsLobby) return;
            if (ev.Player is null)
            {
                return;
            }

            if (string.IsNullOrEmpty(ev.Player.AuthenticationToken))
            {
                return;
            }

            if (DidVote(ev.Player))
            {
                return;
            }

            Voted.Add(ev.Player);
            if (Voted.Count >= MainPlugin.Instance.Config.MinPlayerVote)
            {
                Log.Info("starting the round");
                Round.IsLobbyLocked = false;
                voteCasted = true;
            }
        }
        private void OnWaitingForPlayers()
        {
            Init();
            Round.IsLobbyLocked = true;
        }

        private void Init()
        {
            Voted.Clear();
            voteCasted = false;
        }

        private void OnJoined(JoinedEventArgs ev)
        {
            Player player = ev.Player;

            if(ev.Player is null)
            {
                return;
            }


            if (!ev.Player.IsConnected)
            {
                return;
            }

            Timing.CallDelayed(1f, () =>
            {
                if (Round.IsLobby)
                {
                    DisplayHandler.Instance.CreateAuto(player, (args) => GetPlayers(player), HintPosition.HintPlacement);
                }
            });
            
        }

        private string GetPlayers(Player player)
        {
            if (!Round.IsLobby) return string.Empty;

            StringBuilder sb = StringBuilderPool.Pool.Get();

            sb.Append("Votes (");

            sb.Append(Voted.Count);
            sb.Append("/");
            sb.Append(MainPlugin.Instance.Config.MinPlayerVote);

            sb.AppendLine(") : ");
            foreach (Player other in Voted)
            {
                bool flag1 = other == player;


                if (flag1)
                {
                    sb.Append("<b>");
                }
                sb.Append(other.Nickname);
                if (flag1)
                {
                    sb.Append("</b>");
                }
                sb.Append(" ");
            }
            if (Voted.Contains(player))
            {
                sb.AppendLine();
                sb.Append("<size=14>.rv dans la console client pour annuler le vote</size>.");
            }


            return StringBuilderPool.Pool.ToStringReturn(sb);

        }
        private void OnRoundStarted()
        {
            foreach(Player player in Player.List)
            {
                DisplayHandler.Instance.RemoveHint(player, HintPosition.HintPlacement);
            }

        }


    }
}
