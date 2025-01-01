using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using System.Collections.Generic;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using KE.GlobalEventFramework.GEFE.API.Features;
using KE.GlobalEventFramework.GEFE.Commands;

namespace KE.GlobalEventFramework.GEFE.Handlers
{
	internal class ServerHandler
	{
		public void OnRoundStarted()
		{
            Log.Debug("starting round");
            HandleCommands();



        }

        private void HandleCommands()
        {
            //force ge
            if (ForceGE.ForcedGE.Count > 0)
            {
                Log.Debug("forcing ge");
                GlobalEvent.ActiveGE = ForceGE.ForcedGE;
                ForceGE.ForcedGE = new List<IGlobalEvent>();
            }
            else
            {
                int nbGE;

                //choose nb of ge
                if (ForceNbGE.NbGE > -1)
                {
                    nbGE = ForceNbGE.NbGE;
                    Log.Debug($"forcing nb ge = {nbGE}");
                    ForceNbGE.NbGE = -1;
                }
                //normal case
                else
                {
                    Log.Debug($"no commands");
                    nbGE = UnityEngine.Random.value < .1f ? 2 : 1;
                }
                GlobalEvent.ActiveGE = GlobalEvent.ChooseGE(nbGE);
            }

            GlobalEvent.ActivateAll();
        }

        public void OnWaitingForPlayers()
        {
            GlobalEvent.StopCoroutines();
        }

		public void OnEndingRound(RoundEndedEventArgs _)
		{
            Log.Debug("ending round");
            GlobalEvent.ActiveGE.ForEach(e => e.UnsubscribeEvent());
        }
        public void OnRestartingRound()
        {
            Log.Debug("restarting");
            GlobalEvent.ActiveGE.ForEach(e => e.UnsubscribeEvent());
        }

    }
}
