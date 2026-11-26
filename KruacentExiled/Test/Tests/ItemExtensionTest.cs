using Exiled.API.Features;
using KruacentExiled.Extensions;
using KruacentExiled.Test.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace KruacentExiled.Test.Tests
{
    [TestClass]
    public class ItemExtensionTest
    {

        [TestCase(ItemType.Adrenaline)]
        [TestCase(ItemType.Coin)]
        [TestCase(ItemType.Radio)]
        [TestCase(ItemType.GrenadeFlash)]
        public void IsViolent_NonViolentItem_ReturnFalse(ItemType value)
        {
            bool result = ItemExtensions.IsViolent(value);

            Assert.IsFalse(result, "should be false");
        }


        [TestCase(ItemType.GunLogicer)]
        [TestCase(ItemType.Jailbird)]
        [TestCase(ItemType.SCP018)]
        [TestCase(ItemType.GrenadeHE)]
        public void IsViolent_ViolentItem_ReturnTrue(ItemType value)
        {
            bool result = ItemExtensions.IsViolent(value);

            Assert.IsTrue(result, "should be true");
        }
    }
}
