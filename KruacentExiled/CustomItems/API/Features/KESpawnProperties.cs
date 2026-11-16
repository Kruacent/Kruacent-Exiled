using Exiled.API.Features.Spawn;
using System;
using System.Collections.Generic;

namespace KruacentExiled.CustomItems.API.Features
{
    public class KESpawnProperties : SpawnProperties
    {
        [Obsolete("DynamicSpawnPoints is deprecated for custom items, please use RoomSpawnPoints")]
        public new List<DynamicSpawnPoint> DynamicSpawnPoints
        {
            get => base.DynamicSpawnPoints;
            set => base.DynamicSpawnPoints = value;
        }
    }
}
