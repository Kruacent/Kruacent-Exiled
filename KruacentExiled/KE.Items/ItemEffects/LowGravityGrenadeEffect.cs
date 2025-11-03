using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Items.Extensions;
using KE.Items.Interface;
using MEC;
using System.Collections.Generic;
using UnityEngine;
using PlayerLab = LabApi.Features.Wrappers.Player;

namespace KE.Items.ItemEffects
{
    public class LowGravityGrenadeEffect : CustomItemEffect
    {
        private Dictionary<Player, Vector3> _effectedPlayers = new();
        public Vector3 LowGravity { get; set; } = new(0, -12.60f, 0);
        public float Duration { get; set; } = 15f;
        public float Range { get; set; } = 10f;

        public override void Effect(UsedItemEventArgs ev)
        {
            
        }
        public override void Effect(DroppingItemEventArgs ev)
        {
            ev.Player.ItemEffectHint("No grandson don't leave me !");
        }

        public override void Effect(ExplodingGrenadeEventArgs ev)
        {
            OnExploding(ev);
        }

        public void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            ev.IsAllowed = false;

            foreach (Player player in Player.List)
            {
                if (Vector3.Distance(ev.Position, player.Position) <= this.Range)
                {
                    Vector3 previousGravity = PlayerLab.Get(player.NetworkIdentity)!.Gravity;
                    _effectedPlayers[player] = previousGravity;
                    PlayerLab.Get(player.NetworkIdentity)!.Gravity = LowGravity;
                    Timing.CallDelayed(this.Duration, () =>
                    {
                        PlayerLab.Get(ev.Player.NetworkIdentity)!.Gravity = _effectedPlayers[ev.Player];
                        _effectedPlayers.Remove(ev.Player);
                    });
                }
            }
        }
    }
}