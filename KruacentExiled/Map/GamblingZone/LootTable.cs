using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.GamblingZone
{
    internal class LootTable
    {
        private HashSet<DroppableItem> _items = new HashSet<DroppableItem>()
        {
            new(ItemType.Jailbird,5,1),
            new(ItemType.ParticleDisruptor,5,1),
            new(ItemType.Radio,15),
        };

        public LootTable()
        {
        }
        public LootTable(IEnumerable<DroppableItem> items)
        {
            _items = items.ToHashSet();
        }

        private DroppableItem ChooseRandomItem()
        {
            int totalWeight = 0;
            foreach (DroppableItem drop in _items)
            {
                if (!drop.HasReachCap())
                    totalWeight += drop.Chance;
            }

            if (totalWeight == 0)
                throw new ArgumentException("Total probability must be greater than zero.");

            int randValue = UnityEngine.Random.Range(0, totalWeight);
            int cumulativeSum = 0;

            foreach (DroppableItem drop in _items)
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
    }
}
