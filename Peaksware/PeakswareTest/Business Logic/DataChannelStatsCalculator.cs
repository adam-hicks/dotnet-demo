using System.Collections.Generic;
using System.Threading;
using System;

namespace PeakswareTest.Business_Logic
{
    public class DataChannelStatsCalculator
    {
        private static Dictionary<double, int> data;
        private static Dictionary<int, int> maxEfforts;

        public DataChannelStatsCalculator(Dictionary<double, int> inputData)
        {
            data = inputData;
            maxEfforts = new Dictionary<int, int>();
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
            return maxEfforts;
        }

        private static void CalculateMaxEffort(int effortTimeMinutes)
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
            maxEfforts.Add(effortTimeMinutes, (int)(maxDataSum / dataSumDatums));
        }
    }
}