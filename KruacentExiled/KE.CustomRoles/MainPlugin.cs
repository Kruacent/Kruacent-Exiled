using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Exiled.API.Interfaces;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Server;
using HarmonyLib;
using KE.CustomRoles.Settings;
using KE.Utils.API.Displays.DisplayMeow;
using MEC;
using System;


namespace KE.CustomRoles
{
    public class MainPlugin : Plugin<Config,Translations>
    {
        public override string Name { get; } = "KE.CustomRoles";
        public override string Author => "Patrique & OmerGS";
        public override Version Version => new(1, 1, 0);
        public static MainPlugin Instance;
        private Controller _controller;
        public static readonly HintPlacement CRHint = new(0, 750);
        public static readonly HintPlacement CREffect = new(700, 300);
        public static readonly HintPlacement Abilities = new(0, 900,HintServiceMeow.Core.Enum.HintAlignment.Left);
        public static Translations Translations => Instance?.Translation;
        private SettingHandler _settingHandler;
        internal static SettingHandler SettingHandler => Instance?._settingHandler;

        private Harmony Harmony; 

        public override void OnEnabled()
        {
            
            Instance = this;
            _controller = new Controller();
            _settingHandler = new();

            Harmony = new(Name);
            Harmony.PatchAll();
            SettingHandler.SubscribeEvents();
            CustomRole.RegisterRoles(false,null,true,Assembly);
            this.SubscribeEvents();

        }

        public override void OnDisabled()
        {
            
            CustomRole.UnregisterRoles();
            SettingHandler.UnsubscribeEvents();
            Harmony.UnpatchAll();

            this.UnsubscribeEvents();
            _settingHandler = null;
            _controller = null;
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
            _controller.GiveRole(Player.List);
            
        }

        public void CustomRoleRespawning(RespawnedTeamEventArgs ev)
        {
            _controller.GiveRole(ev.Players);
        }
    }
}
