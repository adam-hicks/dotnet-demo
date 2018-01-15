using System;
using System.Collections.Generic;
using Dynastream.Fit;
using PeakswareTest.Models;

namespace PeakswareTest.Business_Logic
{
    public static class FitUnitConverter
    {
        private static readonly double SPEED_MPS_FROM_MPH = 2.24;
        private static readonly double DISTANCE_METERS_TO_MILES = .0006214;
        private static readonly double DISTANCE_METERS_TO_FEET = 3.281;

        public static void ConvertWorkoutToImperial(Workout workout)
        {
            ConvertSessionFields(workout.Session);
            ConvertLapFields(workout.Laps);
            ConvertRecordFields(workout.Records);
        }

        private static void ConvertSessionFields(Session ThisSession)
        {
            List<string> Keys = new List<string>(ThisSession.SessionMetrics.Keys);
            foreach (string Key in Keys)
            {
                double DoubleValue = 0;
                try
                {
                    DoubleValue = (double)Convert.ToDecimal(ThisSession.SessionMetrics[Key]);
                }
                catch (InvalidCastException)
                {
                    continue;
                }
                double newValue = ConvertFieldsToImperial(new KeyValuePair<string, double>(Key, DoubleValue));
                ThisSession.SessionMetrics[Key] = newValue;
            }
        }

        private static void ConvertLapFields(List<Lap> Laps)
        {
            foreach (Lap Lap in Laps)
            {
                List<string> Keys = new List<string>(Lap.LapMetrics.Keys);
                foreach (string Key in Keys)
                {
                    double DoubleValue = 0;
                    try
                    {
                        DoubleValue = (double)Convert.ToDecimal(Lap.LapMetrics[Key]);
                    }
                    catch (InvalidCastException)
                    {
                        continue;
                    }
                    double newValue = ConvertFieldsToImperial(new KeyValuePair<string, double>(Key, DoubleValue));
                    Lap.LapMetrics[Key] = newValue;
                }
            }
        }

        private static void ConvertRecordFields(List<Record> Records)
        {
            foreach (Record ThisRecord in Records)
            {
                List<string> Keys = new List<string>(ThisRecord.RecordMetrics.Keys);
                foreach (string Key in Keys)
                {
                    double DoubleValue = 0;
                    try
                    {
                        DoubleValue = (double)Convert.ToDecimal(ThisRecord.RecordMetrics[Key]);
                    }
                    catch (InvalidCastException)
                    {
                        continue;
                    }
                    double newValue = ConvertFieldsToImperial(new KeyValuePair<string, double>(Key, DoubleValue));
                    ThisRecord.RecordMetrics[Key] = newValue;
                }
            }
        }

        private static double ConvertFieldsToImperial(KeyValuePair<string, double> Metric)
        {
            if (Metric.Key.ToLowerInvariant().Contains("speed"))
            {
                return Metric.Value * SPEED_MPS_FROM_MPH;
            }
            else if (Metric.Key.ToLowerInvariant().Contains("distance"))
            {
                return Metric.Value * DISTANCE_METERS_TO_MILES;
            }
            else if (Metric.Key.ToLowerInvariant().Contains("altitude"))
            {
                return Metric.Value * DISTANCE_METERS_TO_FEET;
            }
            return Metric.Value;
        }
    }
}