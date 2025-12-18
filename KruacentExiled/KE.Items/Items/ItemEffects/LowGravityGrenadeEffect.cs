using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Items.API.Interface;
using MEC;
using System.Collections.Generic;
using UnityEngine;
using PlayerLab = LabApi.Features.Wrappers.Player;

namespace KE.Items.Items.ItemEffects
{
    public class LowGravityGrenadeEffect : CustomItemEffect
    {
        private Dictionary<PlayerLab, Vector3> _effectedPlayers = new();
        public Vector3 LowGravity { get; set; } = new(0, -12.6f, 0);
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

        public void OnExploding(PlayerLab player)
        {
            if (player is null) return;

            Vector3 previousGravity = player.Gravity;
            _effectedPlayers[player] = previousGravity;
            player.Gravity = LowGravity;
            Timing.CallDelayed(Duration, () =>
            {
                if (player is not null)
                {
                    player.Gravity = _effectedPlayers[player];
                    _effectedPlayers.Remove(player);
                }
                
            });
        }
    }
}