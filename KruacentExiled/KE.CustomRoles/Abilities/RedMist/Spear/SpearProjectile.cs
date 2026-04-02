using Exiled.API.Features.Toys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils;

namespace KE.CustomRoles.Abilities.RedMist.Spear
{
    [RequireComponent(typeof(Primitive))]
    public class SpearProjectile : MonoBehaviour
    {
        private Primitive primitive;


        private void Awake()
        {
            primitive = GetComponent<Primitive>();
            primitive.Collidable = false;
        }


        private void Update()
        {

            Bounds b= new()
        }





    }
}
