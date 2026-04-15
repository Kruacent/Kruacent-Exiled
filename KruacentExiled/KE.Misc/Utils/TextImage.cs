using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features.Pools;
using LabApi.Features.Wrappers;
using UnityEngine;
using Color = System.Drawing.Color;
using Exiled.API.Features.Toys;
using Exiled.API.Features;
using UnityEngine.Rendering;
using Graphics = System.Drawing.Graphics;
using Exiled.API.Interfaces;

namespace KE.Misc.Utils
{
    public class TextImage : IEquatable<TextImage>
    {
        private static HashSet<TextImage> list = new HashSet<TextImage>();

        private string rawString = string.Empty;
        private HashSet<TextToy> spawnedTextToys = new HashSet<TextToy>();

        public static Vector2 DefaultDisplaySize => new Vector2(5000, 5000);

        public static string StartColorTag => "<color=";
        public static string EndColorTag => "</color>";
        public static string DefaultCharacter => "█";

        public TextImage(Image img)
        {
            
            var b = new Bitmap(img,img.Width, img.Height);
            StringBuilder sb = StringBuilderPool.Pool.Get();
            //Log.Info($"{b.Height}/{b.Width}");

            for (int y = 0; y < b.Height; y++)
            {
                Color? oldColor = null;
                int nb = 0;

                for (int x = 0; x < b.Width; x++)
                {
                    Color pixelColor = b.GetPixel(x, y);

                    if (oldColor is null)
                    {
                        oldColor = pixelColor;
                        nb = 1;
                        continue;
                    }

                    if (ColorEquals(oldColor, pixelColor))
                    {
                        nb++;
                    }
                    else
                    {
                        sb.Append(StartColorTag);
                        sb.Append(ToHexValue(oldColor.Value) + ">");
                        for (int i = 0; i < nb; i++)
                            sb.Append(DefaultCharacter);
                        sb.Append(EndColorTag);

                        oldColor = pixelColor;
                        nb = 1;
                    }
                }

                if (nb > 0 && oldColor != null)
                {
                    sb.Append(StartColorTag);
                    sb.Append(ToHexValue(oldColor.Value) + ">");
                    for (int i = 0; i < nb; i++)
                        sb.Append(DefaultCharacter);
                    sb.Append(EndColorTag);
                }

                sb.AppendLine();
            }
            rawString = sb.ToString();

            StringBuilderPool.Pool.Return(sb);
            list.Add(this);
        }

        public static bool ColorEquals(Color? color1,Color? color2)
        {
            if (color1 is null || color2 is null) return false;

            Color tcolor1 = color1 ?? Color.Black;
            Color tcolor2 = color2 ?? Color.Black;

            return tcolor1.R == tcolor2.R &&
                tcolor1.G == tcolor2.G &&
                tcolor1.B == tcolor2.B &&
                tcolor1.A == tcolor2.A;
        }


        //Todo change texttoy
        public TextToy Spawn(Vector3 position,float pixelSize = 1f, Quaternion? rotation = null, Transform parent = null)
        {
            Vector3 scale = new Vector3(1,.5f,1)* pixelSize;

            TextToy textToy = TextToy.Create(position, rotation ?? Quaternion.Euler(Vector3.zero), scale, parent, false);
            textToy.DisplaySize = DefaultDisplaySize;
            textToy.TextFormat = rawString;


            textToy.Spawn();

            spawnedTextToys.Add(textToy);
            return textToy;
        }


        public void Destroy(TextToy textToy)
        {
            if (textToy == null) throw new ArgumentNullException();

            if (spawnedTextToys.Remove(textToy))
            {
                textToy.Destroy();
            }

            
        }

        public void Destroy()
        {
            if (spawnedTextToys is null) return;

            
            
            foreach(TextToy tt in spawnedTextToys)
            {
                if (!tt.IsDestroyed)
                {
                    tt?.Destroy();
                }
                
            }
        }




        private static string ToHexValue(Color color)
        {
            return "#" + color.R.ToString("X2") +
                         color.G.ToString("X2") +
                         color.B.ToString("X2") +
                         color.A.ToString("X2");
        }

        public bool Equals(TextImage other)
        {
            return spawnedTextToys == other.spawnedTextToys;
        }
    }
}
