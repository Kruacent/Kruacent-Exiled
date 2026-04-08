using Exiled.API.Features;
using Exiled.API.Features.Core;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using KE.Utils.Extensions;
using KE.Utils.Quality.Models.Base;
using KE.Utils.Quality.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.Utils.Quality.Models
{
    public abstract class ModelPrefab
    {
        
        protected abstract IEnumerable<BaseModel> GetBaseModels();

        public virtual Model Create(Vector3 position,Quaternion rotation, bool spawn = true)
        {

            HashSet<BaseModel> clonetoys = new();
            HashSet<BaseModel> toys = GetBaseModels().ToHashSet();
            foreach (BaseModel model in toys)
            {
                BaseModel clone = (BaseModel)model.Clone();
                clonetoys.Add(clone);
            }
            return new Model(this, position, rotation, clonetoys,spawn);

        }



        
    }
}
