using Exiled.API.Features;
using System;
using Exiled.API.Enums;
using KE.GlobalEventFramework.GEFE.API.Features;
using KE.Utils.API.Displays.DisplayMeow;
namespace KE.GlobalEventFramework
{
    internal class MainPlugin : Plugin<Config>
	{
        public override string Author => "Patrique";
        public override string Name => "KE.GEFramework";
        public override string Prefix => "KE.GEF";
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
