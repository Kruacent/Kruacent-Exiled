using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using MEC;
using System;
using System.Diagnostics;
using System.Text;

namespace KruacentExiled.Misc.Features.DisplayPluginVersion
{
    internal class DisplayPluginVersion : MiscFeature
    {
        public static HintPosition HintPosition = new DisplayPluginVersionPosition();

        private StringBuilder stringBuilder;
        private Stopwatch stopwatch;

        public override void SubscribeEvents()
        {
            stringBuilder = StringBuilderPool.Pool.Get();
            stopwatch = new Stopwatch();
            Exiled.Events.Handlers.Player.Verified += OnVerified;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            base.SubscribeEvents();
        }

        public override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Verified -= OnVerified;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            StringBuilderPool.Pool.Return(stringBuilder);
            stopwatch.Stop();
            stopwatch = null;

            base.UnsubscribeEvents();
        }

        private void OnWaitingForPlayers()
        {
            stopwatch.Restart();
        }

        private void OnVerified(VerifiedEventArgs ev)
        {
            Player player = ev.Player;

            if (ev.Player == null)
            {
                return;
            }

            Timing.CallDelayed(1f, () =>
            {
                if (Round.IsLobby)
                {
                    //probably better with the Addhint
                    DisplayHandler.Instance.CreateAuto(player, (args) => GetPlayers(), HintPosition.HintPlacement,HintServiceMeow.Core.Enum.HintSyncSpeed.Slow);
                }
            });
            
        }

        private string GetPlayers()
        {
            if (!Round.IsLobby) return string.Empty;
            stringBuilder.Clear();
            stringBuilder.Append('v');
            stringBuilder.Append(KruacentExiled.MainPlugin.Instance.Version);

            if (KruacentExiled.MainPlugin.Instance.Config.Debug)
            {
                stringBuilder.Append(" -  DEBUG");
            }

            double time = Math.Floor(stopwatch.Elapsed.TotalMinutes);

            stringBuilder.Append("   Lobby time : ")
                .Append(time.ToString())
                .Append(" minute");

            if(time != 1)
            {
                stringBuilder.Append('s');
            }

            return stringBuilder.ToString();
        }

        private void OnRoundStarted()
        {
            foreach (Player player in Player.List)
            {
                DisplayHandler.Instance.RemoveHint(player, HintPosition.HintPlacement);
            }

        }
    }
}
