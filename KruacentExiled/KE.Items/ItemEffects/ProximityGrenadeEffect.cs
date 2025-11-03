using AdminToys;
using Exiled.API.Enums;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Items.Extensions;
using KE.Items.Interface;
using MEC;
using System.Collections.Generic;
using UnityEngine;
using Exiled.API.Features;

namespace KE.Items.ItemEffects
{
    public class ProximityGrenadeEffect : CustomItemEffect
    {
        public float Duration { get; set; } = 10;
        public float Range { get; set; } = 50;

        public override void Effect(UsedItemEventArgs ev)
        {
            
        }
        public override void Effect(DroppingItemEventArgs ev)
        {
            
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
                if (Vector3.Distance(ev.Position, player.Position) <= Range)
                {
                    var color = GetTeamColor(player);
                    var lineColor = new Color(color.red, color.green, color.blue);
                    var direction = player.Position - ev.Position;
                    var distance = direction.magnitude;
                    var scale = new Vector3(0.1f, distance * 0.5f, 0.1f);
                    var laserPos = ev.Position + direction * 0.5f;
                    var rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
                    var laser = Primitive.Create(PrimitiveType.Cylinder, PrimitiveFlags.Visible, laserPos, rotation.eulerAngles,
                        scale, true, lineColor);
                    Timing.CallDelayed(this.Duration, laser.Destroy);
                }
            }
        }
        public (float red, float green, float blue) GetTeamColor(Player player)
        {
            float red;
            float green;
            float blue;

            switch (player.Role.Side)
            {
                case Side.Mtf:
                    red = 0;
                    green = 0.39f;
                    blue = 1;
                    break;
                case Side.ChaosInsurgency:
                    red = 0;
                    green = 0.51f;
                    blue = 0;
                    break;
                case Side.Scp:
                    red = 0.59f;
                    green = 0;
                    blue = 0;
                    break;
                default:
                    red = 1;
                    green = 0.41f;
                    blue = 0.71f;
                    break;
            }

            return (red, green, blue);
        }
    }
}