using Exiled.CustomItems.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core.Tokens;

namespace KE.Items.Upgrade
{
    public class UpgradeProperties
    {
        private float _chance;
        public float Chance
        {
            get { return _chance; }
        }

        private uint _newItem;
        public uint UpgradedItem
        {
            get { return _newItem; }
        }
        
        public UpgradeProperties(float chance, uint newItem)
        {
            _newItem = newItem;
            if (chance > 100) _chance = 100;
            else if(chance < 0) _chance = 0;
            else _chance = chance;
        }

    }
}
