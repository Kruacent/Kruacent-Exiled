using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using MEC;
using PlayerRoles.FirstPersonControl;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.CustomItems.Items.ItemEffects
{
    public class LowGravityGrenadeEffect : CustomItemEffect
    {
        public static float Duration { get; set; } = 15f;
        public float Range { get; set; } = 10f;

        public static Dictionary<Player,DateTime> affectedPlayers = new Dictionary<Player, DateTime>();

        public override void Effect(UsedItemEventArgs ev)
        {
            OnExploding(ev.Player);
        }
        public override void Effect(DroppingItemEventArgs ev)
        {
            OnExploding(ev.Player);
        }

        public override void Effect(ExplodingGrenadeEventArgs ev)
        {
            ev.IsAllowed = false;

            foreach(Player player in ev.TargetsToAffect)
            {
                OnExploding(player);
            }

            
        }

        public void OnExploding(Player player)
        {
            if (player is null) return;

            if (player.Role is FpcRole fpcRole) 
            { 
                fpcRole.Gravity = FpcGravityController.DefaultGravity * 0.15f;
                affectedPlayers.Add(player,DateTime.Now);
            }

            Timing.CallDelayed(Duration, () =>
            {
                ResetGravity(player);
            });
        }

        public static void ResetGravity(Player player)
        {
            if (player.Role is FpcRole fpcRole)
            {
                KELog.Debug("reset gravity");
                fpcRole.Gravity = FpcGravityController.DefaultGravity;
                affectedPlayers.Remove(player);
            }
        }
    }
}