using Exiled.API.Features;
using Exiled.API.Features.Core;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Toys;
using Exiled.API.Structs;
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
        private static List<Model> _models = new(1);
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

        internal Primitive centerPrim;

        public void SetCenterPrimitive(bool show)
        {
            if (show)
            {
                centerPrim.UnSpawn();
            }
            else
            {
                centerPrim.Spawn(); 
            }
        }



        public void Add(Primitive p)
        {
            AddToy(p);
            
        }
        public void Add(Light l)
        {
            AddToy(l);
        }


        private void AddToy(AdminToy toy)
        {
            _toys.Add(toy);
        }

        public void Spawn()
        {
            foreach(AdminToy t in Toys)
            {
                t.Spawn();
            }
        }
        public void UnSpawn()
        {
            foreach (AdminToy t in Toys)
            {
                t.UnSpawn();
            }
        }

        public void Destroy()
        {
            foreach (AdminToy t in Toys)
            {
                t.Destroy();
            }
        }

        internal static Model Create(Vector3 position,IEnumerable<AdminToy> toys, string name = "")
        {
            var m =Model.Create(position, name);
            m._toys = toys.ToHashSet();

            return m;
        }

        public static Model Create(Vector3 position, string name= "")
        {
            
            Model m = new();
            _models.Add(m);
            m._id = _models.Count;
            m._center = position;
            if (string.IsNullOrEmpty(name))
            {
                m._name = "Model" + m.Id;
            }
            else
            {
                m._name = name;
            }

            Log.Debug("created model id=" + m.Id);
            m.centerPrim = Primitive.Create(position, null, Vector3.one/5, true, new(1, 0, 0, .25f));
            m.centerPrim.Collidable = false;

            return m;
        }


        public static Model Get(string name)
        {
            foreach(Model m in _models)
            {
                if(m.Name == name)
                {
                    return m;
                }
            }
            return null;
        }

        public static Model Get(int id)
        {
            _models.TryGet(id, out Model m);
            return m;
        }
        public static bool TryGet(int id, out Model model)
        {
            model = Get(id);
            return model != null;
        }


        public override string ToString()
        {
            return $"{Id} : {Name} center = {Center}";
        }
    }
}
