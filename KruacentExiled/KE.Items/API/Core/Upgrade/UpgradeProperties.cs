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
        public float Chance { get; }

        public uint UpgradedItem { get; }

        public UpgradeProperties(float chance, uint newItem)
        {
            UpgradedItem = newItem;
            Chance = Mathf.Clamp(chance, 0,100);
        }

    }
}
