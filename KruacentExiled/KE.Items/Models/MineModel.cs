

using Exiled.API.Features;
using Exiled.API.Features.Toys;
using MEC;
using System.Collections.Generic;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.Items.Models
{
    internal class MineModel : Model
    {
        private Light _light;
        internal override void Spawn(Vector3 spawnPos, Quaternion _)
        {
            //spawn + offset
            Position = spawnPos + new Vector3(0, .05f);
            Vector3 sizeDisk = new Vector3(.7f, 0.1f, .7f);
            Vector3 posLight = Position + new Vector3(0, sizeDisk.y);

            var baseMine = Primitive.Create(PrimitiveType.Cylinder, Position, null, sizeDisk, true);
            baseMine.Color = Color.black;
            var lightGlobe = Primitive.Create(PrimitiveType.Sphere, posLight, null, new Vector3(.1f, .1f, .1f));
            var lightMine = Light.Create(posLight + new Vector3(0, 0.1f), null, null, true, Color.red);
            lightMine.UnSpawn();
            lightMine.Intensity = .55f;

            baseMine.Collidable = false;
            lightGlobe.Color = new Color(1, 0, 0, .33f);
            lightGlobe.Collidable = false;

            Toys.Add(lightGlobe);
            Toys.Add(baseMine);
            Toys.Add(lightMine);
            _light = lightMine;
        }

        internal IEnumerator<float> Activate()
        {
            if (_light == null) throw new System.Exception("no light");
            while (Round.InProgress)
            {
                _light.Spawn();
                yield return Timing.WaitForSeconds(3);
                _light.UnSpawn();
                yield return Timing.WaitForSeconds(5);
            }

        }
    }
}
