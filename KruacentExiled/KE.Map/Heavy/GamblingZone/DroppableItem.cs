using Items = Exiled.API.Features.Items.Item;
using System;
using UnityEngine;
using Exiled.API.Features.Items;

namespace KE.Map.Heavy.GamblingZone
{
    public class DroppableItem : IEquatable<DroppableItem>
    {
        private readonly ItemType _item;
        internal ItemType Item { get { return _item; } }
        private int _chance;
        internal int Chance
        {
            get { return _chance; }
            set
            {
                _chance = Mathf.Clamp(value, 0, 100);
            }
        }

        private int _itemCap = -1;
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




        internal DroppableItem(ItemType item, int chance, int itemCap = -1)
        {

            _item = item;
            Chance = chance;
            ItemCap = itemCap;
        }
        public static implicit operator DroppableItem(ItemType d) => new(d, 1, -1);
        public bool Equals(DroppableItem other)
        {
            return other.Item == Item && other.Chance == Chance && other.ItemCap == ItemCap && CurrentCap == other.CurrentCap;
        }

        public Items GetItem()
        {
            if (HasReachCap()) throw new Exception("Cap reached");
            CurrentCap++;
            return Items.Create(Item);
        }

        public bool HasReachCap()
        {
            return CurrentCap >= ItemCap && ItemCap != -1;
        }

        public override string ToString()
        {
            return $"{Item.ToString()} : ({Chance}%) {CurrentCap}/{ItemCap}";
        }
    }
}
