using Exiled.API.Features.Doors;
using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoiceChat.Networking;

namespace KE.Map.Surface.BlinkingBlocks
{
    public class BlinkingBlock : IWorldSpace
    {


        public Vector3 Position { get; }

        public Quaternion Rotation { get; }

        public BlockColor BlockColor { get; }




        private HashSet<Primitive> _list = new HashSet<Primitive>();
        private HashSet<Primitive> outline = new HashSet<Primitive>();
        public BlinkingBlock(Vector3 pos, Quaternion rotation, Vector3 scale, BlockColor color)
        {
            Position = pos;
            Rotation = rotation;
            BlockColor = color;
            _list.Add(Primitive.Create(PrimitiveType.Cube, pos, rotation.eulerAngles, scale, false, BlockColorToColor(color)));
        }



        public void Switch(BlockColor newColor)
        {
            foreach(Primitive p in _list)
            {
                if (newColor == BlockColor)
                {
                    p.Spawn();
                }
                else
                {
                    p.UnSpawn();
                }
                
            }

            foreach(Primitive p in outline)
            {
                if (newColor == BlockColor)
                {
                    p.UnSpawn();
                }
                else
                {
                    p.Spawn();
                }
            }
        }

        public static Color BlockColorToColor(BlockColor c)
        {
            return c switch
            {
                BlockColor.Blue => Color.blue,
                BlockColor.Red => Color.red,
                _ => Color.red
            };
        }



    }
    public enum BlockColor
    {
        Red,
        Blue
    }

}
