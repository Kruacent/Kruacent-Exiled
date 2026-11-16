using Exiled.API.Interfaces;
using System.ComponentModel;

namespace KruacentExiled.ServerTools
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        [Description("Enable the .bug command that creates a GitHub issue")]
        public bool BugReport { get; set; } = true;

        [Description("GitHub App ID shown on the App settings page")]
        public string GitHubAppId { get; set; } = "5688122";

        [Description("Path to the GitHub App private key")]
        public string GitHubPrivateKeyFile { get; set; } = System.IO.Path.Combine(Exiled.API.Features.Paths.Configs, "kruacent-bugreport.pem");

        [Description("Repository receiving the issues (owner/name)")]
        public string GitHubRepository { get; set; } = "Kruacent/Kruacent-Exiled";

        [Description("Cooldown between two reports")]
        public float BugReportCooldown { get; set; } = 120f;
    }
}