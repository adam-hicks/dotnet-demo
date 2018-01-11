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
        public void maxEffortsStartsEmpty() 
        {
            Dictionary<int, double> maxEfforts = channel.getMaxEfforts();
            CollectionAssert.IsEmpty(maxEfforts);
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

        private void seedData() 
        {
            int dataSize = 5000;
            Dictionary<double, ushort> seedData = new Dictionary<double, ushort>();
            for(int i = 0; i <= dataSize; i++)
            {
                if (i<dataSize/2)
                {
                    seedData.Add(i, (ushort)i);
                } else {
                    seedData.Add(i, (ushort)(dataSize/2 - i));
                }
            }
            channel.setData(seedData);
        }
    }
}