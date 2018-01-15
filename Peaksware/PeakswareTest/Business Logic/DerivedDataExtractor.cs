using System;
using System.Collections.Generic;
using PeakswareTest.Models;

namespace PeakswareTest.Business_Logic
{
    public static class DerivedDataExtractor
    {
        private static Workout ThisWorkout;
        internal static void ExtractDataChannels(Workout InputWorkout)
        {
            ThisWorkout = InputWorkout;
            List<Record> Records = ThisWorkout.Records;
            foreach (Record ThisRecord in Records)
            {
                foreach (KeyValuePair<string, object> Metric in ThisRecord.RecordMetrics)
                {
                    SortMetricDataIntoDataFields(ThisRecord, Metric);
                }
            }
        }

        private static void SortMetricDataIntoDataFields(Record ThisRecord, KeyValuePair<string, object> Metric)
        {
            DataChannel DataChannel = FindOrCreateDataChannel(Metric.Key);
            double timeOffset = GetTimeOffset(ThisRecord.Timestamp);
            DataChannel.Data.Add(timeOffset, (double)Convert.ToDecimal(Metric.Value));
        }

        private static DataChannel FindOrCreateDataChannel(string InputDataType)
        {
            System.Predicate<DataChannel> DataTypeFilter = channel => channel.DataType.Equals(InputDataType);
            DataChannel DataChannel = ThisWorkout.DataChannels.Find(DataTypeFilter);
            if (DataChannel == null)
            {
                DataChannel = new DataChannel
                {
                    DataType = InputDataType,
                    Data = new Dictionary<double, double>()
                };
                ThisWorkout.DataChannels.Add(DataChannel);
            }

            return DataChannel;
        }

        static double GetTimeOffset(System.DateTime date)
        {
            return date.Subtract((System.DateTime)ThisWorkout.Timestamp).TotalMilliseconds;
        }
    }
}
