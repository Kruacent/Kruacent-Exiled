using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Toys;
using KE.Utils.Extensions;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Utils.Tests
{
    public static class PrimitiveTest
    {

        public static void TestSetFakePrimitive(Player pl)
        {
            Primitive p = Primitive.Create(pl.Position+UnityEngine.Vector3.forward, null, null, true, UnityEngine.Color.blue);
            p.SetFakePrimitive(pl);

            p = Primitive.Create(pl.Position+ UnityEngine.Vector3.back,null,null,true,UnityEngine.Color.red);
            p.SetFakePrimitive(null);
        }
    }
}
