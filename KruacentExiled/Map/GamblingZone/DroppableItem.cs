using Items = Exiled.API.Features.Items.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.CustomItems.API.Features;
using UnityEngine;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;

namespace Map.GamblingZone
{
    internal class DroppableItem
    {
        private ItemType _item;
        internal ItemType Item { get { return _item; } }
        private int _chance;
        internal int Chance
        {
            get { return _chance; }
            set
            {
                if (_chance > 100) _chance = 100;
                else if (_chance < 0) _chance = 0;
                else _chance = value;
            }
        }

        private int _itemCap;
        internal int ItemCap
        {
            get { return _itemCap; }
            set { _itemCap = value; } 
        }

        private int _currentCap = 0;
        internal int CurrentCap
        {
            get { return _currentCap; }
            set { _currentCap = value; }
        }




        internal DroppableItem(ItemType item, int chance,int itemCap = -1)
        {
            _item = item;
            Chance = chance;
            ItemCap = itemCap;
        }

        internal Items GetItem()
        {
            if (HasReachCap()) throw new Exception("Cap reached");
            CurrentCap++;
            return Items.Create(Item);
        }

        internal bool HasReachCap()
        {
            return CurrentCap >= ItemCap;
        }
    }
}
