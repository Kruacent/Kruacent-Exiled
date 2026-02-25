using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KE.Utils.API.Interfaces;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features.PatchNotes
{
    internal class PatchNote : MiscFeature
    {
        public static HintPosition HintPosition = new PatchNotesPosition();
        public override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Joined += OnJoined;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }

        public override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Joined -= OnJoined;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        }


        private void OnJoined(JoinedEventArgs ev)
        {
            Player player = ev.Player;

            if (ev.Player is null)
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
                    var hint = DisplayHandler.Instance.CreateAuto(player, (args) => GetPatchNotes(), HintPosition.HintPlacement);
                    hint.FontSize = 20;
                }
            });

        }


        private void OnRoundStarted()
        {
            foreach (Player player in Player.List)
            {
                DisplayHandler.Instance.RemoveHint(player, HintPosition.HintPlacement);
            }

        }


        public const string PatchNoteStart = "Patch Notes :\n";

        private static string cachedPatchNote = PatchNoteNotFound;
        private const string PatchNoteNotFound = " ";


        private string GetPatchNotes()
        {
            if(cachedPatchNote == PatchNoteNotFound)
            {
                cachedPatchNote = PatchNoteStart + MainPlugin.Instance.Config.PatchNote;
            }

            return cachedPatchNote;

        }


        public static void Reload()
        {
            cachedPatchNote = PatchNoteNotFound;
        }

    }
}
