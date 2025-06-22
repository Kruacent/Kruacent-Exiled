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
namespace KE.Utils.API.Models
{
    public static class ModelLoader
    {
        public static string Path => Paths.Configs + @"\";
        private static readonly char SEPARATOR = '_';


        public static Model Load(string filename)
        {
            return Load(Path, filename);
        }

        public static Model Load(string path,string filename)
        {
            
            string[] raw = File.ReadAllText(path+ filename).Split('\n');
            string[] infoline = raw[0].Split(SEPARATOR);
            foreach(string i in infoline)
            {
                Log.Info(i);
            }
            string name = infoline[0];
            Vector3 center = Parser.Vector3(infoline[1]);
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




            return Model.Create(center, name);
        }


        // Name.Center\nAdminToyType.ATPosition.ATRotationEuler.ATScale.LightPrimitiveColor.PrimitiveType.PrimitiveCollidable.LightIntensity\n and repeat
        //
        public static void Save(this Model m)
        {
            
            StringBuilder b = new();
            List<string> result = new();

            result.Add(m.Name+SEPARATOR+m.Center);

            foreach(AdminToy t in m.Toys)
            {
                b.Append(t.ToyType);
                b.Append(SEPARATOR);
                b.Append(t.Position);
                b.Append(SEPARATOR);
                b.Append(t.Rotation.eulerAngles);
                b.Append(SEPARATOR);
                b.Append(t.Scale);
                b.Append(SEPARATOR);

                if (t is Primitive p)
                {
                    b.Append("#"+ColorUtility.ToHtmlStringRGBA(p.Color));
                    b.Append(SEPARATOR);
                    b.Append(p.Type);
                    b.Append(SEPARATOR);
                    b.Append(p.Collidable);
                }
                if(t is Light l)
                {
                    b.Append("#" + ColorUtility.ToHtmlStringRGBA(l.Color));
                    b.Append(SEPARATOR);
                    b.Append(l.Intensity);
                }
                result.Add(b.ToString());
                b.Clear();
            }

            File.WriteAllLines(Path + m.Id + ".modelscpsl", result);
        }


        

    }
}
