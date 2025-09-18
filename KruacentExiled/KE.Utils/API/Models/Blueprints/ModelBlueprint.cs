using Exiled.API.Features;
using Exiled.API.Features.Core;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Toys;
using Exiled.API.Structs;
using KE.Utils.API.Models.ToysSettings;
using KE.Utils.Quality.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.Utils.API.Models.Blueprints
{
    public class ModelBlueprint
    {
        private static List<ModelBlueprint> _bps = new();
        public static List<ModelBlueprint> Blueprints => _bps.ToList();
        private HashSet<AdminToyBlueprint> _toys = new();
        public IReadOnlyCollection<AdminToyBlueprint> Toys => [.. _toys];
        private string _name;
        public string Name
        {
            get { return _name; }
        }

        private ModelBlueprint() { }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ModelBlueprint Create(Model model)
        {
            var oldMbp = Get(model.Name);
            _bps.Remove(oldMbp);


            ModelBlueprint mbp = new();
            mbp._name = model.Name;
            _bps.Add(mbp);

            foreach(AdminToy toy in model.Toys)
            {
                mbp._toys.Add(AdminToyBlueprint.Create(toy,model.Center));
            }

            return mbp;
        }


        public static ModelBlueprint Create(string name,IEnumerable<AdminToy> toys = null)
        {
            ModelBlueprint mbp = new();
            _bps.Add(mbp);

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name null or empty");
            }

            if(toys != null)
            {
                foreach(AdminToy at in toys)
                {
                    mbp._toys.Add(AdminToyBlueprint.Create(at));
                }
            }



            mbp._name = name;

            return mbp;
        }

        public void Spawn(Vector3 position)
        {
            Model m = Model.Create(this, position,false);


        }


        #region getters
        public static ModelBlueprint Get(string name)
        {
            foreach (ModelBlueprint m in Blueprints)
            {
                if (m.Name == name)
                {
                    return m;
                }
            }
            return null;
        }

        public static bool TryGet(string name, out ModelBlueprint model)
        {
            model = Get(name);
            return model != null;
        }
        #endregion
    }
}
