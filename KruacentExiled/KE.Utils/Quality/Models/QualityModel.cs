using Exiled.API.Features;
using Exiled.API.Features.Toys;
using KE.Utils.Extensions;
using KE.Utils.Quality.Enums;
using KE.Utils.Quality.Models.Base;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KE.Utils.Quality.Models
{
    public abstract class QualityModel : ModelPrefab
    {
        public abstract ModelQuality Quality { get; }
        public abstract bool IsPickup { get; }




        public override sealed Model Create(Vector3 position, Quaternion rotation,bool spawn = true)
        {

            HashSet<BaseModel> clonetoys = new();
            HashSet<BaseModel> toys = GetBaseModels().ToHashSet();
            foreach (BaseModel model in toys)
            {
                BaseModel clone = (BaseModel)model.Clone();
                
                if (IsPickup)
                    clone.AdminToy.SetAsPickupAdminToy(false);

                clone.AdminToy.SetQuality(Quality,false);
                clonetoys.Add(clone);
            }
            QualityHandler.Sync();
            return new Model(this,position, rotation, clonetoys, spawn);
        }

    }
}
