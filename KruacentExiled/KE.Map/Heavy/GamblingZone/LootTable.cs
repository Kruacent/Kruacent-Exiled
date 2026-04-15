using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Pools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.Heavy.GamblingZone
{
    public class LootTable
    {
        public IReadOnlyCollection<DroppableItem> Items { get; }

        /// <summary>
        /// Create a <see cref="LootTable"/> with some testing items
        /// </summary>
        public LootTable()
        {
            Items = new HashSet<DroppableItem>()
            {
                new DroppableItem(ItemType.Jailbird,5,1),
                new DroppableItem(ItemType.ParticleDisruptor,5,1),
                new DroppableItem(ItemType.Radio,15),
            };

        }

        /// <summary>
        /// Create a <see cref="LootTable"/> with customizable <see cref="DroppableItem"/>s
        /// </summary>
        public LootTable(IEnumerable<DroppableItem> items)
        {
            Items = items.ToHashSet();
        }

        private DroppableItem ChooseRandomItem()
        {
            int totalWeight = 0;
            foreach (DroppableItem drop in Items)
            {
                if (!drop.HasReachCap())
                    totalWeight += drop.Chance;
            }

            if (totalWeight == 0)
                throw new ArgumentException("Total probability must be greater than zero.");

            int randValue = UnityEngine.Random.Range(0, totalWeight);
            int cumulativeSum = 0;

            foreach (DroppableItem drop in Items)
            {
                if (!drop.HasReachCap())
                    cumulativeSum += drop.Chance;
                if (randValue < cumulativeSum)
                    return drop;
            }
            return null;

        }

        public Item GetRandomItem()
        {
            DroppableItem item = ChooseRandomItem();
            Log.Debug("random item =" + item);
            return item.GetItem();
        }

        ///<inheritdoc/>
        public override string ToString()
        {
            StringBuilder builder = StringBuilderPool.Pool.Get();


            foreach (DroppableItem item in Items)
            {
                builder.AppendLine(item.ToString());
            }       
            return StringBuilderPool.Pool.ToStringReturn(builder); 
        }
    }
}
