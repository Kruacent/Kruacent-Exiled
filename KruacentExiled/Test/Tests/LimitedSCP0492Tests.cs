using KruacentExiled.Misc.Features.SCPRebalance;
using KruacentExiled.Test.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Test.Tests
{
    [TestClass]
    public class LimitedSCP0492Tests
    {


        [TestMethod]
        public void GetMaxZombie_0_Return0()
        {
            int result = LimitedSCP0492.GetMaxZombie(0);

            Assert.IsTrue(result == 0, "should be 0");
        }

        [TestMethod]
        public void GetMaxZombie_1_Return2()
        {
            int result = LimitedSCP0492.GetMaxZombie(1);

            Assert.IsTrue(result == 2, "should be 2");
        }

        [TestMethod]
        public void GetMaxZombie_2_Return4()
        {
            int result = LimitedSCP0492.GetMaxZombie(2);

            Assert.IsTrue(result == 4, "should be 4");
        }

        [TestMethod]
        public void HasMaxZombie_0Zombie1Scp049_ReturnFalse()
        {
            bool result = LimitedSCP0492.HasMaxZombie(0,1);

            Assert.IsFalse(result, "should be false");
        }

        [TestMethod]
        public void HasMaxZombie_1Zombie1Scp049_ReturnFalse()
        {
            bool result = LimitedSCP0492.HasMaxZombie(1, 1);

            Assert.IsFalse(result, "should be false");
        }


        [TestMethod]
        public void HasMaxZombie_2Zombie1Scp049_ReturnTrue()
        {
            bool result = LimitedSCP0492.HasMaxZombie(2, 1);

            Assert.IsTrue(result, "should be true");
        }

        [TestMethod]
        public void HasMaxZombie_2Zombie2Scp049_ReturnFalse()
        {
            bool result = LimitedSCP0492.HasMaxZombie(2, 2);

            Assert.IsFalse(result, "should be false");
        }

        [TestMethod]
        public void HasMaxZombie_5Zombie2Scp049_ReturnTrue()
        {
            bool result = LimitedSCP0492.HasMaxZombie(5, 2);

            Assert.IsTrue(result, "should be true");
        }


    }
}
