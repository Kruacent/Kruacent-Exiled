

using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.Events.Commands.Reload;
using MEC;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.Items.Models
{
    internal class DeployWallModel : Model
    {
        const float distance = 2;
        internal override void Spawn(Vector3 spawnPos, Quaternion rotation)
        {

            Vector3 forward = rotation * Vector3.forward;
            Vector3 spawnPosi = spawnPos + forward * distance;
            Vector3 rotat = new Vector3(0, rotation.eulerAngles.y, 0);



        }

        internal void Change()
        {
            Timing.CallDelayed(10, () =>
            {
                UnSpawn();
            });
            Timing.CallDelayed(5, () =>
            {
                Toys.ForEach(t =>
                {
                    if (t is Primitive p) p.Color = Color.yellow;
                });
            });
            Timing.CallDelayed(8, () =>
            {
                Toys.ForEach(t =>
                {
                    if (t is Primitive p) p.Color = Color.red;
                });
            });
        }

        private void SpawnWall(Vector3 pos, Quaternion rotation)
        {
            Vector3 forward = rotation * Vector3.forward;
            Vector3 spawnPos = pos + forward * distance;
            Vector3 rotat = new Vector3(0, rotation.eulerAngles.y, 0);

            MainPlugin.Instance.Sound.PlayClip("build", spawnPos);
            Primitive wall = Primitive.Create(PrimitiveType.Cube, spawnPos, rotat, new Vector3(4, 4, 0.2f), true);
            wall.Collidable = true;
            wall.Visible = true;



        }
    }
}
