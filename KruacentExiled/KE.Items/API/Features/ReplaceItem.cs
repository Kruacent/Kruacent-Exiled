using Exiled.API.Enums;
using Exiled.API.Features.Spawn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Items.API.Features
{
    public class ReplaceItem
    {
        public float Chance { get; set; }
        public RoomType Room { get; set; }
        public uint LimitPerRoom { get; set; }

        public ItemType ItemToReplace { get; set; }
    }
}
