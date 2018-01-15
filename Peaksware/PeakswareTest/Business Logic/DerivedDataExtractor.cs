using System;
using System.Collections.Generic;
using PeakswareTest.Models;

namespace PeakswareTest.Business_Logic
{
    public static class DerivedDataExtractor
    {
        private static Workout ThisWorkout;
        internal static void ExtractDataChannels(Workout inputWorkout)
        {
            ThisWorkout = inputWorkout;
            List<Record> records = ThisWorkout.Records;
            foreach (Record thisRecord in records)
            {
                foreach (KeyValuePair<string, object> Metric in thisRecord.RecordMetrics)
                {
                    SortMetricDataIntoDataFields(thisRecord, Metric);
                }
            }
        }

        private static void SortMetricDataIntoDataFields(Record thisRecord, KeyValuePair<string, object> metric)
        {
            DataChannel dataChannel = FindOrCreateDataChannel(metric.Key);
            double timeOffset = GetTimeOffset(thisRecord.Timestamp);
            dataChannel.Data.Add(timeOffset, (double)Convert.ToDecimal(metric.Value));
        }

        // Allows for flexibility in collecting any channels provided by a device without knowing what they are ahead of time.
        private static DataChannel FindOrCreateDataChannel(string inputDataType)
        {
            System.Predicate<DataChannel> dataTypeFilter = channel => channel.DataType.Equals(inputDataType);
            DataChannel dataChannel = ThisWorkout.DataChannels.Find(dataTypeFilter);
            if (dataChannel == null)
            {
                dataChannel = new DataChannel
                {
                    DataType = inputDataType,
                    Data = new Dictionary<double, double>()
                };
                ThisWorkout.DataChannels.Add(dataChannel);
            }

            return dataChannel;
        }

        static double GetTimeOffset(System.DateTime date)
        {
            return date.Subtract((System.DateTime)ThisWorkout.Timestamp).TotalMilliseconds;
        }
    }
}
