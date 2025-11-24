using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
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
        public Vector3 LowGravity { get; set; } = new(0, -12.6f, 0);
        public float Duration { get; set; } = 15f;
        public float Range { get; set; } = 10f;

        public override void Effect(UsedItemEventArgs ev)
        {
            OnExploding(ev.Player, ev.Player.Position);
        }
        public override void Effect(DroppingItemEventArgs ev)
        {
            OnExploding(ev.Player, ev.Player.Position);
        }

        public override void Effect(ExplodingGrenadeEventArgs ev)
        {
            ev.IsAllowed = false;
            OnExploding(ev.Player, ev.Position);
        }

        public void OnExploding(Player thrownPlayer, Vector3 position)
        {
            foreach (Player player in Player.List)
            {
                if (Vector3.Distance(position, player.Position) <= this.Range)
                {
                    Vector3 previousGravity = PlayerLab.Get(player.NetworkIdentity)!.Gravity;
                    _effectedPlayers[player] = previousGravity;
                    PlayerLab.Get(player.NetworkIdentity)!.Gravity = LowGravity;
                    Timing.CallDelayed(this.Duration, () =>
                    {
                        PlayerLab.Get(thrownPlayer.NetworkIdentity)!.Gravity = _effectedPlayers[thrownPlayer];
                        _effectedPlayers.Remove(thrownPlayer);
                    });
                }
            }
        }
    }
}