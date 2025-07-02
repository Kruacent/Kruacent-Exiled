using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;
using Exiled.API.Enums;
using KE.Utils.API.Models.Blueprints;
namespace KE.Utils.API.Models
{
    public static class ModelLoader
    {
        public static string Path => Paths.Configs + @"\";
        private static readonly char SEPARATOR = '_';
        public static string Extension => ".modelscpsl";


        public static IEnumerable<ModelBlueprint> LoadAll()
        {
            List<ModelBlueprint> m = new();

            string[] raw =  Directory.GetFiles(Path, "*" + Extension);

            foreach (string d in raw)
            {
                Log.Info("loading="+d);
                m.Add(Load(string.Empty,d));
            }

            return m;
        }

        public static ModelBlueprint Load(string filename)
        {
            return Load(Path, filename);
        }

        public static ModelBlueprint Load(string path,string filename)
        {
            string[] raw;
            try
            {
                raw = File.ReadAllText(path + filename).Split('\n');
            }
            catch(Exception e)
            {
                Log.Error(e);
                return null;
            }


            string[] infoline = raw[0].Split(SEPARATOR);
            string name = infoline[0];
            List<AdminToy> toys = new();


            for (int i = 1; i < raw.Length; i++)
            {
                string[] line = raw[i].Split(SEPARATOR);
                AdminToy toy = null;
                AdminToyType type;
                if (!Enum.TryParse(line[0], out type)) continue;

                Vector3 ATPos = Parser.Vector3(line[1]);
                Vector3 ATRotation = Parser.Vector3(line[2]);
                Vector3 ATScale = Parser.Vector3(line[3]);
                if(type == AdminToyType.PrimitiveObject)
                {
                    Color color;
                    ColorUtility.TryParseHtmlString(line[4], out color);
                    PrimitiveType ptype;
                    Enum.TryParse(line[5], out ptype);
                    bool collidable = bool.Parse(line[6]);
                    var p = Primitive.Create(ptype, ATPos, ATRotation, ATScale, true, color);
                    p.Collidable = collidable;
                    toy = p;

                }
                if(type == AdminToyType.LightSource)
                {
                    Color color;
                    ColorUtility.TryParseHtmlString(line[4], out color);
                    
                    float intensity = float.Parse(line[5]);
                    var l = Light.Create(ATPos, ATRotation, ATScale, true, color);
                    l.Intensity = intensity;
                    toy = l;
                }

                if (toy == null) continue;
                toys.Add(toy);

            }




            return ModelBlueprint.Create(name);
        }


        // Name\n
        // Primitive.Position.RotationEuler.Scale.Color.PrimitiveType.Collidable\n
        // Light.Position.RotationEuler.Scale.Color.Intensity\n
        // and repeat
        public static bool Save(this ModelBlueprint m)
        {

            List<string> result = new();

            result.Add(m.Name+SEPARATOR);

            foreach(AdminToyBlueprint t in m.Toys)
            {
                result.Add(t.Loadable(SEPARATOR));
            }

            try
            {
                File.WriteAllLines(Path + m.Id + Extension, result);
                return true;
            }
            catch(Exception e)
            {
                Log.Error(e);
                return false;
            }
            
        }

        public static bool Save(this Model model)
        {
            if (!model.LoadedFromBlueprint)
                model.CreateBlueprint();


            return Save(model.Blueprint);
        }
        

    }
}
