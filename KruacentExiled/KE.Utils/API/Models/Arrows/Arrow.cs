using Exiled.API.Features.Toys;
using InventorySystem.Items.Keycards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Utils.API.Models.Arrows
{
    internal abstract class Arrow
    {
        internal static HashSet<Arrow> List = new();
        internal abstract Vector3 Offset { get; }
        internal abstract Vector3 Rotation { get; }
        internal abstract Color Color { get; }

        protected readonly Vector3 scale = new Vector3(1f, .1f, .1f);

        private Primitive _primitive;

        internal Primitive Primitive
        {
            get { return _primitive; }
        }
        internal Arrow()
        {
            List.Add(this);
            _primitive = Primitive.Create(PrimitiveType.Cube,  null, Rotation, scale);
            _primitive.Color = Color;
        }

        internal void Move(Vector3 newPos)
        {
            _primitive.Position = newPos + Offset;
        }

        internal static bool IsPrimitiveArrow(Primitive p)
        {
            Arrow a =GetArrow(p);
            return a != null;
        }

        internal static Arrow GetArrow(Primitive p)
        {

            foreach(Arrow a in List)
            {
                if (p == a.Primitive)
                    return a;
            }
            return null;

        }
    }
}
