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

namespace KE.Misc
{

    public class MainPlugin : Plugin<Config>
    {
        public override string Author => "Patrique";
        public override string Name => "KEMisc";
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
        

        public override void OnEnabled()
        {
            Instance = this;
            _914 = new _914();
            AutoElevator = new AutoElevator();
            ClassDDoor = new ClassDDoor();
            SurfaceLight = new SurfaceLight();
            ServerHandler = new ServerHandler();
            Spawn = new Spawn();
            SCPBuff = new SCPBuff();
            FriendlyFire = new();
            AutoNukeAnnoucement = new();
            Candy = new Candy();
            Respawn.SetTokens(SpawnableFaction.NtfWave, 2);
            Respawn.SetTokens(SpawnableFaction.ChaosWave, 2);


            
            MiscFeature.SubscribeAllEvents();
            Exiled.Events.Handlers.Server.RoundStarted += AutoNukeAnnoucement.OnRoundStarted;
            Exiled.Events.Handlers.Player.ChangingRole += SCPBuff.BecomingSCP;
            ServerHandle.RoundStarted += ServerHandler.OnRoundStarted;
            Exiled.Events.Handlers.Player.Dying += ScpNoeDeathMessage;
            CustomRole.RegisterRoles(false, null, true, this.Assembly);
            
        }


        public override void OnDisabled()
        {
            ServerHandle.RoundStarted -= ServerHandler.OnRoundStarted;
            Exiled.Events.Handlers.Player.Dying -= ScpNoeDeathMessage;
            Exiled.Events.Handlers.Server.RoundStarted -= AutoNukeAnnoucement.OnRoundStarted;
            MiscFeature.UnsubscribeAllEvents();


            CustomRole.UnregisterRoles([typeof(Scp035)]);


            _914 = null;
            Candy = null;
            ClassDDoor = null;
            ServerHandler = null;
            SCPBuff = null;
            Spawn = null;
            AutoElevator = null;
            AutoNukeAnnoucement = null;
            FriendlyFire = null;
            SurfaceLight = null;
            Instance = null;
        }



        /// <summary>
        /// Lock SCP-173 in its cell for an amount of time determine by the number of player
        /// Formula : timeLock = 135-nbPlayer*15
        /// </summary>
        internal IEnumerator<float> PeanutLockdown()
        {
            
            Log.Debug("peanut lockdown");
            Door peanutDoor = Door.List.First(x => x.Type == DoorType.Scp173NewGate); // broken 049 gate is considered as 173 new gate for some reason
            peanutDoor.IsOpen = false;
            peanutDoor.ChangeLock(DoorLockType.Isolation);
            CoroutineHandle a;
            if (Instance.Config.Debug)
                a = Timing.RunCoroutine(Timer(120, "u r free :3"));
            else
                a = Timing.RunCoroutine(Timer(135 - Player.List.Count * 15, "u r free :3"));
             
            yield return Timing.WaitUntilDone(a);
            peanutDoor.IsOpen = true;
            peanutDoor.Unlock();
            Log.Debug("peanut free");
        }
        private IEnumerator<float> Timer(int secondsWaiting, string msg = "done")
        {
            List<Player> playerToShow = [.. Player.List];
            while (secondsWaiting >= 0)
            {
                playerToShow.RemoveAll(p => p.CurrentRoom.Type != RoomType.Hcz049);
                playerToShow.AddRange(Player.List.Where(p => p.CurrentRoom.Type == RoomType.Hcz049));

                //RueIHint hint = new(HPosition.Center, VPosition.CustomRole, $"{secondsWaiting} seconds left for SCP-173's spawn");
                playerToShow.ForEach(p => 
                {
                    //DisplayPlayer.Get(p).Hint(new Position(HPosition.Center, 600), hub => $"{secondsWaiting} seconds left for SCP-173's spawn");
                    //DisplayCore c = DisplayCore.Get(p.ReferenceHub);
                    //c.SetElemTemp($"<align={HPosition.Center.ToString().ToLower()}>"+hint.RawContent+"</align>", (int)hint.Position.VPosition, TimeSpan.FromSeconds(hint.Duration), new RueI.Displays.Scheduling.TimedElemRef<RueI.Elements.SetElement>());
                    //DisplayPlayer.Get(p).Hint(new Position(HPosition.Center,600), $"{secondsWaiting} seconds left for SCP-173's spawn",1);
                });
                yield return Timing.WaitForSeconds(1);
                secondsWaiting--;
            }
            //playerToShow.ForEach(p => DisplayPlayer.Get(p).Hint(new Position(HPosition.Center,VPosition.CustomRole),msg));
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
