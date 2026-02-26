using Exiled.API.Features;
using Exiled.API.Features.Items;
using KE.GlobalEventFramework.GEFE.API.Features;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using MEC;
using System;
using System.Collections.Generic;

namespace KE.GlobalEventFramework.Examples.GE
{
    /// <summary>
    /// Randomly item will fall from the sky
    /// </summary>
    public class ItemRain : GlobalEvent, IStart
    {
        ///<inheritdoc/>
        public override uint Id { get; set; } = 1090;
        ///<inheritdoc/>
        public override string Name { get; set; } = "ItemRain";
        ///<inheritdoc/>
        public override string Description { get; } = "Il pleut des items !!";
        ///<inheritdoc/>
        public override int WeightedChance => 1;

        public int Cooldown = 120;
        public int NbItemSpawned = 5;
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

                    if (itemType == ItemType.None) continue;

                    Item.Create(itemType).CreatePickup(Room.Random().Position);
                }
            }
        }
    }
}