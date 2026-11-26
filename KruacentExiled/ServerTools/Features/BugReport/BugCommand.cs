using CommandSystem;
using Exiled.API.Features;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.ServerTools.Features.BugReport
{
    /// <summary>
    /// Command for player to report a bug
    /// </summary>
    [CommandHandler(typeof(ClientCommandHandler))]
    public class BugCommand : ICommand
    {
        public string Command => "bug";
        public string[] Aliases => new string[0];
        public string Description => "Report a bug : .bug <what is the problem>";

        private const int MinLength = 10;
        private const int MaxTitleLength = 60;

        private static readonly Dictionary<string, float> lastReports = new Dictionary<string, float>();

        private static Config Config => MainPlugin.Instance.config;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (MainPlugin.Instance == null || !Config.BugReport || string.IsNullOrEmpty(Config.GitHubAppId) || !System.IO.File.Exists(Config.GitHubPrivateKeyFile))
            {
                response = "Bug report is disabled on this server.";
                return false;
            }

            if (!Player.TryGet(sender, out Player player))
            {
                response = "Need to be a player";
                return false;
            }

            string description = string.Join(" ", arguments);
            if (description.Length < MinLength)
            {
                response = "Describe the bug in one sentence : .bug <what is the problem>";
                return false;
            }

            if (lastReports.TryGetValue(player.UserId, out float lastTime) && Time.time - lastTime < Config.BugReportCooldown)
            {
                int wait = Mathf.CeilToInt(Config.BugReportCooldown - (Time.time - lastTime));
                response = $"You need to wait {wait}s before a new report.";
                return false;
            }

            lastReports[player.UserId] = Time.time;

            string title = titleFormatter(description);
            string body = "## Description\n> " + description + "\n\n" + ReporterInformationBuilder.Build(player);
            GitHubIssueSender.Send(Config, title, body);
            
            response = "Thanks you, report sended.";
            return true;
        }

        private static string titleFormatter(string title)
        {
            if (title.Length > MaxTitleLength)
            {
                title = title.Substring(0, MaxTitleLength) + "...";
            }

            return "[IG Report] " + title;
        }
    }
}