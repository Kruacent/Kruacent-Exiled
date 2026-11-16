using Exiled.API.Interfaces;
using KruacentExiled.ServerTools.Features;
using KruacentExiled.ServerTools.Features.BugReport;

namespace KruacentExiled.ServerTools
{
    public class MainPlugin : KEPlugin
    {
        public override string Name => "KE.ServerTools";
        public override string Prefix => "KE.ST";

        internal static MainPlugin Instance { get; private set; }
        public override IConfig Config => config;
        internal Config config;

        public override void OnEnabled()
        {
            Instance = this;
            config = KruacentExiled.MainPlugin.Instance.Config.ServerToolsConfig;
        }

        public override void OnDisabled()
        {
            Instance = null;
        }
    }
}