using Exiled.API.Features;
using Exiled.API.Features.Toys;
using KE.Utils.API.Models.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KE.Utils.API.Models
{
    public class Model
    {
        private static List<Model> _models = new();
        public static List<Model> Models => _models.ToList();

        private HashSet<AdminToy> _toys = new();
        public IReadOnlyCollection<AdminToy> Toys => [.. _toys];
        private string _name;
        public string Name
        {
            get { return _name; }
        }

        private Vector3 _center;
        public Vector3 Center
        {
            get { return _center; }
        }
        private bool _spawned = true;
        public bool Spawned
        {
            get { return _spawned; }
        }

        private ModelBlueprint _blueprint;

        public ModelBlueprint Blueprint
        {
            get { return _blueprint; }
        }

        public bool LoadedFromBlueprint
        {
            get
            {
                return Blueprint != null;
            }
        }

        



        internal Primitive centerPrim;

        public void SetCenterPrimitive(bool show)
        {
            if (show)
            {
                centerPrim.Spawn();
            }
            else
            {
                centerPrim.UnSpawn();
            }
        }



        public Primitive Add(Vector3 pos)
        {
            var p = Primitive.Create(pos, null, null, true);

            AddToy(p);
            return p;
        }


        protected virtual void AddToy(AdminToy toy,bool editMode = true)
        {
            if(toy is Primitive p && editMode)
            {
                p.Collidable = true;
            }


            _toys.Add(toy);
        }

        private Model(Vector3 center)
        {
            _models.Add(this);
            _center = center;
        }

        public static Model Create(Vector3 position, string name)
        {

            Model m = new(position);
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name null or empty");
            }
            m._name = name;

            Log.Debug("created model id=" + m.Name);
            m.centerPrim = Primitive.Create(position, null, Vector3.one / 5, true, new(1, 0, 0, .25f));
            m.centerPrim.Collidable = false;

            return m;
        }


        public static Model Create(ModelBlueprint blueprint,Vector3 position,bool editMode = false)
        {
            Model m = new(position);
            m._name = blueprint.Name;

            foreach (AdminToyBlueprint toy in blueprint.Toys)
            {
                Log.Info("create toy "+toy.AdminToyType);
                AdminToy trueToy = toy.Spawn(m.Center);
                

                m.AddToy(trueToy, editMode);
                
            }
            
            return m;

        }

        /// <summary>
        /// Create a <see cref="ModelBlueprint"/> based of this <see cref="Model"/> <br></br>
        /// Note : will overwrite the old blueprint
        /// </summary>
        public void CreateBlueprint()
        {
            ModelBlueprint bp = ModelBlueprint.Create(this);

            _blueprint = bp;
            
        }


        #region getter
        public static Model Get(string name)
        {
            foreach (Model m in _models)
            {
                if (m.Name == name)
                {
                    return m;
                }
            }
            return null;
        }

        public static bool TryGet(string name, out Model model)
        {
            model = Get(name);
            return model != null;
        }


        public override string ToString()
        {
            return $"{Name} center = {Center}";
        }
        #endregion

        #region spawn
        public void Spawn()
        {
            foreach (AdminToy t in Toys)
            {
                t.Spawn();
            }
            _spawned = true;
        }
        public void UnSpawn()
        {
            foreach (AdminToy t in Toys)
            {
                t.UnSpawn();
            }
            _spawned = false;
        }

        public void Destroy()
        {
            foreach (AdminToy t in Toys)
            {
                t.Destroy();
            }
            _models.Remove(this);
            
        }

        #endregion
    }
}
