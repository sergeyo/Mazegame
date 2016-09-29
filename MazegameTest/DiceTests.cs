using Mazegame.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazegameTest
{
    [TestClass]
    public class DiceTests
    {
        [TestMethod]
        public void DiceGetRollResult_When1d7CalledMuchTimes_ShouldReturnUniformDistribution()
        {
            var dice = new Dice(1, 7);

            const int rollsCount = 10000;

            var valuesDistribution = Enumerable.Range(1, rollsCount)
                                               .Select(r => dice.GetRollResult())
                                               .GroupBy(r => r)
                                               .ToDictionary(v => v.Key, v => v.Count());

            Assert.AreEqual(1, valuesDistribution.Keys.Min());
            Assert.AreEqual(7, valuesDistribution.Keys.Max());

            foreach (var value in valuesDistribution.Keys)
            {
                var valuesCount = valuesDistribution[value];

                int expectedCount = rollsCount / 7;

                Assert.IsTrue(Math.Abs(valuesCount - expectedCount) < rollsCount / 10);
            }
        }

        [TestMethod]
        public void DiceGetRollResult_When2d6CalledMuchTimes_ShouldReturnValuesInRange()
        {
            var dice = new Dice(2, 6);

            const int rollsCount = 10000;

            var valuesDistribution = Enumerable.Range(1, rollsCount)
                                               .Select(r => dice.GetRollResult())
                                               .GroupBy(r => r)
                                               .ToDictionary(v => v.Key, v => v.Count());

            Assert.AreEqual(2, valuesDistribution.Keys.Min());
            Assert.AreEqual(12, valuesDistribution.Keys.Max());

            Assert.IsTrue(valuesDistribution[2] * 3 < valuesDistribution[6]);
            Assert.IsTrue(valuesDistribution[12] * 3 < valuesDistribution[6]);
        }
    }
}
