using AdminToys;
using Exiled.API.Enums;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Items.Interface;
using MEC;
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
            OnExploding(ev.Player.Position);
        }
        public override void Effect(DroppingItemEventArgs ev)
        {
            OnExploding(ev.Player.Position);
        }

        public override void Effect(ExplodingGrenadeEventArgs ev)
        {
            OnExploding(ev.Position);
        }

        public void OnExploding(Vector3 position)
        {
            foreach (Player player in Player.List)
            {
                if (Vector3.Distance(position, player.Position) <= Range)
                {
                    var color = GetTeamColor(player);
                    var lineColor = new Color(color.red, color.green, color.blue);
                    var direction = player.Position - position;
                    var distance = direction.magnitude;
                    var scale = new Vector3(0.1f, distance * 0.5f, 0.1f);
                    var laserPos = position + direction * 0.5f;
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