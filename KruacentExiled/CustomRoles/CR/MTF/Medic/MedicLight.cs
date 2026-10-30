using Exiled.API.Features.Toys;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KruacentExiled.CustomRoles.CR.MTF.Terroriste
{
    internal class MedicLight : MonoBehaviour
    {
        private ReferenceHub _hub;
        private Light light;
        private Primitive prim;

        private void Start()
        {
            _hub = ReferenceHub.GetHub(gameObject);
            CreateLight();
        }

        private void CreateLight()
        {
            light = Light.Create(null, null, null, false);
            light.Transform.parent = _hub.transform;
            light.Transform.localPosition = Vector3.down / 3f + Vector3.back / 4f;


            light.Color = Color.green;
            light.LightType = LightType.Spot;
            light.Intensity = intensity;
            light.Transform.localRotation = Quaternion.LookRotation(-_hub.transform.forward);
            light.SpotAngle = 111;
            light.MovementSmoothing = 0;
            light.Spawn();
        }


        private void OnDestroy()
        {
            light.Destroy();
            prim?.Destroy();
        }

        private int timer = 0;
        private const int objective = 100;

        private float intensity = 2;

        public void Update()
        {

            if (timer >= objective)
            {
                ToggleLight();
                timer = 0;
            }
            timer++;
        }


        public void ToggleLight()
        {
            if (light.Intensity == 0)
            {
                light.Intensity = intensity;
            }
            else
            {
                light.Intensity = 0;
            }
        }
    }
}
