using AdminToys;
using Exiled.API.Enums;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using MEC;
using UnityEngine;
using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using KruacentExiled.CustomItems.API.Interface;

namespace KruacentExiled.CustomItems.Items.ItemEffects
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

            for (int i = 0; i < RoomRadius; i++)
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

            foreach (Player player in Player.Enumerable.Where(p => p.IsAlive))
            {
                if (roomsInRange.Contains(player.CurrentRoom))
                {
                    var lineColor = player.Role.Color;

                    var direction = player.Position - position;
                    var distance = direction.magnitude;

                    var scale = new Vector3(0.1f, distance * 0.5f, 0.1f);
                    var laserPos = position + direction * 0.5f;

                    var rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);

                    var laser = Primitive.Create(PrimitiveType.Cylinder, PrimitiveFlags.Visible, laserPos, rotation.eulerAngles, scale, true, lineColor);

                    Timing.CallDelayed(Duration, laser.Destroy);
                }
            }
        }


    }
}