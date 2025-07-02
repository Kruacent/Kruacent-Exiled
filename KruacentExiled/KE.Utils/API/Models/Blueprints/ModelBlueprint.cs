using Exiled.API.Features;
using Exiled.API.Features.Core;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Toys;
using Exiled.API.Structs;
using KE.Utils.API.Models.ToysSettings;
using KE.Utils.Quality.Models;
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

        private int _id;
        public int Id
        {
            get { return _id; }
        }

        private ModelBlueprint() { }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"> if > 0 overwrite same id bp </param>
        /// <returns></returns>
        public static ModelBlueprint Create(Model model, int id = -1)
        {
            if(id != -1)
            {
                
                var oldMbp = Get(id);
                _bps.Remove(oldMbp);
            }


            ModelBlueprint mbp = new();
            _bps.Add(mbp);
            mbp._id = _bps.Count;

            foreach(AdminToy toy in model.Toys)
            {
                mbp._toys.Add(AdminToyBlueprint.Create(toy,model.Center));
            }

            return mbp;
        }


        public static ModelBlueprint Create(string name)
        {
            ModelBlueprint mbp = new();
            _bps.Add(mbp);
            mbp._id = _bps.Count;

            if (string.IsNullOrEmpty(name))
            {
                mbp._name = "Model" + mbp.Id;
            }
            else
            {
                mbp._name = name;
            }

            return mbp;
        }


        #region getters
        public static ModelBlueprint Get(string name)
        {
            foreach (ModelBlueprint m in _bps)
            {
                if (m.Name == name)
                {
                    return m;
                }
            }
            return null;
        }


        public static ModelBlueprint Get(int id)
        {
            foreach (ModelBlueprint m in _bps)
            {
                if (m.Id == id)
                    return m;
            }
            return null;
        }
        public static bool TryGet(int id, out ModelBlueprint model)
        {
            model = Get(id);
            return model != null;
        }

        public static bool TryGet(string name, out ModelBlueprint model)
        {
            model = Get(name);
            return model != null;
        }
        #endregion
    }
}
