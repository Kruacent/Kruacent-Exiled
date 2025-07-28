using KE.Utils.Quality.Enums;
using KE.Utils.Quality.Models.Base;
using System.Collections.Generic;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.Utils.Quality.Models.Examples
{
    public abstract class MineModel : QualityModel
    {
        protected Light _light;
        private bool _lightOn = false;


        

        public void ToggleLight()
        {
            if (_light == null) throw new System.Exception("no light");
            if (_lightOn)
                _light.UnSpawn();
            else
                _light.Spawn();
            _lightOn = !_lightOn;
        }

    }

    public class MineModelLow : MineModel
    {
        public override bool IsPickup => false;
        public override ModelQuality Quality => ModelQuality.Low;

        protected override IEnumerable<BaseModel> GetBaseModels()
        {
            Quaternion baseRotation = new();
            //spawn + offset
            Vector3 offset = new Vector3(0, .05f);
            Vector3 sizeDisk = new Vector3(.7f, 0.1f, .7f);

            Vector3 sizeGlobe = new Vector3(.1f, .1f, .1f);
            Vector3 posLight = new Vector3(0, sizeDisk.y+sizeGlobe.y);
            

            var baseMine = new PrimitiveModel(PrimitiveType.Cylinder, sizeDisk, Color.black, offset, baseRotation);

            var lightGlobe = new PrimitiveModel(PrimitiveType.Sphere, sizeGlobe , new Color(1, 0, 0, .33f), posLight, baseRotation);

            var lightMine = new LightModel(null, Color.red, posLight, baseRotation);

            _light = lightMine.AdminToy as Light;
            _light.Intensity = .55f;

            return [baseMine, lightGlobe, lightMine];
        }


    }

    public class MineModelMedium : MineModel
    {
        public override bool IsPickup => false;
        public override ModelQuality Quality => ModelQuality.Medium;

        protected override IEnumerable<BaseModel> GetBaseModels()
        {
            Quaternion baseRotation = new();
            //spawn + offset
            Vector3 offset = new Vector3(0, .05f);
            Vector3 sizeDisk = new Vector3(.7f, 0.1f, .7f);

            Vector3 sizeGlobe = new Vector3(.1f, .1f, .1f);
            Vector3 posLight = new Vector3(0, sizeDisk.y + sizeGlobe.y);


            var baseMine = new PrimitiveModel(PrimitiveType.Cylinder, sizeDisk, Color.black, offset, baseRotation);

            var lightGlobe = new PrimitiveModel(PrimitiveType.Sphere, sizeGlobe, new Color(1, 0, 0, .33f), posLight, baseRotation);

            var lightMine = new LightModel(null, Color.green, posLight, baseRotation);

            _light = lightMine.AdminToy as Light;
            _light.Intensity = .55f;

            return [baseMine, lightGlobe, lightMine];
        }
    }

    public class MineModelPickup : MineModel
    {
        public override bool IsPickup => true;
        public override ModelQuality Quality => ModelQuality.None;


        protected override IEnumerable<BaseModel> GetBaseModels()
        {
            Quaternion baseRotation = new();
            //spawn + offset
            Vector3 offset = new Vector3(0, .05f);
            Vector3 sizeDisk = new Vector3(.7f, 0.1f, .7f);

            Vector3 sizeGlobe = new Vector3(.1f, .1f, .1f);
            Vector3 posLight = new Vector3(0, sizeDisk.y + sizeGlobe.y);


            var baseMine = new PrimitiveModel(PrimitiveType.Cylinder, sizeDisk, Color.black, offset, baseRotation);

            var lightGlobe = new PrimitiveModel(PrimitiveType.Sphere, sizeGlobe, new Color(1, 0, 0, .33f), posLight, baseRotation);

            var lightMine = new LightModel(null, Color.magenta, posLight, baseRotation);

            _light = lightMine.AdminToy as Light;
            _light.Intensity = .55f;

            return [baseMine, lightGlobe, lightMine];
        }
    }
}
