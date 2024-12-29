using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using System.Collections.Generic;
using KruacentE.GlobalEventFramework.GEFE.API.Interfaces;
using KruacentE.GlobalEventFramework.GEFE.API.Features;

namespace KruacentE.GlobalEventFramework.GEFE.Handlers
{
	internal class ServerHandler
	{
		MainPlugin _plugin;
		List<IGlobalEvent> _activeGE;
        public ServerHandler(MainPlugin mainPlugin)
		{
			this._plugin = mainPlugin;
		}
		public void OnRoundStarted()
		{
			Log.Debug("starting round");

            _activeGE = _plugin.ChooseGE(UnityEngine.Random.value < .1f ? 2 : 1);
            Log.Debug("sub event");
            _activeGE.ForEach(e => e.SubscribeEvent());
            Log.Debug("show to player");
            _plugin.Show();
            Log.Debug("end starting round");
        }

        public void OnWaitingForPlayers()
        {
            GlobalEvent.StopCoroutines();
        }

		public void OnEndingRound(RoundEndedEventArgs _)
		{
            Log.Debug("ending round");
            _activeGE.ForEach(e => e.UnsubscribeEvent());
        }
        public void OnRestartingRound()
        {
            Log.Debug("restarting");
            _activeGE.ForEach(e => e.UnsubscribeEvent());
        }

    }
}
