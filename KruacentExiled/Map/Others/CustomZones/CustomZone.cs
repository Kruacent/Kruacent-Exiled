using Exiled.API.Extensions;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.Map.Others.CustomZones
{
    public abstract class CustomZone
    {
        public abstract CustomFacilityZone FacilityZone { get; }
        public Layout Layout { get; private set; }
        public abstract Vector3 Spawnzone { get; }
        public void Generate(System.Random rng)
        {
            Generate(rng,SetRandomLayout());
        }

        public void GenerateWithLayout(System.Random rng,Layout layout)
        {
            Generate(rng, layout);
        }
        public abstract void Generate(System.Random rng, Layout layout);

        private Layout SetRandomLayout()
        {
            Layout layout =Layout.Layouts.GetRandomValue();
            Layout = layout;
            return layout;

        }

    }
}
