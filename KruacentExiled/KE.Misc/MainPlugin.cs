using Exiled.API.Enums;
using Exiled.API.Features;
using ServerHandle = Exiled.Events.Handlers.Server;
using PlayerRoles;
using System;
using KE.Misc.Features;
using KE.Misc.Handlers;
using KE.Misc.Features.GamblingCoin;
using HarmonyLib;
using LabApi.Events.Arguments.ServerEvents;
using KE.Misc.Features.VoteStart;
using KE.Misc.Features.Spawn;
using KE.Misc.Features.LastHuman;
using KE.Utils.API.Settings.GlobalSettings;
using KE.Utils.API.Translations;
using KE.Misc.Features.PostNuke;
using Exiled.Events.EventArgs.Server;
using KE.Misc.Features.LobbyHints;
using KruacentExiled;
using Exiled.API.Interfaces;

namespace KE.Misc
{

    public class MainPlugin : KEPlugin, ILocalizable
    {
        public override string Name => "KE.Misc";
        public override string Prefix => "KE.Misc";


        internal static MainPlugin Instance { get; private set; }
        private ServerHandler ServerHandler;
        internal _914 _914 { get; private set; }
        internal AutoElevator AutoElevator { get; private set; }
        internal ClassDDoor ClassDDoor { get; private set; }
        internal Candy Candy { get; private set; }
        internal SurfaceLight SurfaceLight { get; private set; }
        internal SCPBuff SCPBuff { get; private set; }
        internal Spawn Spawn { get; private set; }
        internal FriendlyFire FriendlyFire { get; private set; }
        internal NukeKill AutoNukeAnnoucement { get; private set; }
        internal AutoTesla AutoTesla { get; private set; }
        internal EventHandlers _gamblingCoinHandler {  get; private set; }
        internal SpawnLcz SpawnLcz { get; private set; }
        internal LastHumanHandler LastHuman { get; private set; }
        private Harmony harmony;

        internal VoteStart vote { get; private set; }
        internal PostNukeHandler postnuke { get; private set; }
        internal LobbyHint LobbyHint { get; private set; }

        public string LocalizationId => Prefix;

        public override IConfig Config => config;
        public static Config Configs => Instance?.config;
        private Config config;

        public override void OnEnabled()
        {
            Instance = this;
            harmony = new Harmony(Prefix);

            config = KruacentExiled.MainPlugin.Instance.Config.MiscConfig;

            _914 = new _914();
            AutoElevator = new AutoElevator();
            ClassDDoor = new ClassDDoor();
            //SurfaceLight = new SurfaceLight(); messes with the nuke light
            ServerHandler = new ServerHandler();
            Spawn = new Spawn();
            SCPBuff = new SCPBuff();
            FriendlyFire = new FriendlyFire();
            AutoNukeAnnoucement = new NukeKill();
            AutoTesla = new AutoTesla();
            LastHuman = new LastHumanHandler();
            Candy = new Candy();
            vote = new VoteStart();
            postnuke = new PostNukeHandler();
            LobbyHint = new LobbyHint();


            //SpawnLcz = new();
            
            GlobalSettingsHandler.Instance.TryLoad();
            GlobalSettingsHandler.Instance.SubscribeEvents();
            RegisterTranslations();

            harmony.PatchAll(KruacentExiled.MainPlugin.Instance.Assembly);
            ClassDDoor.SubscribeEvents();
            ServerHandle.RoundEnded += OnRoundEnded;
            MiscFeature.SubscribeAllEvents();
            AutoNukeAnnoucement.SubscribeEvents();
            if (Configs.GamblingCoin)
            {
                GamblingCoinManager.RegisterAll();
                _gamblingCoinHandler = new EventHandlers();
                Exiled.Events.Handlers.Player.FlippingCoin += _gamblingCoinHandler.OnCoinFlip;
            }
            LastHuman.SubscribeEvents();
            SCPBuff.SubscribeEvents();
            
            ServerHandle.RoundStarted += ServerHandler.OnRoundStarted;
            LabApi.Events.Handlers.ServerEvents.CassieQueuingScpTermination += NoeDeath;
            
        }


        public override void OnDisabled()
        {
            ServerHandle.RoundStarted -= ServerHandler.OnRoundStarted;
            ServerHandle.RoundEnded -= OnRoundEnded;
            LabApi.Events.Handlers.ServerEvents.CassieQueuingScpTermination -= NoeDeath;
            AutoNukeAnnoucement.UnsubscribeEvents();
            LastHuman.UnsubscribeEvents();
            SCPBuff.UnsubscribeEvents();
            if (Configs.GamblingCoin)
            {
                Exiled.Events.Handlers.Player.FlippingCoin -= _gamblingCoinHandler.OnCoinFlip;
            }
            AutoTesla.StopLoop();
            MiscFeature.UnsubscribeAllEvents();
            ClassDDoor.UnsubscribeEvents();
            harmony.UnpatchAll(harmony.Id);
            GlobalSettingsHandler.Instance.UnsubscribeEvents();

            _914 = null;
            Candy = null;
            //SpawnLcz = null;
            ClassDDoor = null;
            ServerHandler = null;
            SCPBuff = null;
            AutoTesla = null;
            Spawn = null;
            AutoElevator = null;
            vote = null;
            AutoNukeAnnoucement = null;
            FriendlyFire = null;
            //SurfaceLight = null;
            GamblingCoinManager.DestroyAll();
            _gamblingCoinHandler = null;
            postnuke = null;
            LobbyHint = null;
            LastHuman = null;
            harmony = null;
            Instance = null;
        }       
        
        private void OnRoundEnded(Exiled.Events.EventArgs.Server.RoundEndedEventArgs ev)
        {
            Server.FriendlyFire = true;
        }

        private void NoeDeath(CassieQueuingScpTerminationEventArgs ev)
        {
            Player player = Player.Get(ev.Player);
            if (!player.UserId.Equals("76561199066936074@steam"))
            {
                return;
            }


            if (!player.IsScp)
            {
                return;
            }

            if(player.Role == RoleTypeId.Scp0492)
            {
                return;
            }

            ev.IsAllowed = false;
            Exiled.API.Features.Cassie.MessageTranslated("SCP 69 420 has been contained successfully", "SCP-69420-NOE has been contained suscessfully");
        }

        public void RegisterTranslations()
        {
            TranslationHub.Add(LocalizationId, LastHumanTranslations.LangToKeyToTranslation);
            TranslationHub.Add(LocalizationId, WrongAspectRatioWarningLobbyHint.LangToKeyToTranslation);
            //TranslationHub.Add(LocalizationId, VoteStart.LangToKeyToTranslation);
        }


        public static string GetTranslation(Player player,string key)
        {
            return TranslationHub.Get(player, Instance.LocalizationId, key);
        }
    }
}
