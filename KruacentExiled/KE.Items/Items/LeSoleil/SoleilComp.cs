using Exiled.API.Features;
using Exiled.API.Features.Toys;
using KE.Utils.API.Features;
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
        private Light[] Lights;
        public void Init(Primitive prim)
        {
            @base = prim;
            @base.Spawn();
            TimeActive = 300;
            Lights = new Light[3];
            time = 0;
            

            Sun = Primitive.Create(@base.Position, null, Vector3.one * 3, false);
            Sun.Color = Color.yellow;
            Sun.Collidable = false;

            Light l = Light.Create(@base.Position, null, null, false);
            l.LightType = LightType.Directional;
            l.Color = Color.yellow;
            l.Intensity = 10;
            l.Rotation = Quaternion.Euler(135, 0, 0);
            l.ShadowType = LightShadows.None;
            Lights[0] = l;
            l.Spawn();

            KELog.Debug("soucisse");

            Sun.Spawn();
        }

        private float time;



        private void Update()
        {
            if (@base == null) return;
            time += Time.deltaTime;

            float intensity = time / 2;

            Lights[0].Intensity = Mathf.Min(50, intensity);

            Sun.Scale = Mathf.Min(50, intensity)* Vector3.one;

            if (time >= TimeActive)
            {
                Destroy();
            }

        }


        private void OnDestroy()
        {
            try
            {
                Lights[0].Destroy();
                Lights = null;
                Sun.Destroy();
                Sun = null;
                @base.Destroy();
                @base = null;
            }
            catch(Exception e)
            {
                Log.Error(e);
            }

            Log.Debug("on desroy");
        }

        public void Destroy()
        {
            Log.Debug("desroy");
            Destroy(this);

        }

    }
}
