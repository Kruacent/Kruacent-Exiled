using Exiled.CustomItems.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using YamlDotNet.Core.Tokens;

namespace KE.Items.API.Core.Upgrade
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
            _chance = Mathf.Clamp(chance, 0,100);
        }

    }
}
