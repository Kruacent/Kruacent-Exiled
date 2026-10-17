using Exiled.API.Enums;
using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.CustomItems
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public Dictionary<RoomType, List<Vector3>> LocalpositionInRooms { get; set; } = new Dictionary<RoomType, List<Vector3>>()
        {
            { RoomType.Lcz914,new List<Vector3>() {new Vector3(0,0.70f,-7.14f) } }
        };
    }
}
