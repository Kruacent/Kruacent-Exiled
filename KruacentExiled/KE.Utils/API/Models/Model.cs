using Exiled.API.Features;
using Exiled.API.Features.Core;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Toys;
using Exiled.API.Structs;
using KE.Utils.API.Models.Blueprints;
using KE.Utils.API.Models.ToysSettings;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

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

        private int _id;
        public int Id
        {
            get { return _id; }
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


        private void AddToy(AdminToy toy)
        {
            _toys.Add(toy);
        }

        private Model(Vector3 center)
        {
            _center = center;
        }


        internal static Model Create(Vector3 position, IEnumerable<AdminToy> toys, string name = "")
        {
            var m = Create(position, name);
            m._toys = toys.ToHashSet();

            return m;
        }

        public static Model Create(Vector3 position, string name = "")
        {

            Model m = new(position);
            _models.Add(m);
            m._id = _models.Count;
            if (string.IsNullOrEmpty(name))
            {
                m._name = "Model" + m.Id;
            }
            else
            {
                m._name = name;
            }

            Log.Debug("created model id=" + m.Id);
            m.centerPrim = Primitive.Create(position, null, Vector3.one / 5, true, new(1, 0, 0, .25f));
            m.centerPrim.Collidable = false;

            return m;
        }

        public static Model Create(ModelBlueprint blueprint,Vector3 position)
        {
            Model m = new(position);
            m._id = _models.Count;

            foreach (AdminToyBlueprint toy in blueprint.Toys)
            {
                m.AddToy(toy.Spawn(m.Center));
            }

            return m;

        }

        /// <summary>
        /// Create a <see cref="ModelBlueprint"/> based of this <see cref="Model"/> <br></br>
        /// Note : will overwrite the old blueprint
        /// </summary>
        public void CreateBlueprint()
        {
            int oldId = -1;
            if (LoadedFromBlueprint)
            {
                oldId = _blueprint.Id;
            }

            ModelBlueprint bp = ModelBlueprint.Create(this, oldId);

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

        
        public static Model Get(int id)
        {
            foreach(Model m in _models)
            {
                if (m.Id == id)
                    return m;
            }
            return null;
        }
        public static bool TryGet(int id, out Model model)
        {
            model = Get(id);
            return model != null;
        }

        public static bool TryGet(string name, out Model model)
        {
            model = Get(name);
            return model != null;
        }


        public override string ToString()
        {
            return $"{Id} : {Name} center = {Center}";
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
