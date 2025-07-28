using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using KE.Utils.Quality.Structs;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.Utils.Quality.Models.Base
{

    /// <summary>
    /// The blocks to build a model with
    /// </summary>
    public abstract class BaseModel : ICloneable
    {

        protected AdminToy _toy;
        public  AdminToy AdminToy { get { return _toy; } }
        private Vector3 _position;
        public Vector3 LocalPosition { get { return _position; } }
        private Quaternion _rotation;
        public Quaternion LocalRotation { get { return _rotation; } }


        public BaseModel(Vector3 localPositon, Quaternion localRotation)
        {
            _position = localPositon;
            _rotation = localRotation;
        }

        public abstract object Clone();
    }

}
