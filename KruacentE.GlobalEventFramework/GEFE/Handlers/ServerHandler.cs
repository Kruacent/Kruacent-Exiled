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
            _activeGE.ForEach(e => e.SubscribeEvent());
			_plugin.Show();
            Log.Debug("end starting round");
        }

		public void OnEndingRound(EndingRoundEventArgs ev)
		{
            Log.Debug("ending round");
            
            
            Log.Debug("stopping coroutine");
            //this._plugin.StopCoroutines();
            Log.Debug("unsubbing events");
            _activeGE.ForEach(e => e.UnsubscribeEvent());
            Log.Debug("round end");
        }
	}
}
