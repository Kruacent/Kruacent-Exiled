using Exiled.API.Features.Toys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utf8Json.Formatters;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.Items.Items.LeSoleil
{
    public class SoleilComp : MonoBehaviour
    {

        public float TimeActive { get; set; }


        private Primitive @base = null;

        private Primitive Sun;
        private Light Light;
        public void Init(Primitive prim)
        {
            @base = prim;
            TimeActive = 30;
            Light = Light.Create(@base.Position, null, null, false);
            Light.LightType = LightType.Point;
            Light.Color = Color.yellow;
            Light.Intensity = 50;
            Light.Range = 300;

            Sun = Primitive.Create(@base.Position, null, Vector3.one * 3, false);
            Sun.Color = Color.yellow;
            Sun.Collidable = false;

            Sun.Spawn();
            Light.Spawn();
        }





        private void Update()
        {
            if (@base == null) return;
            TimeActive -= Time.deltaTime;

            if(TimeActive <= 0)
            {
                Destroy();
            }

        }


        public void Destroy()
        {
            Destroy(this);
            @base.Destroy();
            @base = null;
        }

    }
}
