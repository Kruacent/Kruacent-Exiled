using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Exiled.API.Interfaces;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using HarmonyLib;
using KE.CustomRoles.Abilities.FireAbilities;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.HintPositions;
using KE.CustomRoles.Settings;
using KE.Misc.Features.Spawn;
using KE.Utils.API.CustomStats;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Features.SCPs;
using KE.Utils.API.GifAnimator;
using MEC;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;


namespace KE.CustomRoles
{
    public class MainPlugin : Plugin<Config>
    {
        public override string Name => "KE.CustomRoles";
        public override string Prefix => "KE.CR";
        public override string Author => "Patrique & OmerGS";
        public override Version Version => new(1, 1, 0);
        public static MainPlugin Instance;
        public static Config Configs => Instance?.Config;
        public static readonly HintPlacement CRHint = new(0, 750);
        public static readonly HintPlacement CREffect = new(700, 300);
        public static readonly HintPlacement AbilitiesDesc = new(0, 900);
        public static readonly HintPlacement Abilities = new(0, 850,HintServiceMeow.Core.Enum.HintAlignment.Left);
        public static readonly HintPlacement RightHPbars = new(55, 1000,HintServiceMeow.Core.Enum.HintAlignment.Left);
        private SettingHandler _settingHandler;
        internal static SettingHandler SettingHandler => Instance?._settingHandler;

        private Harmony Harmony;

        internal Dictionary<string, TextImage> icons;

        public static readonly string ImageLocation = Paths.Configs + "/Img/";
        public override void OnEnabled()
        {
            
            Instance = this;
            _settingHandler = new();
            Utils.API.Settings.GlobalSettings.GlobalSettingsHandler.Instance.TryLoad();
            Utils.API.Settings.GlobalSettings.GlobalSettingsHandler.Instance.SubscribeEvents();

            CustomPlayerStat.AddModule<FireStat>();
            CustomStatsEvents.SubscribeEvents();
            icons = new();

            CustomTeamEvents.SubscribeEvents();

            
            LoadImage();

            Harmony = new(Name);
            Harmony.PatchAll();
            SettingHandler.SubscribeEvents();
            KEAbilities.Register(Assembly);
            KECustomRole.Register();
            SubscribeEvents();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            
            KECustomRole.Unregister();
            SettingHandler.UnsubscribeEvents();
            Harmony.UnpatchAll();

            KEAbilities.Unregister();
            UnsubscribeEvents();
            CustomTeamEvents.UnsubscribeEvents();
            CustomStatsEvents.UnsubscribeEvents();
            Utils.API.Settings.GlobalSettings.GlobalSettingsHandler.Instance.UnsubscribeEvents();
            _settingHandler = null;
            Instance = null;
            base.OnDisabled();
        }

        public void SubscribeEvents()
        {
            Misc.Features.Spawn.Spawn.OnAssigned += KECustomRole.SpawnStartRound;
            Exiled.Events.Handlers.Server.RespawnedTeam += KECustomRole.RespawnCustomRole;
            Exiled.Events.Handlers.Server.WaitingForPlayers += KECustomRole.ResetNumberOfSpawn;
            Exiled.Events.Handlers.Player.Joined += KECustomRole.ShowCustomRole;
        }
        public void UnsubscribeEvents()
        {
            Misc.Features.Spawn.Spawn.OnAssigned -= KECustomRole.SpawnStartRound;
            Exiled.Events.Handlers.Server.RespawnedTeam -= KECustomRole.RespawnCustomRole;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= KECustomRole.ResetNumberOfSpawn;
            
            Exiled.Events.Handlers.Player.Joined -= KECustomRole.ShowCustomRole;
        }




        private void LoadImage()
        {
            if (!Directory.Exists(ImageLocation))
            {
                Log.Warn("Directory not found. creating...");
                Directory.CreateDirectory(ImageLocation);
            }

            string[] rawfile = Directory.GetFiles(ImageLocation, "*.png");

            foreach (string file in rawfile)
            {
                string noExFile = Path.GetFileNameWithoutExtension(file);
                Log.Info($"loading {file} as {noExFile}");
                icons.Add(noExFile,new TextImage(Image.FromFile(file),5));
            }
        }



        public static void ShowEffectHint(Player player, string text)
        {
            float delay = SettingHandler.GetTime(player); ;
            DisplayHandler.Instance.AddHint(MainPlugin.CREffect, player, text, delay);
        }
    }
}
