using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using MEC;
using Exiled.API.Enums;
using Player = Exiled.API.Features.Player;
using KruacentE.GlobalEventFramework.GEFE.API.Features;
using KruacentE.GlobalEventFramework.GEFE.API.Interfaces;
using ServerHandler = Exiled.Events.Handlers.Server;
namespace KruacentE.GlobalEventFramework
{
    internal class MainPlugin : Plugin<Config>
	{
        public override string Author => "Patrique";
        public override string Name => "KE.GEFramework";
        public override Version Version => new Version(1, 0, 0);
        public override PluginPriority Priority => PluginPriority.Highest;

		internal GEFE.Handlers.ServerHandler _server;

		internal static MainPlugin Instance {get;private set;}

		public override void OnEnabled()
		{

            Instance = this;
			Loader.Instance.Load();
			


			RegisterEvents();
			
			base.OnEnabled();
        }

		public override void OnDisabled()
		{
			UnregisterEvents();
			Timing.KillCoroutines();
			base.OnDisabled();
            Instance = null;
		}

		private void RegisterEvents()
		{
			_server = new GEFE.Handlers.ServerHandler(this);


			ServerHandler.WaitingForPlayers += _server.OnWaitingForPlayers;
            ServerHandler.RoundStarted += _server.OnRoundStarted;
            ServerHandler.RoundEnded += _server.OnEndingRound;
			ServerHandler.RestartingRound += _server.OnRestartingRound;

		}

		private void UnregisterEvents()
		{
            ServerHandler.WaitingForPlayers -= _server.OnWaitingForPlayers;
            ServerHandler.RoundStarted -= _server.OnRoundStarted;
            ServerHandler.RoundEnded -= _server.OnEndingRound;
            ServerHandler.RestartingRound -= _server.OnRestartingRound;

            _server = null;
		}

		public void Show()
		{
			var random = UnityEngine.Random.value;

            foreach (Player player in Player.List)
			{
                Exiled.API.Features.Broadcast b = new Exiled.API.Features.Broadcast
                {
                    Content = ShowText(random > .5f),
                    Duration = 10
                };
                player.Broadcast(b);
			}
		}

		private String ShowText(bool redacted = false)
		{
			String result = "Global Events: ";
			for (int i = 0; i < GlobalEvent.ActiveGE.Count(); i++)
			{
				

                if (redacted)
				{
                    result += GlobalEvent.ActiveGE[i].Description;
				}
				else
				{
					result += "[REDACTED]";
				}
				
				if (GlobalEvent.ActiveGE.Count() > 1 && i < GlobalEvent.ActiveGE.Count()-1)
				{
					result += ",";
				}
			}


			return result;
		}

		public List<IGlobalEvent> ChooseGE(int numberOfGlobalEvent = 1)
		{
            List<IGlobalEvent> activeGE = ChooseRandomGE(numberOfGlobalEvent);
			Log.Debug($"activeGE size : {activeGE.Count}");
			Log.Info($"Global Event(s) ({numberOfGlobalEvent}): ");

			foreach (IGlobalEvent ge in activeGE)
			{
				Log.Info(ge.Name);
				var a = Timing.RunCoroutine(ge.Start());
                GlobalEvent.coroutineHandles.Add(a); //crash when using other ge from other assembly
            }
			return activeGE;
        }

		private List<IGlobalEvent> ChooseRandomGE(int nbGE = 1)
		{
			List<IGlobalEvent> result = new List<IGlobalEvent>();

			List<IGlobalEvent> weightedPool = new List<IGlobalEvent>();
			foreach (IGlobalEvent ge in GlobalEvent.GlobalEventsList)
			{
				for (int i = 0; i < ge.Weight; i++)
				{
					weightedPool.Add(ge);
					Log.Debug($"getochoose : {ge.Name} ");
				}
			}

			nbGE = Math.Min(nbGE, GlobalEvent.GlobalEventsList.Count); 

			for (int i = 0; i < nbGE; i++)
			{
				int randomIndex = UnityEngine.Random.Range(0, weightedPool.Count);
				IGlobalEvent selectedGE = weightedPool[randomIndex];

				result.Add(selectedGE);

				weightedPool.RemoveAll(e => e == selectedGE);
			}

			// Step 3: Update the active global events
			GlobalEvent.ActiveGE = result.ToList();

			return result;
		}


	}
}
