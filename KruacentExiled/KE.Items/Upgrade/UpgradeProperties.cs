using Exiled.CustomItems.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace KE.Items.Upgrade
{
    public class UpgradeProperties
    {
        private float _chance = 0;
        public float Chance
        {
            get { return _chance; }
            set
            {
                if (_chance > 100)
                    _chance = 100;
                else
                    _chance = value;
            }
        }

        private uint _newItem;

        public uint UpgradedItem
        {
            get { return _newItem; }
        }
        
        public UpgradeProperties(byte chance, uint newItem)
        {
            Chance = chance;
            _newItem = newItem;
        }

    }
}
