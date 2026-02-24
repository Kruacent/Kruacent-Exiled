using Exiled.API.Features;
using Exiled.API.Features.Pools;
using MapGeneration;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Color = System.Drawing.Color;

namespace KE.Map.Others.CustomZones
{
    public static class AltasReader
    {


        public static readonly Dictionary<Color32, RoomShape> ColortoRoom = new()
        {
            { new Color32(0,255,0,255), RoomShape.Endroom },
            { new Color32(255,0,0,255), RoomShape.TShape },
            { new Color32(0,0,255,255), RoomShape.Curve },
            { new Color32(255,255,0,255), RoomShape.Straight },
            { new Color32(255,0,255,255), RoomShape.XShape }
        };

        public static string Path => Paths.Configs + "/Atlases/";

        public class RoomShapeRotation
        {

            public RoomShape RoomShape { get; }
            public Vector3 Rotation { get; }


            public RoomShapeRotation(RoomShape roomShape, Vector3 rotation)
            {
                RoomShape = roomShape;
                Rotation = rotation;
            }


        }


        public static void Read()
        {
            if (!Directory.Exists(Path))
            {
                Log.Warn("no atlases directory. creating");
                Directory.CreateDirectory(Path);
                return ;
            }

            HashSet<Layout> layouts = new();

            IEnumerable<string> files = Directory.GetFiles(Path, "*.png");

            Dictionary<Vector2Int, RoomShapeRotation> coordtoroom = new();

            foreach (string file in files)
            {

                string noExFile = System.IO.Path.GetFileName(file);
                Log.Debug($"loading {file} as {noExFile}");
                using Bitmap bmp = new Bitmap(file);
                int width = bmp.Width;
                int height = bmp.Height;

                if(width % 3 != 0 || height % 3 != 0)
                {
                    throw new FileLoadException("wrong size : must be a multiple of 3");
                }

                for (int y = 1; y < height; y += 3)
                {
                    for (int x = 1; x < width; x+= 3)
                    {

                        Color pixel = bmp.GetPixel(x, y);
                        if (pixel.A == 0) continue;
                        Color pixelup = bmp.GetPixel(x, y+1);
                        Color pixeldown = bmp.GetPixel(x, y-1);
                        Color pixelleft = bmp.GetPixel(x-1, y);
                        Color pixelright = bmp.GetPixel(x+1, y);

                        Dictionary<Vector2Int,Color> colors = new()
                        {
                            { new(0,1),pixelup },
                            { new(0,-1),pixeldown },
                            { new(-1,0),pixelleft },
                            { new(1,0),pixelright }
                        };



                        List<Vector2Int> empty = ListPool<Vector2Int>.Pool.Get();
                        foreach(Vector2Int pos in colors.Keys)
                        {
                            if(colors[pos].A == 0)
                            {
                                empty.Add(pos);
                            }
                        }
                        int numEmpty = empty.Count;

                        RoomShape shape = RoomShape.Undefined;
                        Vector3 rotation = Vector3.zero;

                        if (numEmpty == 0)
                        {
                            shape = RoomShape.XShape;
                            rotation = Vector3.zero;
                        }

                        if(numEmpty == 1)
                        {
                            shape = RoomShape.TShape;
                            Vector2Int coordempty = empty[0];
                            rotation = new(0, coordempty.x * 90,0);

                            if (coordempty.x == 0 && coordempty.y == 1)
                            {
                                rotation = Vector3.zero;
                            }
                            if (coordempty.x == 0 && coordempty.y == -1)
                            {
                                rotation = new Vector3(0, 180, 0);
                            }
                        }


                        if(numEmpty == 2)
                        {
                            if (Mathf.Abs(empty[0].x) == Mathf.Abs(empty[1].x) 
                                || Mathf.Abs(empty[0].y) == Mathf.Abs(empty[1].y))
                            {
                                shape = RoomShape.Straight;
                                if(empty[0].x == 0)
                                {
                                    rotation = Vector3.zero;
                                }
                                else
                                {
                                    rotation = new Vector3(0, 90, 0);
                                }
                            }
                            else
                            {
                                shape = RoomShape.Curve;

                                Dictionary<Vector2Int, Vector3> coordToRotation = new()
                                {
                                    {new (1,1),Vector3.zero },
                                    {new (-1,1), new Vector3(0,90,0) },
                                    {new (-1,-1), new Vector3(0,180,0) },
                                    {new (1,-1), new Vector3(0,-90,0) }
                                };


                                foreach(var kvp in coordToRotation)
                                {
                                    if (!empty.Contains(new Vector2Int(0, kvp.Key.y)) &&
                                        !empty.Contains(new Vector2Int(kvp.Key.x, 0)))
                                    {
                                        rotation = kvp.Value;
                                    }
                                }

                            }
                        }


                        if(numEmpty == 3)
                        {
                            Vector2Int coordnotempty = Vector2Int.zero;
                            shape = RoomShape.Endroom;
                            foreach (Vector2Int pos in colors.Keys)
                            {
                                if (!empty.Contains(pos))
                                {
                                    coordnotempty = pos;
                                }
                            }


                            if (coordnotempty.x == 0 && coordnotempty.y == 1)
                            {
                                rotation = Vector3.zero;
                            }
                            if (coordnotempty.x == 0 && coordnotempty.y == -1)
                            {
                                rotation = new Vector3(0, 180, 0);
                            }
                            if (coordnotempty.x == 1 && coordnotempty.y == 0)
                            {
                                rotation = new Vector3(0, 90, 0);
                            }
                            if (coordnotempty.x == -1 && coordnotempty.y == 0)
                            {
                                rotation = new Vector3(0, -90, 0);
                            }



                        }




                        /*Color32 color = new Color32(pixel.R, pixel.G, pixel.B, pixel.A);
                        

                        if (!ColortoRoom.ContainsKey(color))
                        {
                            throw new ArgumentException($"[{noExFile}] Unrecognized color at {coord}");
                        }
                        shape = ColortoRoom[color];
                        */


                        Vector2Int coord = new Vector2Int(x, y);

                        if(shape == RoomShape.Undefined)
                        {
                            Log.Warn($"found undefined at {coord} room skipping");
                            continue;
                        }
                        Log.Debug($"adding {shape} at {coord} ({rotation})");
                        coordtoroom.Add(coord, new (shape,rotation));

                    }
                }
                
                Layout layout = new(coordtoroom);
                coordtoroom.Clear();
            }
        }

        




    }
}
