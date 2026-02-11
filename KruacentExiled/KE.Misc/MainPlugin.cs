using Exiled.API.Enums;
using Exiled.API.Features;
using ServerHandle = Exiled.Events.Handlers.Server;
using PlayerRoles;
using System;
using KE.Misc.Features;
using KE.Misc.Handlers;
using Exiled.CustomRoles.API.Features;
using KE.Misc.Features.GamblingCoin;
using HarmonyLib;
using LabApi.Events.Arguments.ServerEvents;
using KE.Misc.Features.VoteStart;
using KE.Misc.Features.Spawn;
using KE.Misc.Features.LastHuman;
using KE.Utils.API.Translations;

namespace KE.Misc
{

    public class MainPlugin : Plugin<Config>
    {
        public override string Author => "Patrique";
        public override string Name => "KE.Misc";
        public override string Prefix => "KE.Misc";
        public override Version Version => new Version(1, 1, 0);
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

        public override void OnEnabled()
        {
            Instance = this;
            harmony = new(Prefix);

            _914 = new _914();
            AutoElevator = new AutoElevator();
            ClassDDoor = new ClassDDoor();
            //SurfaceLight = new SurfaceLight(); messes with the nuke light
            ServerHandler = new ServerHandler();
            Spawn = new Spawn();
            SCPBuff = new SCPBuff();
            FriendlyFire = new();
            AutoNukeAnnoucement = new();
            AutoTesla = new();
            LastHuman = new();
            Candy = new Candy();
            vote = new();
            //SpawnLcz = new();
            Respawn.SetTokens(SpawnableFaction.NtfWave, 2);
            Respawn.SetTokens(SpawnableFaction.ChaosWave, 2);

            TranslationManager.Instance.TryLoad();


            harmony.PatchAll(Assembly);
            ClassDDoor.SubscribeEvents();
            MiscFeature.SubscribeAllEvents();
            AutoNukeAnnoucement.SubscribeEvents();
            if (Config.GamblingCoin)
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
            LabApi.Events.Handlers.ServerEvents.CassieQueuingScpTermination -= NoeDeath;
            AutoNukeAnnoucement.UnsubscribeEvents();
            LastHuman.UnsubscribeEvents();
            SCPBuff.UnsubscribeEvents();
            if (Config.GamblingCoin)
            {
                Exiled.Events.Handlers.Player.FlippingCoin -= _gamblingCoinHandler.OnCoinFlip;
            }
            AutoTesla.StopLoop();
            MiscFeature.UnsubscribeAllEvents();
            ClassDDoor.UnsubscribeEvents();
            harmony.UnpatchAll(harmony.Id);


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
            LastHuman = null;
            harmony = null;
            Instance = null;
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

        


        
    }
}
