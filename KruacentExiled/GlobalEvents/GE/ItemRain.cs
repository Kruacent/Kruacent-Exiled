using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using KruacentExiled.CustomSpawnPoint;
using KruacentExiled.CustomSpawnPoint.Spawned;
using GlobalEventFramework.API.Features;
using GlobalEventFramework.API.Interfaces;
using MEC;
using System;
using System.Collections.Generic;
using KruacentExiled.Misc.Utils;
using System.Linq;

namespace KruacentExiled.GlobalEvents.GE
{
    /// <summary>
    /// Randomly item will fall from the sky
    /// </summary>
    public class ItemRain : GlobalEvent, IAsyncStart
    {
        ///<inheritdoc/>
        public override string Name { get; set; } = "ItemRain";
        ///<inheritdoc/>
        public override string Description { get; } = "Il pleut des items !!";
        ///<inheritdoc/>
        public override int DefaultWeightedChance => 1;

        public int Cooldown { get; set; } = 120;
        public int NbItemSpawned { get; set; } = 5;

        public IEnumerator<float> Start()
        {
            while (!Round.IsEnded)
            {
                Log.Debug("waiting");
                Array values = Enum.GetValues(typeof(ItemType));
                yield return Timing.WaitForSeconds(Cooldown);
                
                for (int i = 0; i < NbItemSpawned; i++)
                {

                    ItemType itemType = BlacklistItemTypeManager.GetRandom();

                    Room randomRoom = Room.Random();

                    RoomSpawnedCustomPoint point = RoomCustomPointHandler.GetAll(randomRoom,RoomCustomPointType.Teleport).GetRandomValue();
                    Pickup.CreateAndSpawn(itemType, point.Position);
                }
            }
        }
    }
}