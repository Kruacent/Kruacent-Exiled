using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.Patches.Events.Player;
using KE.Utils.API;
using KE.Utils.Extensions;
using MapGeneration;
using MEC;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using YamlDotNet.Core.Tokens;


namespace KE.Map.Surface.Turrets
{
    public class Turret : IWorldSpace
    {
        public static bool IsEnabled => MainPlugin.Configs.TurretEnabled;

        private readonly Npc npc;
        private readonly CoroutineHandle handle;
        public float Range { get; private set; } = 10;
        public int Id { get; }
        public Player Player { get; private set; }

        public bool IsNeutral
        {
            get
            {
                return Player == null;
            }
        }
        public Vector3 Position
        {
            get
            {
                return npc.Position;
            }
            set
            {
                Move(value);
            }
        }
        public Quaternion Rotation
        {
            get
            {
                return npc.Rotation;
            }
            set
            {
                npc.Rotation = value;
            }
        }



        private static List<Turret> list = new List<Turret>();

        private Turret(Player p,Vector3 position)
        {
            Log.Debug($"player {p.Nickname} spawned turret");
            Id = list.Count;
            list.Add(this);
            Player = p;
            RoleTypeId role = p.Role;

            if (!role.IsFpcRole())
            {
                role = RoleTypeId.ClassD;
            }

            if (role.IsScp())
            {
                role = RoleTypeId.Scp0492;
            }


            npc = Npc.Spawn("Turret" + Id, role, true, position);
            Timing.CallDelayed(Npc.SpawnSetRoleDelay +Timing.WaitForOneFrame, () => npc.IsGodModeEnabled = true);
            if (npc.Role is FpcRole fpc)
            {
                fpc.Gravity = Vector3.zero;
            }

            npc.Hide();


            handle = Timing.RunCoroutine(Detect());
        }



        public static Turret Create(Player owner, Vector3 position)
        {
            if (!IsEnabled) return null;

            return new Turret(owner, position);


        }


        private IEnumerator<float> Detect()
        {

            while (Player.IsAlive)
            {
                //&& p.Role.Side != Player.Role.Side
                List<Player> inRange = Player.List.Where(p => OtherUtils.IsInCircle(p.Position, Position, Range) && !p.IsNPC ).OrderBy(p => Vector3.Distance(p.Position,Position)).ToList();
                Player closest;
                if(inRange.TryGet(0,out closest))
                {
                    Log.Debug("player closest " + closest);
                    
                    Rotation = Quaternion.LookRotation(closest.Position - Position);


                    RaycastHit hit;
                    if (UnityEngine.Physics.Linecast(Position, closest.Position, out hit))
                    {
                        Player playerhit = Player.Get(hit.collider);
                        Log.Debug("hit = " + playerhit?.Nickname);
                        if (playerhit != null)
                        {
                            
                            playerhit.Hurt(10);
                        }
                    }




                    yield return Timing.WaitForSeconds(1);
                }
                else
                {
                    Log.Debug("no player nearby");
                    yield return Timing.WaitForSeconds(5);
                }
            }
        }

        public void Move(Vector3 newPosition)
        {
            npc.Teleport(newPosition);

        }



        public void Destroy()
        {
            npc.Destroy();
        }

        private static void OnEndingRound(EndingRoundEventArgs ev)
        {
            if (ev.IsAllowed)
            {
                DestroyAll();
            }
        }

        public static void DestroyAll()
        {
            foreach (Turret turret in list)
            {
                turret.Destroy();
            }
        }

        public static void SubscribeEvents()
        {
            if (!IsEnabled) return;
            Exiled.Events.Handlers.Server.EndingRound += OnEndingRound;
        }

        public static void UnsubscribeEvents()
        {
            if (!IsEnabled) return;
            DestroyAll();
            Exiled.Events.Handlers.Server.EndingRound -= OnEndingRound;

        }

    }
}
