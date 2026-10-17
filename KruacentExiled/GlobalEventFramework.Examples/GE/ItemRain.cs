using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using KruacentExiled.CustomSpawnPoint;
using KruacentExiled.CustomSpawnPoint.Spawned;
using KruacentExiled.GlobalEventFramework.GEFE.API.Features;
using KruacentExiled.GlobalEventFramework.GEFE.API.Interfaces;
using MEC;
using System;
using System.Collections.Generic;

namespace KruacentExiled.GlobalEventFramework.Examples.GE
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
        public override int WeightedChance => 1;

        public int Cooldown { get; set; } = 120;
        public int NbItemSpawned { get; set; } = 5;

        public static readonly HashSet<ItemType> BlacklistedItems = new HashSet<ItemType>()
        {
            ItemType.None,ItemType.Coal,ItemType.SpecialCoal,ItemType.Snowball,ItemType.SCP1507Tape,ItemType.Scp021J,ItemType.DebugRagdollMover,
            ItemType.KeycardCustomManagement,ItemType.KeycardCustomMetalCase,ItemType.KeycardCustomSite02,ItemType.KeycardCustomTaskForce,
        };
        public IEnumerator<float> Start()
        {
            while (!Round.IsEnded)
            {
                Log.Debug("waiting");
                Array values = Enum.GetValues(typeof(ItemType));
                yield return Timing.WaitForSeconds(Cooldown);
                
                for (int i = 0; i < NbItemSpawned; i++)
                {

                    ItemType itemType = (ItemType)values.GetValue(UnityEngine.Random.Range(0, values.Length));

                    if (CheckItemType(itemType)) continue;

                    Room randomRoom = Room.Random();

                    RoomSpawnedCustomPoint point = RoomCustomPointHandler.GetAll(randomRoom,RoomCustomPointType.Teleport).GetRandomValue();
                    Pickup.CreateAndSpawn(itemType, point.Position);
                }
            }
        }


        private bool CheckItemType(ItemType itemType)
        {
            return !BlacklistedItems.Contains(itemType);
        }
    }
}