using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;

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
            CollectionAssert.IsEmpty(calculator.MaxEfforts);
        }

        [TestCase(1, 2484)]
        [TestCase(5, 2424)]
        [TestCase(10, 2349)]
        [TestCase(15, 2274)]
        [TestCase(20, 2199)]
        public void TestCalculateMaxEffort(int effortTimeMinutes, int expectedRoundedAverage)
        {
            calculator.CalculateMaxEffort(effortTimeMinutes);
            channel.MaxEfforts = calculator.MaxEfforts;
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
            Dictionary<double, int> data = new Dictionary<double, int>();
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

        private class DataChannelStatsCalculator
        {
            public Dictionary<double, int> data;
            public Dictionary<int, int> MaxEfforts { get; set; }

            public DataChannelStatsCalculator(Dictionary<double, int> inputData)
            {
                data = inputData;
                MaxEfforts = new Dictionary<int, int>();
            }

            public Dictionary<int, int> CalculateAllEfforts()
            {
                int[] timesOfInterestMinutes = { 1, 5, 10, 15, 20 };
                foreach (int effortTime in timesOfInterestMinutes)
                {
                    Thread thread = new Thread(() => CalculateMaxEffort(effortTime));
                    thread.Start();
                    thread.Join();
                }
                return MaxEfforts;
            }

            internal void CalculateMaxEffort(int effortTimeMinutes)
            {
                int millisecondsPerMinute = 60 * 1000;
                int expectedWindowMillis = effortTimeMinutes * millisecondsPerMinute;
                Queue<double> timeQueue = new Queue<double>();
                Queue<int> dataValueQueue = new Queue<int>();
                double maxDataSum = 0;
                double currentDataSum = 0;
                int dataSumDatums = 0;
                foreach (KeyValuePair<double, int> powerReading in data)
                {
                    timeQueue.Enqueue(powerReading.Key);
                    dataValueQueue.Enqueue(powerReading.Value);
                    currentDataSum += powerReading.Value;
                    double windowTime = powerReading.Key - timeQueue.Peek();
                    if (windowTime >= expectedWindowMillis)
                    {
                        if (currentDataSum > maxDataSum)
                        {
                            maxDataSum = currentDataSum;
                            dataSumDatums = dataValueQueue.Count;
                        }
                        currentDataSum -= dataValueQueue.Dequeue();
                        timeQueue.Dequeue();
                    }
                }
                MaxEfforts.Add(effortTimeMinutes, (int)(maxDataSum / dataSumDatums));
            }
        }

        private class DataChannel
        {
            public string DataType { get; set; }
            public int AverageValue { get; set; }
            public int MinValue { get; set; }
            public int MaxValue { get; set; }
            public Dictionary<int, int> MaxEfforts { get; set; }
            public Dictionary<double, int> Data { get; set; }
        }
    }
}