using NUnit.Framework;
using System.Collections.Generic;
using PeakswareTest.Business_Logic;
using PeakswareTest.Models;

namespace Tests
{
    [TestFixture]
    public class StatsCalculatorTest
    {
        private DataChannelStatsCalculator Calculator;
        private Workout ThisWorkout;
        private DataChannel ADataChannel;

        [SetUp]
        public void Setup()
        {
            ThisWorkout = new Workout();
            ADataChannel = new DataChannel
            {
                DataType = "Power",
                Data = SeedData()
            };
            ThisWorkout.DataChannels.Add(ADataChannel);
            Calculator = new DataChannelStatsCalculator(ThisWorkout.DataChannels);
        }

        [Test]
        public void TestDataIsSeeded()
        {
            CollectionAssert.IsNotEmpty(Calculator.DataChannels);
        }

        [Test]
        public void MaxEffortsStartsEmpty()
        {
            CollectionAssert.IsEmpty(Calculator.GetMaxEfforts());
        }

        [TestCase(1, 2484)]
        [TestCase(5, 2424)]
        [TestCase(10, 2349)]
        [TestCase(15, 2274)]
        [TestCase(20, 2199)]
        public void TestCalculateMaxEffort(int effortTimeMinutes, int expectedRoundedAverage)
        {
            Calculator.CalculateMaxEffort(effortTimeMinutes, ADataChannel.Data);
            ADataChannel.MaxEfforts = Calculator.GetMaxEfforts();
            int roundedResultString = ADataChannel.MaxEfforts[effortTimeMinutes];
            Assert.AreEqual(roundedResultString, expectedRoundedAverage);
        }

        [Test]
        public void TestCalculateAllEfforts()
        {
            ADataChannel.MaxEfforts = Calculator.CalculateAllEffortsForSingleChannel(ADataChannel.Data);
            int roundedResultString = ADataChannel.MaxEfforts[1];
            Assert.AreEqual(roundedResultString, 2484);

            roundedResultString = ADataChannel.MaxEfforts[5];
            Assert.AreEqual(roundedResultString, 2424);

            roundedResultString = ADataChannel.MaxEfforts[10];
            Assert.AreEqual(roundedResultString, 2349);

            roundedResultString = ADataChannel.MaxEfforts[15];
            Assert.AreEqual(roundedResultString, 2274);

            roundedResultString = ADataChannel.MaxEfforts[20];
            Assert.AreEqual(roundedResultString, 2199);

            Assert.That(ADataChannel.MaxEfforts.Keys, Does.Not.Contain(25));
        }

        [Test]
        public void TestCalculateEffortsForAllChannels()
        {
            DataChannel AnotherDataChannel = new DataChannel
            {
                DataType = "HeartRate",
                Data = SeedData()
            };
            Calculator.DataChannels.Add(AnotherDataChannel);
            Calculator.CalculateEffortsForAllChannels();

            System.Predicate<DataChannel> dataTypeFilter = channel => channel.DataType.Equals("Power");
            DataChannel ThisDataChannel = ThisWorkout.DataChannels.Find(dataTypeFilter);
            Assert.NotNull(ThisDataChannel);

            int roundedResultString = ThisDataChannel.MaxEfforts[1];
            Assert.AreEqual(roundedResultString, 2484);

            roundedResultString = ThisDataChannel.MaxEfforts[5];
            Assert.AreEqual(roundedResultString, 2424);

            roundedResultString = ThisDataChannel.MaxEfforts[10];
            Assert.AreEqual(roundedResultString, 2349);

            roundedResultString = ThisDataChannel.MaxEfforts[15];
            Assert.AreEqual(roundedResultString, 2274);

            roundedResultString = ThisDataChannel.MaxEfforts[20];
            Assert.AreEqual(roundedResultString, 2199);

            dataTypeFilter = channel => channel.DataType.Equals("HeartRate");
            ThisDataChannel = ThisWorkout.DataChannels.Find(dataTypeFilter);
            Assert.NotNull(ThisDataChannel);

            roundedResultString = ThisDataChannel.MaxEfforts[1];
            Assert.AreEqual(roundedResultString, 2484);

            roundedResultString = ThisDataChannel.MaxEfforts[5];
            Assert.AreEqual(roundedResultString, 2424);

            roundedResultString = ThisDataChannel.MaxEfforts[10];
            Assert.AreEqual(roundedResultString, 2349);

            roundedResultString = ThisDataChannel.MaxEfforts[15];
            Assert.AreEqual(roundedResultString, 2274);

            roundedResultString = ThisDataChannel.MaxEfforts[20];
            Assert.AreEqual(roundedResultString, 2199);
        }

        // generate repeatable data with predictable attributes for testing
        private Dictionary<double, double> SeedData()
        {
            int DataSize = 5000;
            Dictionary<double, double> SeedDataDict = new Dictionary<double, double>();
            for (double i = 0; i <= DataSize; i++)
            {
                if (i < DataSize / 2)
                {
                    SeedDataDict.Add(i * 1000, (int)i);
                }
                else
                {
                    SeedDataDict.Add(i * 1000, (int)(DataSize - i));
                }
            }
            return SeedDataDict;
        }
    }
}