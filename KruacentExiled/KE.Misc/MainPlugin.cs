using Exiled.API.Enums;
using Exiled.API.Features;
using System.Collections.Generic;
using ServerHandle = Exiled.Events.Handlers.Server;
using Nine14Handle = Exiled.Events.Handlers.Scp914;
using MEC;
using Exiled.API.Features.Doors;
using System.Linq;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Scp914;
using Scp914;
using PlayerRoles;
using Exiled.Events.EventArgs.Player;
using KruacentExiled.KruacentE.Misc;
using System;

namespace KE.Misc
{

    public class MainPlugin : Plugin<Config>
    {
        public override string Author => "Patrique";
        public override string Name => "KEMisc";
        public override Version Version => new Version(1, 0, 0);
        internal static MainPlugin Instance { get; private set; }
        private ServerHandler ServerHandler;
        internal _914 _914 { get; private set; }
        internal AutoElevator AutoElevator { get; private set; }
        internal ClassDDoor ClassDDoor { get; private set; }

        public override void OnEnabled()
        {
            Instance = this;
            _914 = new _914();
            AutoElevator = new AutoElevator();
            ClassDDoor = new ClassDDoor();
            ServerHandler = new ServerHandler();

            ServerHandle.RoundStarted += ServerHandler.OnRoundStarted;
            Nine14Handle.UpgradingPlayer += _914.OnUpgradingPlayer;
            Exiled.Events.Handlers.Player.Dying += ScpNoeDeathMessage;

        }

        public override void OnDisabled()
        {
            ServerHandle.RoundStarted -= ServerHandler.OnRoundStarted;
            Nine14Handle.UpgradingPlayer -= _914.OnUpgradingPlayer;
            Exiled.Events.Handlers.Player.Dying -= ScpNoeDeathMessage;

            _914 = null;
            ClassDDoor = null;
            ServerHandler = null;
            AutoElevator = null;
            Instance = null;
        }


        /// <summary>
        /// Set the Friendly Fire to true or false at random
        /// </summary>
        internal void RandomFF()
        {
            Server.FriendlyFire = UnityEngine.Random.Range(0, 101) < Instance.Config.ChanceFF;
            Log.Info($"Friendly Fire : {Server.FriendlyFire}");
        }

        /// <summary>
        /// C.A.S.S.I.E. announce 5 min before the autonuke
        /// </summary>
        internal IEnumerator<float> NukeAnnouncement()
        {
            Log.Debug("autonuke announcement : on");
            yield return Timing.WaitUntilTrue(() => 25 <= Round.ElapsedTime.TotalMinutes);
            Cassie.MessageTranslated("Warning automatic warhead will detonate in 5 minutes", 
                "Warning automatic warhead will detonate in <color=#FF0000>5</color> minutes");
        }

        /// <summary>
        /// Lock SCP-173 in its cell for an amount of time determine by the number of player
        /// Formula : timeLock = 135-nbPlayer*15
        /// </summary>
        internal IEnumerator<float> PeanutLockdown()
        {
            if(!Player.List.Any(p => p.Role.Type == RoleTypeId.Scp173))
            {
                yield return 0;
            }
            Log.Debug("peanut lockdown");
            Door peanutDoor = Door.List.ToList().Where(x => x.Type == DoorType.Scp173NewGate).ToList()[0];
            peanutDoor.IsOpen = false;
            peanutDoor.ChangeLock(DoorLockType.Lockdown2176);
            yield return Timing.WaitForSeconds(135-Player.List.Count*15);
            peanutDoor.IsOpen = true;
            peanutDoor.Unlock();
            Log.Debug("peanut free");
        }
        
        /// <summary>
        /// Special death message when Delecons dies as a SCP
        /// </summary>
        /// <param name="ev"></param>
        internal void ScpNoeDeathMessage(DyingEventArgs ev)
        {
            
            Player player = ev.Player;
            Log.Debug($"someone died = {player.UserId}");

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
