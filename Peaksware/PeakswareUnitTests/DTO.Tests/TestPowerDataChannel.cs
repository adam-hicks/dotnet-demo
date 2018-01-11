using NUnit.Framework;
using PeakswareTest.DTO;
using System.Collections.Generic;

namespace Tests
{
    [TestFixture]
    public class TestPowerDataChannel
    {
        PowerDataChannel channel;

        [SetUp]
        public void Setup()
        {
            channel = new PowerDataChannel();
            seedData();
        }

        [Test]
        public void testDataIsSeeded() 
        {
            Dictionary<double, ushort> data = channel.getData();
            CollectionAssert.IsNotEmpty(data);
        }

        [Test]
        public void maxEffortsStartsEmpty() 
        {
            Dictionary<int, int> maxEfforts = channel.getMaxEfforts();
            CollectionAssert.IsEmpty(maxEfforts);
        }

        [TestCase(1, 2484)]
        [TestCase(5, 2424)]
        [TestCase(10, 2349)]
        [TestCase(15, 2274)]
        [TestCase(20, 2199)]
        public void testCalculateMaxEffort(int effortTimeMinutes, int expectedRoundedAverage)
        {
            channel.calculateMaxEffort(effortTimeMinutes);
            int roundedResultString = channel.maxEfforts[effortTimeMinutes];
            Assert.AreEqual(roundedResultString, expectedRoundedAverage);
        }

        [Test]
        public void testCalculateAllEfforts()
        {
            channel.calculateAllEfforts();
            int roundedResultString = channel.maxEfforts[1];
            Assert.AreEqual(roundedResultString, 2484);
            
            roundedResultString = channel.maxEfforts[5];
            Assert.AreEqual(roundedResultString, 2424);
            
            roundedResultString = channel.maxEfforts[10];
            Assert.AreEqual(roundedResultString, 2349);
            
            roundedResultString = channel.maxEfforts[15];
            Assert.AreEqual(roundedResultString, 2274);
            
            roundedResultString = channel.maxEfforts[20];
            Assert.AreEqual(roundedResultString, 2199);
            
            Assert.That(channel.maxEfforts.Keys, Does.Not.Contain(25));
        }

        // generate repeatable data with predictable attributes for testing
        private void seedData() 
        {
            int dataSize = 5000;
            Dictionary<double, ushort> seedData = new Dictionary<double, ushort>();
            for(double i = 0; i <= dataSize; i++)
            {
                if (i<dataSize/2)
                {
                    seedData.Add(i*1000, (ushort)i);
                } else {
                    seedData.Add(i*1000, (ushort)(dataSize - i));
                }
            }
            channel.setData(seedData);
        }

       /* [TestCase(2, true)]
        [TestCase(5, false)]
        [TestCase(8, false)]
        public void Test1(int value, bool expectedResult)
        {
            PowerDataChannel channel = new PowerDataChannel();
            bool result = channel.testMethod(value);
            Assert.AreEqual(result, expectedResult);
        }*/
    }
}