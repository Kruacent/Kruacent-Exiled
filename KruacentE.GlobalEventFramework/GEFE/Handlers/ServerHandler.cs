using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEC;
using GEFExiled.GEFE.API.Interfaces;

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
            Log.Debug("end starting round");
        }

		public void OnEndingRound(EndingRoundEventArgs ev)
		{
            Log.Debug("ending round");
			_activeGE.ForEach(e => e.UnsubscribeEvent());
            this._plugin.StopCoroutines();
		}
	}
}
