using System;
using System.Collections.Generic;
using Dynastream.Fit;
using PeakswareTest.Models;

namespace PeakswareTest.Business_Logic
{
    internal static class FitUnitConverter
    {
        private static readonly double SPEED_MPH_FROM_MPS = 2.24;
        private static readonly double DISTANCE_METERS_TO_MILES = .0006214;
        private static readonly double DISTANCE_METERS_TO_FEET = 3.281;

        public static void ConvertWorkoutToImperial(Workout workout)
        {
            ConvertLapFields(workout.Laps);
            ConvertSessionFields(workout.Session);
            ConvertDataChannelFields(workout.DataChannels);
        }

        private static void ConvertLapFields(List<Lap> Laps)
        {
            foreach (Lap Lap in Laps)
            {
                foreach (KeyValuePair<string, object> Metric in Lap.LapMetrics)
                {
                    ConvertFieldsToImperial(Metric);
                }
            }
        }

        private static void ConvertSessionFields(Session ThisSession)
        {
            foreach (KeyValuePair<string, object> Metric in ThisSession.SessionMetrics)
            {
                ConvertFieldsToImperial(Metric);
            }
        }

        private static void ConvertDataChannelFields(List<DataChannel> DataChannels)
        {
            foreach (DataChannel ThisDataChannel in DataChannels)
            {
                foreach (KeyValuePair<double, double> datum in ThisDataChannel.Data)
                {
                    
                }
            }
        }

        private static KeyValuePair<string, object> ConvertFieldsToImperial(KeyValuePair<string, object> Metric)
        {
            double DoubleValue = 0;
            try
            {
                DoubleValue = (double)Convert.ToDecimal(Metric.Value);
            }
            catch (InvalidCastException)
            {
                return Metric;
            }
            KeyValuePair<string, object> NewMetric;
            if (Metric.Key.Contains("Speed"))
            {
                NewMetric = new KeyValuePair<string, object>(Metric.Key, DoubleValue * SPEED_MPH_FROM_MPS);
            }
            else if (Metric.Key == "Distance")
            {
                NewMetric = new KeyValuePair<string, object>(Metric.Key, DoubleValue * DISTANCE_METERS_TO_MILES);
            }
            else if (Metric.Key.Contains("Altitude"))
            {
                NewMetric = new KeyValuePair<string, object>(Metric.Key, DoubleValue * DISTANCE_METERS_TO_FEET);
            }
            return NewMetric;
        }
    }
}