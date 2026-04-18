using Exiled.API.Features;
using Exiled.API.Interfaces;
using HarmonyLib;
using KE.Utils.API.CustomStats;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.GifAnimator;
using KE.Utils.API.Settings.GlobalSettings;
using KruacentExiled.CustomRoles.Abilities.FireAbilities;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using KruacentExiled.CustomRoles.Settings;
using System.Collections.Generic;
using System.Drawing;
using System.IO;


namespace KruacentExiled.CustomRoles
{
    public class MainPlugin : KEPlugin
    {
        public override string Name => "KE.CustomRoles";
        public override string Prefix => "KE.CR";
        public static MainPlugin Instance;
        public static Config Configs => (Config) Instance?.Config;
        public static readonly HintPlacement CRHint = new HintPlacement(0, 750);
        public static readonly HintPlacement CREffect = new HintPlacement(700, 300);
        public static readonly HintPlacement AbilitiesDesc = new HintPlacement(0, 900);
        public static readonly HintPlacement Abilities = new HintPlacement(0, 850,HintServiceMeow.Core.Enum.HintAlignment.Left);
        public static readonly HintPlacement RightHPbars = new HintPlacement(55, 1000,HintServiceMeow.Core.Enum.HintAlignment.Left);
        private SettingHandler _settingHandler;
        internal static SettingHandler SettingHandler => Instance?._settingHandler;

        private Harmony Harmony;

        internal Dictionary<string, TextImage> icons;

        public static readonly string ImageLocation = Paths.Configs + "/Img/";

        public override IConfig Config => config;
        private IConfig config = null;
        public override void OnEnabled()
        {
            Instance = this;
            config = KruacentExiled.MainPlugin.Instance.Config.CustomRoleConfig;

            _settingHandler = new SettingHandler();
            GlobalSettingsHandler.Instance.TryLoad();
            GlobalSettingsHandler.Instance.SubscribeEvents();

            CustomPlayerStat.AddModule<FireStat>();
            CustomStatsEvents.SubscribeEvents();
            icons = new Dictionary<string, TextImage>();

            CustomTeamEvents.SubscribeEvents();

            
            LoadImage();

            Harmony = new Harmony(Name);
            Harmony.PatchAll();
            SettingHandler.SubscribeEvents();
            KEAbilities.Register(KruacentExiled.MainPlugin.Instance.Assembly);
            KECustomRole.Register();
            SubscribeEvents();
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
            GlobalSettingsHandler.Instance.UnsubscribeEvents();
            _settingHandler = null;
            Instance = null;
        }

        public void SubscribeEvents()
        {
            Misc.Features.Spawn.Spawn.OnAssigned += KECustomRole.SpawnStartRound;
            Exiled.Events.Handlers.Server.RespawnedTeam += KECustomRole.RespawnCustomRole;
            Exiled.Events.Handlers.Server.WaitingForPlayers += KECustomRole.ResetNumberOfSpawn;
            Exiled.Events.Handlers.Player.Verified += KECustomRole.ShowCustomRole;
        }
        public void UnsubscribeEvents()
        {
            Misc.Features.Spawn.Spawn.OnAssigned -= KECustomRole.SpawnStartRound;
            Exiled.Events.Handlers.Server.RespawnedTeam -= KECustomRole.RespawnCustomRole;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= KECustomRole.ResetNumberOfSpawn;
            Exiled.Events.Handlers.Player.Verified -= KECustomRole.ShowCustomRole;
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
            DisplayHandler.Instance.AddHint(CREffect, player, text, delay);
        }
    }
}
