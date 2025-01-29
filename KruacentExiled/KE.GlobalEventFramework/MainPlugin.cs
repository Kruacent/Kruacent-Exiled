using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using MEC;
using Exiled.API.Enums;
using Player = Exiled.API.Features.Player;
using KE.GlobalEventFramework.GEFE.API.Features;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using ServerHandler = Exiled.Events.Handlers.Server;
using Discord;
namespace KE.GlobalEventFramework
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
			_server = new GEFE.Handlers.ServerHandler();


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

		


	}
}
