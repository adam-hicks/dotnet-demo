using NUnit.Framework;
using System.Collections.Generic;
using PeakswareTest.Business_Logic;
using PeakswareTest.Models;

namespace Tests
{
    [TestFixture]
    public class StatsCalculatorTest
    {
        private DataChannelStatsCalculator calculator;
        private DataChannel channel;

        [SetUp]
        public void Setup()
        {
            channel = new DataChannel();
            SeedData();
            calculator = new DataChannelStatsCalculator(channel.Data);
        }

        [Test]
        public void TestDataIsSeeded()
        {
            CollectionAssert.IsNotEmpty(calculator.data);
        }

        [Test]
        public void MaxEffortsStartsEmpty()
        {
            CollectionAssert.IsEmpty(calculator.GetMaxEfforts());
        }

        [TestCase(1, 2484)]
        [TestCase(5, 2424)]
        [TestCase(10, 2349)]
        [TestCase(15, 2274)]
        [TestCase(20, 2199)]
        public void TestCalculateMaxEffort(int effortTimeMinutes, int expectedRoundedAverage)
        {
            calculator.CalculateMaxEffort(effortTimeMinutes);
            channel.MaxEfforts = calculator.GetMaxEfforts();
            int roundedResultString = channel.MaxEfforts[effortTimeMinutes];
            Assert.AreEqual(roundedResultString, expectedRoundedAverage);
        }

        [Test]
        public void TestCalculateAllEfforts()
        {
            channel.MaxEfforts = calculator.CalculateAllEfforts();
            int roundedResultString = channel.MaxEfforts[1];
            Assert.AreEqual(roundedResultString, 2484);

            roundedResultString = channel.MaxEfforts[5];
            Assert.AreEqual(roundedResultString, 2424);

            roundedResultString = channel.MaxEfforts[10];
            Assert.AreEqual(roundedResultString, 2349);

            roundedResultString = channel.MaxEfforts[15];
            Assert.AreEqual(roundedResultString, 2274);

            roundedResultString = channel.MaxEfforts[20];
            Assert.AreEqual(roundedResultString, 2199);

            Assert.That(channel.MaxEfforts.Keys, Does.Not.Contain(25));
        }

        // generate repeatable data with predictable attributes for testing
        private void SeedData()
        {
            int dataSize = 5000;
            Dictionary<double, double> data = new Dictionary<double, double>();
            for (double i = 0; i <= dataSize; i++)
            {
                if (i < dataSize / 2)
                {
                    data.Add(i * 1000, (int)i);
                }
                else
                {
                    data.Add(i * 1000, (int)(dataSize - i));
                }
            }
            channel.Data = data;
        }
    }
}