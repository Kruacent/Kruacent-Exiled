using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEC;
using GEFExiled.GEFE.API.Interfaces;
using Exiled.Events.Commands.PluginManager;

namespace GEFExiled.Handlers
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
            _activeGE = _plugin.ChooseGE();
            Log.Debug("sub event");
            _activeGE.ForEach(e => e.SubscribeEvent());
            Log.Debug("show to player");
            _plugin.Show();
            Log.Debug("end starting round");
        }

		public void OnEndingRound(RoundEndedEventArgs _)
		{
            Log.Debug("ending round");
            _activeGE.ForEach(e => e.UnsubscribeEvent());
            Timing.KillCoroutines();
        }
        public void OnRestartingRound()
        {
            Log.Debug("restarting");
            _activeGE.ForEach(e => e.UnsubscribeEvent());
            Timing.KillCoroutines();    
        }

    }
}
