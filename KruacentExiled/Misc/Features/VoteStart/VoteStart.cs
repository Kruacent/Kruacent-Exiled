using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using MEC;
using System;
using System.Collections.Generic;
using System.Text;

namespace KruacentExiled.Misc.Features.VoteStart
{
    internal class VoteStart : MiscFeature
    {
        public const string CancelVoteTranslation = "CancelVote";
        //public static Dictionary<string, Dictionary<string, string>> LangToKeyToTranslation { get; } = new()
        //{
        //    ["en"] =
        //    {
        //        [CancelVoteTranslation] = "<size=14>.rv in the console to cancel your vote</size>."
        //    },
        //    ["fr"] =
        //    {
        //        [CancelVoteTranslation] = "<size=14>.rv dans la console client pour annuler le vote</size>."
        //    }
        //};

        public static HintPosition HintPosition = new VotePosition();

        private readonly List<int> Voted;
        private bool voteCasted = false;

        private StringBuilder sb;

        public int MinimumVote { get; set; } = MainPlugin.Configs.MinPlayerVote;

        internal VoteStart() : base()
        {
            Voted = ListPool<int>.Pool.Get();
            sb = StringBuilderPool.Pool.Get();
        }

        ~VoteStart()
        {
            ListPool<int>.Pool.Return(Voted);
            StringBuilderPool.Pool.Return(sb);
        }


        public bool EveryoneVoted => voteCasted;
        public int NumberVote => Voted.Count;

        public override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.VoiceChatting += OnVoiceChatting;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Player.Left += OnLeft;
            Exiled.Events.Handlers.Player.Verified += OnVerified;
            base.SubscribeEvents();
        }


        public override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.VoiceChatting -= OnVoiceChatting;
            Exiled.Events.Handlers.Player.Verified -= OnVerified;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Player.Left -= OnLeft;
            base.UnsubscribeEvents();
        }


        private void OnLeft(LeftEventArgs ev)
        {
            int playerId = ev.Player.Id;
            if (DidVote(playerId))
            {
                CancelVote(playerId);
            }
        }


        public void RemoveAllVote()
        {
            Voted.Clear();
            Round.IsLobbyLocked = true;
            voteCasted = false;
        }

        /// <summary>
        /// Check if the minimum number of player voted
        /// </summary>
        /// <returns></returns>
        public bool CheckVote()
        {
            return Voted.Count >= MainPlugin.Configs.MinPlayerVote;
        }

        public bool DidVote(int playerId)
        {
            return Voted.Contains(playerId);
        }

        public void CancelVote(int playerId)
        {
            if (!Voted.Contains(playerId))
            {
                throw new ArgumentOutOfRangeException($"Player ({playerId}) didn't vote");
            }


            Voted.Remove(playerId);
            if(!CheckVote())
            {
                Round.IsLobbyLocked = true;
                voteCasted = false;
            }
            
        }


        public void AddVote(int playerId)
        {
            if (Voted.Contains(playerId))
            {
                throw new ArgumentException($"Player ({playerId}) already voted");
            }

            Voted.Add(playerId);
            if (CheckVote())
            {
                Round.IsLobbyLocked = false;
                voteCasted = true;
            }
        }



        private void OnVoiceChatting(VoiceChattingEventArgs ev)
        {
            if (EveryoneVoted) return;
            if (!Round.IsLobby) return;
            if (ev.Player is null)
            {
                return;
            }

            if (string.IsNullOrEmpty(ev.Player.AuthenticationToken))
            {
                return;
            }
            int playerId = ev.Player.Id;

            if (DidVote(playerId))
            {
                return;
            }

            AddVote(playerId);



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

        private void OnVerified(VerifiedEventArgs ev)
        {
            Player player = ev.Player;

            if(ev.Player == null)
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
            if (!Round.IsLobby) return " ";
            if (Voted == null) return " ";
            if (player == null) return " ";

            sb.Clear();

            sb.Append("Votes (");

            sb.Append(Voted.Count);
            sb.Append("/");
            sb.Append(MainPlugin.Configs.MinPlayerVote);

            sb.AppendLine(") : ");
            int playerId = player.Id;
            foreach (int otherId in Voted)
            {

                Player other = Player.Get(otherId);

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
            if (DidVote(playerId))
            {
                sb.AppendLine();

                sb.Append("<size=14>.rv dans la console client pour annuler le vote</size>.");
                //sb.Append(MainPlugin.GetTranslation(player,CancelVoteTranslation));
            }


            return sb.ToString();

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
