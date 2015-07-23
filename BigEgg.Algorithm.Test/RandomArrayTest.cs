using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BigEgg.UnitTesting;

namespace BigEgg.Algorithm.Test
{
    [TestClass]
    public class RandomArrayTest
    {
        [TestMethod]
        public void GeneralTest()
        {
            var randomArray = new RandomArray(5);
            var num = randomArray.Next();
            Assert.IsTrue(num <= 5 && num >= 0);

            AssertHelper.ExpectedException<ArgumentOutOfRangeException>(() => new RandomArray(-1));
            AssertHelper.ExpectedException<ArgumentOutOfRangeException>(() => new RandomArray(0));
        }

        [TestMethod]
        public void NextTest()
        {
            var randomArray = new RandomArray(5);

            var num = randomArray.Next();
            Assert.IsTrue(num <= 5 && num >= 0);

            num = randomArray.Next();
            Assert.IsTrue(num <= 5 && num >= 0);

            num = randomArray.Next();
            Assert.IsTrue(num <= 5 && num >= 0);

            num = randomArray.Next();
            Assert.IsTrue(num <= 5 && num >= 0);

            num = randomArray.Next();
            Assert.IsTrue(num <= 5 && num >= 0);

            num = randomArray.Next();
            Assert.IsTrue(num <= 5 && num >= 0);
        }
    }
}
