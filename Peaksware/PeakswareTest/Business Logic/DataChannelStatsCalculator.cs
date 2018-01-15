using System.Collections.Generic;
using System.Threading;
using PeakswareTest.Models;

namespace PeakswareTest.Business_Logic
{
    public class DataChannelStatsCalculator
    {
        // Effort channels and windows could be set through dependency injection and/or user selection. An interface or configuration file could define which data channels are "efforts".
        private static readonly string[] EFFORT_CHANNELS = { "Power", "HeartRate" };
        private static readonly int[] EFFORT_WINDOW_MINUTES = { 1, 5, 10, 15, 20 };
        private static readonly int MILLISECONDS_PER_MINUTE = 60 * 1000;

        private static Dictionary<int, int> MaxEfforts;
        public List<DataChannel> DataChannels;

        // Since the class is not static, I decided to require the workout data in the constructor. This could change if threading were handled differently.
        public DataChannelStatsCalculator(List<DataChannel> inputDataChannels)
        {
            this.DataChannels = inputDataChannels;
            MaxEfforts = new Dictionary<int, int>();
        }

        public void CalculateEffortsForAllChannels()
        {
            foreach (string channelType in EFFORT_CHANNELS)
            {
                System.Predicate<DataChannel> dataTypeFilter = channel => channel.DataType.Equals(channelType);
                DataChannel thisDataChannel = DataChannels.Find(dataTypeFilter);
                if (thisDataChannel != null)
                {
                    thisDataChannel.MaxEfforts = CalculateAllEffortsForSingleChannel(thisDataChannel.Data);
                }
            }
        }

        public Dictionary<int, int> CalculateAllEffortsForSingleChannel(Dictionary<double, double> currentChannelData)
        {
            MaxEfforts = new Dictionary<int, int>();
            foreach (int effortTime in EFFORT_WINDOW_MINUTES)
            {
                Thread thread = new Thread(() => CalculateMaxEffort(effortTime, currentChannelData));
                thread.Start();
                thread.Join();
            }
            return MaxEfforts;
        }

        public void CalculateMaxEffort(int effortTimeMinutes, Dictionary<double, double> currentChannelData)
        {
            
            int ExpectedWindowMillis = effortTimeMinutes * MILLISECONDS_PER_MINUTE;
            Queue<double> timeQueue = new Queue<double>();
            Queue<double> dataValueQueue = new Queue<double>();
            double maxDataSum = 0;
            double currentDataSum = 0;
            int dataSumDatums = 0;
            foreach (KeyValuePair<double, double> dataPoint in currentChannelData)
            {
                timeQueue.Enqueue(dataPoint.Key);
                dataValueQueue.Enqueue(dataPoint.Value);
                currentDataSum += dataPoint.Value;
                double windowTime = dataPoint.Key - timeQueue.Peek();
                if (windowTime >= ExpectedWindowMillis)
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

        public Dictionary<int, int> GetMaxEfforts() => MaxEfforts;
    }
}