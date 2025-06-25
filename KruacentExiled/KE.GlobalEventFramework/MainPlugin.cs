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
using KE.Utils.API.Displays.DisplayMeow;
namespace KE.GlobalEventFramework
{
    internal class MainPlugin : Plugin<Config>
	{
        public override string Author => "Patrique";
        public override string Name => "KE.GEFramework";
        public override Version Version => new Version(2, 0, 0);
        public override PluginPriority Priority => PluginPriority.Highest;



		public static readonly HintPlacement GEAnnouncement = new(0, 50, HintServiceMeow.Core.Enum.HintAlignment.Center);
		public static readonly HintPlacement GEEffect = new(360, 300, HintServiceMeow.Core.Enum.HintAlignment.Right);

        internal static MainPlugin Instance {get;private set;}

		public override void OnEnabled()
		{

            Instance = this;

			KEEvents.OnEnabled();

			base.OnEnabled();
        }

		public override void OnDisabled()
		{
			KEEvents.OnDisabled();
			base.OnDisabled();
            Instance = null;
		}

		


	}
}
