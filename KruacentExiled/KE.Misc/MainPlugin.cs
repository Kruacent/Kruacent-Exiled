using Exiled.API.Enums;
using Exiled.API.Features;
using System.Collections.Generic;
using ServerHandle = Exiled.Events.Handlers.Server;
using MEC;
using Exiled.API.Features.Doors;
using System.Linq;
using PlayerRoles;
using Exiled.Events.EventArgs.Player;
using System;
using KE.Misc.Features;
using KE.Misc.Handlers;
using Exiled.CustomRoles.API.Features;
using KE.Misc.Features.CR;
using LightContainmentZoneDecontamination;
using KE.Misc.Features.GamblingCoin;

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
        internal AutoNukeAnnoucement AutoNukeAnnoucement { get; private set; }
        internal AutoTesla AutoTesla { get; private set; }
        internal EventHandlers _gamblingCoinHandler {  get; private set; }
        internal SpawnLcz SpawnLcz { get; private set; }

        public override void OnEnabled()
        {
            Instance = this;
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
            Candy = new Candy();
            //SpawnLcz = new();
            Respawn.SetTokens(SpawnableFaction.NtfWave, 2);
            Respawn.SetTokens(SpawnableFaction.ChaosWave, 2);

            ClassDDoor.SubscribeEvents();


            MiscFeature.SubscribeAllEvents();

            if (Config.GamblingCoin)
            {
                GamblingCoinManager.RegisterAll();
                _gamblingCoinHandler = new EventHandlers();
                Exiled.Events.Handlers.Player.FlippingCoin += _gamblingCoinHandler.OnCoinFlip;
            }

            SCPBuff.SubscribeEvents();
            Exiled.Events.Handlers.Server.RoundStarted += AutoNukeAnnoucement.OnRoundStarted;
            ServerHandle.RoundStarted += ServerHandler.OnRoundStarted;
            Exiled.Events.Handlers.Player.Dying += ScpNoeDeathMessage;
            CustomRole.RegisterRoles(false, null, true, this.Assembly);
            
        }


        public override void OnDisabled()
        {
            ServerHandle.RoundStarted -= ServerHandler.OnRoundStarted;
            Exiled.Events.Handlers.Player.Dying -= ScpNoeDeathMessage;
            Exiled.Events.Handlers.Server.RoundStarted -= AutoNukeAnnoucement.OnRoundStarted;
            SCPBuff.UnsubscribeEvents();
            if (Config.GamblingCoin)
            {
                Exiled.Events.Handlers.Player.FlippingCoin -= _gamblingCoinHandler.OnCoinFlip;
            }
            AutoTesla.StopLoop();
            MiscFeature.UnsubscribeAllEvents();
            ClassDDoor.UnsubscribeEvents();


            CustomRole.UnregisterRoles([typeof(Scp035)]);

            _914 = null;
            Candy = null;
            //SpawnLcz = null;
            ClassDDoor = null;
            ServerHandler = null;
            SCPBuff = null;
            AutoTesla = null;
            Spawn = null;
            AutoElevator = null;
            AutoNukeAnnoucement = null;
            FriendlyFire = null;
            //SurfaceLight = null;
            GamblingCoinManager.DestroyAll();
            _gamblingCoinHandler = null;
            Instance = null;
        }       
        
        /// <summary>
        /// Special death message when Delecons dies as a SCP
        /// </summary>
        /// <param name="ev"></param>
        internal void ScpNoeDeathMessage(DyingEventArgs ev)
        {
            
            Player player = ev.Player;

            if (!player.UserId.Equals("76561199066936074@steam"))
                return;
            if (!player.IsScp)
                return;
            if (player.Role.Type == RoleTypeId.Scp0492)
                return;
            Cassie.MessageTranslated("SCP 69 420 has been contained successfully", "SCP-69420-NOE has been contained successfully");
        }

        


        
    }
}
