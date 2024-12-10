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

namespace KE.Misc
{
    public class MainPlugin : Plugin<Config>
    {
        internal static MainPlugin Instance { get; private set; }
        private ServerHandler serverHandler;

        public override void OnEnabled()
        {
            Instance = this;

            serverHandler = new ServerHandler();
            ServerHandle.RoundStarted += serverHandler.OnRoundStarted;
            Nine14Handle.UpgradingPlayer += OnUpgradingPlayer;

        }

        public override void OnDisabled()
        {
            
            ServerHandle.RoundStarted -= serverHandler.OnRoundStarted;


            serverHandler = null;

            Instance = null;
        }

        internal void ClassDDoorGoesBoom()
        {
            if (UnityEngine.Random.Range(0, 101) < Instance.Config.ChanceClassDDoorGoesBoom)
            {
                foreach (Door door in Door.List)
                {
                    if (door.Type == DoorType.PrisonDoor)
                    {
                        if (door is IDamageableDoor dBoyDoor && !dBoyDoor.IsDestroyed)
                        {
                            dBoyDoor.Break();
                            Log.Debug("Les portes kaboom");
                        }
                    }
                }
            }
            else
            {
                Log.Debug("Les portes ne kaboom pas");
            }
        }


        internal void RandomFF()
        {
            if(UnityEngine.Random.Range(0,101) < Instance.Config.ChanceFF)
            {
                Server.FriendlyFire = true;
            }
            else
            {
                Server.FriendlyFire = false;
            }
            Log.Debug($"Friendly Fire : {Server.FriendlyFire}");
        }


        internal IEnumerator<float> NukeAnnouncement()
        {
            yield return Timing.WaitUntilTrue(() => 25 <= Round.ElapsedTime.TotalMinutes);
            Cassie.MessageTranslated("Warning automatic warhead will detonate in 5 minutes", 
                "Warning automatic warhead will detonate in <color=#FF0000>5</color> minutes");
        }


        internal IEnumerator<float> PeanutLockdown()
        {
            Door peanutDoor = Door.List.ToList().Where(x => x.Type == DoorType.Scp173NewGate).ToList()[0];
            peanutDoor.IsOpen = false;
            peanutDoor.ChangeLock(DoorLockType.Lockdown2176);
            yield return Timing.WaitForSeconds(135-Player.List.Count*15);
            peanutDoor.IsOpen = true;
            peanutDoor.Unlock();
        }
        
        internal IEnumerator<float> AutoElevator()
        {
            while (!Round.IsEnded)
            {
                foreach (Lift l in Lift.List)
                {
                    yield return Timing.WaitForSeconds(UnityEngine.Random.Range(30,45));
                    SendElevator(l);
                }
            }
        }

        private void SendElevator(Lift e)
        {
            e.TryStart(0, true);
        }


        private void OnUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            Teleport(ev.Player,ev.KnobSetting);

            ChangingRole(ev.Player);
        }


        private void Teleport(Player p,Scp914KnobSetting knob)
        {
            if(knob == Scp914KnobSetting.Fine && UnityEngine.Random.value < 0.01f)
                p.Teleport(Room.Random(ZoneType.Entrance));
            if(knob == Scp914KnobSetting.Coarse && UnityEngine.Random.value < .25f)
                p.Teleport(Room.Random(ZoneType.LightContainment));
        }

        private void ChangingRole(Player p)
        {

        }

    }
}
