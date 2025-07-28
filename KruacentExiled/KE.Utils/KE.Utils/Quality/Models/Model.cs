using Exiled.API.Features;
using Exiled.API.Features.Toys;
using KE.Utils.Quality.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Utils.Quality.Models
{
    public class Model : IEquatable<Model>
    {


        private Vector3 _position;
        public Vector3 Position
        {
            get { return _position; }
            set
            {
                _position = value;
            }
        }
        private Quaternion _rotation;
        public Quaternion Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
            }
        }
        private ModelPrefab _prefab;
        public ModelPrefab Prefab
        {
            get { return _prefab; }
        }



        private HashSet<BaseModel> _models;

        internal Model(ModelPrefab prefab, Vector3 position, Quaternion rotation,IEnumerable<BaseModel> toys,bool spawn) 
        {
            _prefab = prefab;
            Position = position;
            Rotation = rotation;
            _models = new HashSet<BaseModel>(toys);
            QualityHandler.Instance.QualityToysHandler.Sync();

            foreach (BaseModel based in toys)
            {
                AdminToy toy = based.AdminToy;

                if (toy is Primitive p)
                    p.Collidable = false;
                toy.AdminToyBase.transform.position = based.LocalPosition + Position;
                toy.AdminToyBase.transform.rotation = based.LocalRotation * Rotation;
            }
            if(spawn)
            {
                Spawn();
            }
        }


        public void Destroy()
        {
            foreach (BaseModel primitive in _models)
            {
                primitive.AdminToy.Destroy();
            }
        }
        public void UnSpawn()
        {
            foreach (BaseModel primitive in _models)
            {
                primitive.AdminToy.UnSpawn();
            }
        }
        public void Spawn()
        {
            QualityHandler.Instance.QualityToysHandler.Sync();
            foreach (BaseModel primitive in _models)
            {
                primitive.AdminToy.Spawn();
            }
            
        }


        public bool Equals(Model x)
        {
            if (x == null) return false;
            return x._prefab == _prefab && x.Position == Position && x.Rotation == Rotation;
        }
        public int GetHashCode(Model obj)
        {
            return obj.GetHashCode();
        }
    }
}
