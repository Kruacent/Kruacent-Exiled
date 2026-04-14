using Exiled.API.Features;
using System;
using Exiled.API.Enums;
using KE.GlobalEventFramework.GEFE.API.Features;
using KE.Utils.API.Displays.DisplayMeow;
using KruacentExiled;
using Exiled.API.Interfaces;
namespace KE.GlobalEventFramework
{
    internal class MainPlugin : KEPlugin
	{
        public override string Name => "KE.GEFramework";
        public override string Prefix => "KE.GEF";



		public static readonly HintPlacement GEAnnouncement = new(0, 50, HintServiceMeow.Core.Enum.HintAlignment.Center);
		public static readonly HintPlacement GEEffect = new(360, 300, HintServiceMeow.Core.Enum.HintAlignment.Right);


        internal static MainPlugin Instance { get; private set;}

        public override IConfig Config => config;
		private Config config;
		public static Config Configs => Instance?.config;

        public override void OnEnabled()
		{
			config = KruacentExiled.MainPlugin.Instance.Config.GEFConfig;
            Instance = this;


            KEEvents.OnEnabled();

        }

		public override void OnDisabled()
		{
			KEEvents.OnDisabled();
            Instance = null;
		}

		


	}
}
