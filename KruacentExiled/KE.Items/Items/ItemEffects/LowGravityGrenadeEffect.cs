using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Items.API.Interface;
using MEC;
using PlayerRoles.FirstPersonControl;
using System.Collections.Generic;
using UnityEngine;

namespace KE.Items.Items.ItemEffects
{
    public class LowGravityGrenadeEffect : CustomItemEffect
    {
        public float Duration { get; set; } = 15f;
        public float Range { get; set; } = 10f;

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
                OnExploding(ev.Player);
            }

            
        }

        public void OnExploding(Player player)
        {
            if (player is null) return;

            if (player.Role is FpcRole fpcRole) 
            { 
                fpcRole.Gravity = FpcGravityController.DefaultGravity * 0.15f;
            }

            Timing.CallDelayed(Duration, () =>
            {
                if (player.Role is FpcRole fpcRole)
                {
                    fpcRole.Gravity = FpcGravityController.DefaultGravity;
                }
            });
        }
    }
}