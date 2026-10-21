using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using MEC;
using System.Text;

namespace KruacentExiled.Misc.Features.DisplayPluginVersion
{
    internal class DisplayPluginVersion : MiscFeature
    {
        public static HintPosition HintPosition = new DisplayPluginVersionPosition();

        public override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Verified += OnVerified;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            base.SubscribeEvents();
        }

        public override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Verified -= OnVerified;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            base.UnsubscribeEvents();
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
                    DisplayHandler.Instance.CreateAuto(player, (args) => GetPlayers(player), HintPosition.HintPlacement);
                }
            });

        }

        private string GetPlayers(Player player)
        {
            if (!Round.IsLobby) return string.Empty;

            StringBuilder sb = StringBuilderPool.Pool.Get();

            sb.Append("v");
            sb.Append(KruacentExiled.MainPlugin.Instance.Version);

            return StringBuilderPool.Pool.ToStringReturn(sb);
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
