using Exiled.API.Features;
using System;
using System.Text;
using System.Threading.Tasks;
using Utf8Json;

namespace KruacentExiled.ServerTools.Features.BugReport
{
    internal static class GitHubIssueSender
    {
        public static void Send(Config config, string title, string body)
        {
            string appId = config.GitHubAppId;
            string privateKeyFile = config.GitHubPrivateKeyFile;
            string repository = config.GitHubRepository;
            string json = "{\"title\":" + JsonSerializer.ToJsonString(title) + ",\"body\":" + JsonSerializer.ToJsonString(body) + ",\"type\":\"Bug\"}";

            Task.Run(() => Send(appId, privateKeyFile, repository, json));
        }

        private static void Send(string appId, string privateKeyFile, string repository, string json)
        {
            try
            {
                string token = GitHubAppAuth.GetInstallationToken(appId, privateKeyFile, repository);
                GitHubAppAuth.Request("POST", "https://api.github.com/repos/" + repository + "/issues", token, json);

                Log.Info("issue created");
            }
            catch (Exception e)
            {
                Log.Error("issue wasn't created : " + e.Message);
            }
        }
    }
}