using Exiled.API.Interfaces;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Map.Others.CustomDoors
{
    public class VanillaDoor : KEDoor, IWrapper<Door>
    {
        public Door Base { get; }

        public override string NameTag => Base.NameTag;
        public VanillaDoor(Door door) : base()
        {
            Base = door;

            
        }
    }
}
