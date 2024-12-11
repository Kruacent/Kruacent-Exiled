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

namespace KE.Misc
{
    public class MainPlugin : Plugin<Config>
    {
        internal static MainPlugin Instance { get; private set; }
        private ServerHandler serverHandler;
        internal Dictionary<int, RoleTypeId> roleScp = new Dictionary<int, RoleTypeId>()
        {
            { -1, RoleTypeId.Scp049 },
            { -2, RoleTypeId.Scp939 },
            { -3, RoleTypeId.Scp096 },

            { 1, RoleTypeId.Scp106 },
            { 2, RoleTypeId.Scp173 },
            { 3, RoleTypeId.Scp3114},

        };

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

            ChangingRole(ev.Player, ev.KnobSetting);
        }


        private void Teleport(Player p,Scp914KnobSetting knob)
        {
            if(knob == Scp914KnobSetting.Fine && UnityEngine.Random.value < .01f)
                p.Teleport(Room.Random(ZoneType.Entrance));
            if(knob == Scp914KnobSetting.Coarse && UnityEngine.Random.value < .25f)
                p.Teleport(Room.Random(ZoneType.LightContainment));
        }

        private void ChangingRole(Player p, Scp914KnobSetting knob)
        {
            if (p.IsScp)
            {
                HandleScpChangingRole(p,knob);
            }
            else if (p.IsHuman)
            {
                HandleHumanChangingRole(p, knob);
            }
        }

        private void HandleHumanChangingRole(Player p, Scp914KnobSetting knob)
        {
            if (p.IsHuman)
            {
                switch (p.Role.Type)
                {
                    case RoleTypeId.Scientist:
                        if (UnityEngine.Random.value < .5f)
                            p.Role.Set(RoleTypeId.ClassD);
                        break;
                    case RoleTypeId.ClassD:
                        if (UnityEngine.Random.value < .5f)
                            p.Role.Set(RoleTypeId.Scientist);
                        break;
                    case RoleTypeId.FacilityGuard:
                        if (knob == Scp914KnobSetting.Rough)
                            p.Role.Set(RoleTypeId.FacilityGuard);
                        break;
                }
            }
        }

        private void HandleScpChangingRole(Player p, Scp914KnobSetting knob)
        {

            if (p.IsScp)
            {
                var invertRoleScp = roleScp.ToDictionary(k => k.Value, v => v.Key);
                if (UnityEngine.Random.value < .5f)
                {
                    // get the id of the scp
                    if (invertRoleScp.TryGetValue(p.Role.Type, out int key))
                    {
                        RoleTypeId newRole;
                        switch (knob)
                        {
                            //going up in the graph
                            case Scp914KnobSetting.Rough:
                                if (roleScp.TryGetValue(key - 1, out newRole))
                                {
                                    p.Role.Set(newRole);
                                }
                                break;
                            //going horizontaly in the graph
                            case Scp914KnobSetting.OneToOne:
                                if (roleScp.TryGetValue(key * (-1), out newRole))
                                {
                                    p.Role.Set(newRole);
                                }
                                break;
                            //going down in the graph
                            case Scp914KnobSetting.VeryFine:
                                if (roleScp.TryGetValue(key + 1, out newRole))
                                {
                                    p.Role.Set(newRole);
                                }
                                break;
                        }
                    }

                }
            }
        }

    }
}
