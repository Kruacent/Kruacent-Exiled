using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.API.Interfaces;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KE.CustomRoles.CR.Human
{
   /* public class Hitman : GlobalCustomRole, IColor
    {
        private static CoroutineHandle _coroutines;
        public override SideEnum Side { get; set; } = SideEnum.Human;
        public override string Description { get; set; } = "pov sou hiyori de Your Turn To Die -Death Game By Majority-";
        public override string PublicName { get; set; } = string.Empty;
        public override bool KeepRoleOnDeath { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = true;
        public override float SpawnChance { get; set; } = 0;

        public Color32 Color => new Color32(54, 54, 54,0);

        private Player TargetPlayer;
        private Player TheHitman;

        private void NerfHitman()
        {
            Log.Info($"Hitman Nerf Applied to {this.TheHitman.Nickname}");
            this.TheHitman.MaxHealth = 70;
            this.TheHitman.Health -= 30;

            this.TheHitman.EnableEffect(EffectType.Slowness, -1,true);
        }

        private void BuffHitman()
        {
            Log.Info($"Hitman Buff Applied to {this.TheHitman.Nickname}");
            this.TheHitman.MaxHealth = 130;
            this.TheHitman.Health += 30;

            this.TheHitman.EnableEffect(EffectType.MovementBoost, -1, true);
        }

        protected override void SetCustomInfo(Player player)
        {

        }

        public override bool IsAvailable(Player player)
        {
            return Player.List.Count(p=> p != player && !p.IsScp) > 0;
        }

        protected override void RoleAdded(Player player)
        {
            Log.Info("Role full name : " + player.Role.Name);
            this.CustomInfo = player.Role.Name;

            this.TheHitman = player;
            // Target cannot be NPC, SCP or the Hitman
            this.TargetPlayer = Player.List.Where(x => x.IsHuman && x != player && !x.IsNPC).GetRandomValue();

            if(TargetPlayer == null)
            {
                Log.Warn("no other human player");
                return;
            }



            Log.Debug($"Target showing to Hitman, target is : {TargetPlayer.Nickname}");
            ShowEffectHint(player, $"The Target is {TargetPlayer.Nickname}");

            if (this.TheHitman.Role.Side == this.TargetPlayer.Role.Side && !Server.FriendlyFire)
            {
                bool success = this.TheHitman.TryAddCustomRoleFriendlyFire(this.TargetPlayer.Role.Type.ToString(), this.TargetPlayer.Role.Type, 1);
                Log.Info("Custom FF : " + success);
            }

            _coroutines = Timing.RunCoroutine(CheckRooms());
        }

        protected override void RoleRemoved(Player player)
        {
            Timing.KillCoroutines(_coroutines);
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Escaped += TargetEscaped;
            Exiled.Events.Handlers.Player.Died += TargetDie;
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Escaped -= TargetEscaped;
            Exiled.Events.Handlers.Player.Died -= TargetDie;
        }

        public void TargetEscaped(EscapedEventArgs ev)
        {
            if (ev.Player != this.TargetPlayer) return;

            
            Log.Info($"Target {this.TargetPlayer.Nickname} escaped");
            ShowEffectHint(TheHitman, $"The target ({this.TargetPlayer.Nickname}) has escaped, you lost the contract");


            NerfHitman();

            this.TargetPlayer = null;
        }

        public void TargetDie(DiedEventArgs ev)
        {
            if (ev.Player != this.TargetPlayer) return;

            Log.Info($"{ev.Attacker} has killed {ev.Player}");

            // Hitman killed the target
            if (ev.Player == this.TargetPlayer && ev.Attacker == this.TheHitman)
            {
                ShowEffectHint(TheHitman, $"You killed the target ({this.TargetPlayer.Nickname}), you achieved the contract");
                BuffHitman();

                this.TargetPlayer = null;
            }
            else
            {
                ShowEffectHint(TheHitman, $"The target ({this.TargetPlayer.Nickname}) is dead, you lost the contract");
                NerfHitman();

                this.TargetPlayer = null;
            }

            if (this.TheHitman.Role.Side == this.TargetPlayer.Role.Side && !Server.FriendlyFire)
            {
                this.TheHitman.TryRemoveCustomeRoleFriendlyFire(this.TargetPlayer.Role.Type.ToString());
            }
        }




        const float CHECK_INTERVAL = 15f;

        private IEnumerator<float> CheckRooms()
        {
            List<(int distance, string message, bool hasLogged)> proximityAlerts = new List<(int, string, bool)>
            {
                (0, "Sweat drips down your forehead...", false),
                (3, "Your heart beats out of your chest...", false), 
                (5, "Your lungs tighten up...", false), 
                (999, "You feel dizzy...", false)
            };

            while (true)
            {
                if (this.TargetPlayer == null)
                {
                    Log.Info("Target player is null. Stopping CheckRooms coroutine.");
                    yield break;
                }

                yield return Timing.WaitForSeconds(CHECK_INTERVAL);

                for (int i = 0; i < proximityAlerts.Count; i++)
                {
                    var (distance, message, hasLogged) = proximityAlerts[i];

                    if (distance == 0 && this.TheHitman.CurrentRoom == this.TargetPlayer.CurrentRoom)
                    {
                        if (!hasLogged)
                        {
                            Log.Info(this.TargetPlayer + " " + message);

                            ShowEffectHint(TargetPlayer, message);
                            ResetLogs(ref proximityAlerts);
                            proximityAlerts[i] = (distance, message, true);
                        }
                        break;
                    }
                    else if (distance == 999 && this.TheHitman.Zone == this.TargetPlayer.Zone)
                    {
                        if (!hasLogged)
                        {
                            ShowEffectHint(TargetPlayer, message);
                            Log.Info(this.TargetPlayer + " " + message);
                            ResetLogs(ref proximityAlerts);
                            proximityAlerts[i] = (distance, message, true);
                        }
                        break;
                    }
                    else if (AreRoomsWithinDepth(this.TheHitman, this.TargetPlayer, distance))
                    {
                        if (!hasLogged)
                        {
                            ShowEffectHint(TargetPlayer, message);
                            Log.Info(this.TargetPlayer + " " + message);
                            ResetLogs(ref proximityAlerts);
                            proximityAlerts[i] = (distance, message, true);
                        }
                        break;
                    }
                }
            }
        }

        private bool AreRoomsWithinDepth(Player p1, Player p2, int depth)
        {
            HashSet<Room> nearbyRooms = GetNearbyRooms(p1, depth);
            return nearbyRooms.Contains(p2.CurrentRoom);
        }

        private HashSet<Room> GetNearbyRooms(Player p, int depth)
        {
            HashSet<Room> rooms = new HashSet<Room>(p.CurrentRoom.NearestRooms);
            for (int i = 0; i < depth; i++)
            {
                HashSet<Room> newRooms = new HashSet<Room>(rooms.SelectMany(r => r.NearestRooms));
                rooms.UnionWith(newRooms);
            }
            return rooms;
        }

        // Reinitialise tous logs sauf celui qui vient d'être actiév
        private void ResetLogs(ref List<(int distance, string message, bool hasLogged)> proximityAlerts)
        {
            for (int i = 0; i < proximityAlerts.Count; i++)
            {
                var (distance, message, _) = proximityAlerts[i];
                proximityAlerts[i] = (distance, message, false);
            }
        }
    }*/
}
