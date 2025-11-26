using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Exiled.API.Interfaces;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Server;
using HarmonyLib;
using KE.CustomRoles.Abilities.FireAbilities;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.Settings;
using KE.Utils.API.CustomStats;
using KE.Utils.API.Displays.DisplayMeow;
using MEC;
using Microsoft.Win32;
using System;
using System.Linq;


namespace KE.CustomRoles
{
    public class MainPlugin : Plugin<Config,Translations>
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
        public static Translations Translations => Instance?.Translation;
        private SettingHandler _settingHandler;
        internal static SettingHandler SettingHandler => Instance?._settingHandler;

        private Harmony Harmony; 

        public override void OnEnabled()
        {
            
            Instance = this;
            _settingHandler = new();
            Utils.API.Settings.SettingHandler.Instance.SubscribeEvents();

            CustomPlayerStat.AddModule<FireStat>();
            CustomStatsEvents.SubscribeEvents();

            Harmony = new(Name);
            Harmony.PatchAll();
            SettingHandler.SubscribeEvents();
            KEAbilities.Register(Assembly);
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
            CustomStatsEvents.UnsubscribeEvents();
            Utils.API.Settings.SettingHandler.Instance.UnsubscribeEvents();
            _settingHandler = null;
            Instance = null;
        }

        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += CustomRoleImplement;
            Exiled.Events.Handlers.Server.RespawnedTeam += CustomRoleRespawning;
        }
        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= CustomRoleImplement;
            Exiled.Events.Handlers.Server.RespawnedTeam -= CustomRoleRespawning;

        }

        public void CustomRoleImplement()
        {
            KECustomRole.GiveRandomRole(Player.List);
            
        }

        public void CustomRoleRespawning(RespawnedTeamEventArgs ev)
        {
            KECustomRole.GiveRandomRole(ev.Players);
        }

        public static void ShowEffectHint(Player player, string text)
        {
            float delay = SettingHandler.GetTime(player); ;
            DisplayHandler.Instance.AddHint(MainPlugin.CREffect, player, text, delay);
        }
    }
}
