using Exiled.API.Features;
using HarmonyLib;
using MapGeneration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Color = System.Drawing.Color;
namespace KruacentExiled.Map.Patches
{
    /*public static class MapGenerationPatches
    {

        [HarmonyPatch(typeof(MapAtlasInterpreter),nameof(MapAtlasInterpreter.Interpret))]
        public static class InterpretPatch
        {
            static int counter = 0;
            public static void Prefix(UnityEngine.Texture2D atlas, System.Random rng, MapAtlasInterpreter __instance)
            {
                //PrintAtlas(atlas);
            }


            public static void PrintAtlas(UnityEngine.Texture2D atlas)
            {

                var _bytes = EncodeToPNG(atlas);

                var dirPath = Paths.Configs + "/Images/";

                if (!Directory.Exists(dirPath))
                {

                    Directory.CreateDirectory(dirPath);

                }
                var name = "test" + counter;
                counter++;

                File.WriteAllBytes(dirPath + name + ".png", _bytes);
            }

            private static byte[] EncodeToPNG(Texture2D texture)
            {
                // Get raw pixel colors from the texture
                Color32[] pixels = texture.GetPixels32();

                using (Bitmap bmp = new Bitmap(texture.width, texture.height, PixelFormat.Format32bppArgb))
                {
                    for (int y = 0; y < texture.height; y++)
                    {
                        for (int x = 0; x < texture.width; x++)
                        {
                            Color32 c = pixels[y * texture.width + x];
                            bmp.SetPixel(x, texture.height - y - 1, Color.FromArgb(c.a, c.r, c.g, c.b));
                        }
                    }

                    using (MemoryStream ms = new MemoryStream())
                    {
                        bmp.Save(ms, ImageFormat.Png);
                        return ms.ToArray();
                    }
                }
            }

        }

        [HarmonyPatch(typeof(AtlasZoneGenerator), nameof(AtlasZoneGenerator.Generate))]
        public static class GeneratePatch
        {
            static bool flag = false;
            static int counter = 0;
            public static void Prefix(System.Random rng, AtlasZoneGenerator __instance)
            {

                if (!flag && __instance is LightContainmentZoneGenerator lcz)
                {
                    List<Texture2D> a = __instance.Atlases.ToList();
                    Texture2D tex = DecodePNGFromFile(Paths.Configs + "/atlais.png");
                    a.Clear();
                    a.Add(tex); 
                    __instance.Atlases = a.ToArray();

                    flag = true;
                }
                Log.Debug(counter);
                counter++;

                foreach (Texture2D atlas in __instance.Atlases)
                {
                    InterpretPatch.PrintAtlas(atlas);
                }
            }


            public static Texture2D DecodePNGFromFile(string path)
            {

                using (var bmp = new Bitmap(path))
                {
                    Texture2D tex = new Texture2D(bmp.Width, bmp.Height, TextureFormat.RGBA32, false);

                    for (int y = 0; y < bmp.Height; y++)
                    {
                        for (int x = 0; x < bmp.Width; x++)
                        {
                            var c = bmp.GetPixel(x, bmp.Height - 1 - y);
                            tex.SetPixel(x, y, new Color32(c.R, c.G, c.B, c.A));
                        }
                    }

                    tex.Apply();
                    return tex;
                }

            }


        }


    }*/
}
