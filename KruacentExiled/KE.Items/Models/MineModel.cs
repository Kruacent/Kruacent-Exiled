

using Exiled.API.Features.Toys;
using System.Collections.Generic;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.Items.Models
{
    internal class MineModel : Model
    {
        Color LightColor { get; set; } = Color.red;
        internal override void Spawn(Vector3 spawnPos)
        {
            Position = spawnPos;
            var baseMine = Primitive.Create(PrimitiveType.Cylinder, spawnPos, null, new Vector3(.7f, 0.1f, .7f), true);
            var lightGlobe = Primitive.Create(PrimitiveType.Sphere, spawnPos+spawnPos, null, new Vector3(.1f, .1f, .1f));
            var lightMine = Light.Create(spawnPos, null, null, true, LightColor);

            baseMine.Collidable = false;
            lightGlobe.Color = LightColor;
            lightGlobe.Collidable = false;

            Toys.Add(lightGlobe);
            Toys.Add(baseMine);
            Toys.Add(lightMine);
        }
    }
}
