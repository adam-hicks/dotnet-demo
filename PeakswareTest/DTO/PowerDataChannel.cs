using System.Collections.Generic;
using System.Threading;
using System;

namespace PeakswareTest.DTO
{
    public class PowerDataChannel : IDataChannel
    {
        public Dictionary<double, ushort> data;
        public Dictionary<int, double> maxEfforts = new Dictionary<int, double>();
        public void calculateAllEfforts()
        {
            int[] timesOfInterestMinutes = { 1, 5, 10, 15, 20 };
            foreach (int effortTime in timesOfInterestMinutes)
            {
                Thread thread = new Thread(() => calculateMaxEffort(effortTime));
                thread.Start();
                thread.Join();
            }
        }

        public void calculateMaxEffort(int effortTimeMinutes)
        {
            int expectedWindowMillis = effortTimeMinutes * 60 * 1000;
            Queue<double> timeQueue = new Queue<double>();
            Queue<ushort> powerQueue = new Queue<ushort>();
            double maxPowerSum = 0;
            double powerSum = 0;
            int powerSumDatums = 0;
            foreach (KeyValuePair<double, ushort> powerReading in data)
            {
                timeQueue.Enqueue(powerReading.Key);
                powerQueue.Enqueue(powerReading.Value);
                powerSum += powerReading.Value;

                double windowTime = powerReading.Key - timeQueue.Peek();
                if (windowTime >= expectedWindowMillis)
                {
                    if (powerSum > maxPowerSum)
                    {
                        maxPowerSum = powerSum;
                        powerSumDatums = powerQueue.Count;
                    }
                    powerSum -= powerQueue.Dequeue();
                    timeQueue.Dequeue();
                }
            }
            maxEfforts.Add(effortTimeMinutes, maxPowerSum / powerSumDatums);
        }

        public Dictionary<double, ushort> getData()
        {
            return data;
        }

        public Dictionary<int, double> getMaxEfforts()
        {
            return maxEfforts;
        }

        public void setData(Dictionary<double, ushort> rawData)
        {
            data = rawData;
        }
    }
}