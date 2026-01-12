using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Exiled.API.Interfaces;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Server;
using HarmonyLib;
using KE.CustomRoles.Abilities.FireAbilities;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.Settings;
using KE.Misc.Features.Spawn;
using KE.Utils.API.CustomStats;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Translations;
using MEC;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;


namespace KE.CustomRoles
{
    public class MainPlugin : Plugin<Config>, Utils.API.Translations.ITranslation
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

        internal TranslationFile translation;
        public TranslationFile Translation => translation;
        private Harmony Harmony; 

        public override void OnEnabled()
        {
            
            Instance = this;
            translation = new CRTranslationFile();
            _settingHandler = new();
            //Utils.API.Settings.SettingHandler.Instance.SubscribeEvents();

            CustomPlayerStat.AddModule<FireStat>();
            CustomStatsEvents.SubscribeEvents();

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
            CustomStatsEvents.UnsubscribeEvents();
            Utils.API.Settings.SettingHandler.Instance.UnsubscribeEvents();
            _settingHandler = null;
            Instance = null;
            base.OnDisabled();
        }

        public void SubscribeEvents()
        {
            Misc.Features.Spawn.Spawn.OnAssigned += CustomRoleImplement;
            Exiled.Events.Handlers.Server.RespawnedTeam += CustomRoleRespawning;
        }
        public void UnsubscribeEvents()
        {
            Misc.Features.Spawn.Spawn.OnAssigned -= CustomRoleImplement;
            Exiled.Events.Handlers.Server.RespawnedTeam -= CustomRoleRespawning;

        }

        public void CustomRoleImplement(SpawnedEventArgs ev)
        {
            ShowTranslation();
            KECustomRole.GiveRandomRole(Player.List.Except(ev.CustomRoles));
            
        }


        public void ShowTranslation()
        {
            ///????????????
            translation.Values.AddRange(KECustomRole.keys);
            Log.Debug("nb trnalsaikey"+ KECustomRole.keys.Count);
            Log.Debug("nb trnalsai"+ translation.Values.Count);

            Log.Debug(translation.ToString());
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
