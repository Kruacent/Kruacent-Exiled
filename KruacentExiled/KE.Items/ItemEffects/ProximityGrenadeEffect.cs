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
        public float Duration { get; set; } = 20;
        public float RoomRadius { get; set; } = 3;

        public override void Effect(UsedItemEventArgs ev)
        {
            OnExploding(ev.Player.CurrentRoom, ev.Player.Position);
        }
        public override void Effect(DroppingItemEventArgs ev)
        {
            OnExploding(ev.Player.CurrentRoom, ev.Player.Position);
        }

        public override void Effect(ExplodingGrenadeEventArgs ev)
        {
            ev.IsAllowed = false;
            OnExploding(ev.Projectile.Room, ev.Position);
        }

        public void OnExploding(Room originRoom, Vector3 position)
        {
            HashSet<Room> roomsInRange = new HashSet<Room>();

            roomsInRange.Add(originRoom);

            List<Room> currentLayer = new List<Room>();
            currentLayer.Add(originRoom);

            for (int i = 0; i < this.RoomRadius; i++)
            {
                List<Room> nextLayer = new List<Room>();

                foreach (Room r in currentLayer)
                {
                    foreach (Room neighbor in r.NearestRooms)
                    {
                        if (roomsInRange.Add(neighbor))
                        {
                            nextLayer.Add(neighbor);
                        }
                    }
                }
                currentLayer = nextLayer;
            }

            foreach (Player player in Player.List)
            {
                if (roomsInRange.Contains(player.CurrentRoom))
                {
                    var color = GetTeamColor(player);

                    var lineColor = new Color(color.red, color.green, color.blue);

                    var direction = player.Position - position;
                    var distance = direction.magnitude;

                    var scale = new Vector3(0.1f, distance * 0.5f, 0.1f);
                    var laserPos = position + direction * 0.5f;

                    var rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);

                    var laser = Primitive.Create(PrimitiveType.Cylinder, PrimitiveFlags.Visible, laserPos, rotation.eulerAngles, scale, true, lineColor);

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