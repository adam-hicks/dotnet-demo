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

        // Using an interface for Session, Lap, and Record could have collapsed the following 3 methods to 1
        private static void ConvertSessionFields(Session thisSession)
        {
            List<string> Keys = new List<string>(thisSession.SessionMetrics.Keys);
            foreach (string Key in Keys)
            {
                double doubleValue = 0;
                try
                {
                    doubleValue = (double)Convert.ToDecimal(thisSession.SessionMetrics[Key]);
                }
                catch (InvalidCastException)
                {
                    continue;
                }
                double newValue = ConvertFieldsToImperial(new KeyValuePair<string, double>(Key, doubleValue));
                thisSession.SessionMetrics[Key] = newValue;
            }
        }

        private static void ConvertLapFields(List<Lap> laps)
        {
            foreach (Lap Lap in laps)
            {
                List<string> Keys = new List<string>(Lap.LapMetrics.Keys);
                foreach (string Key in Keys)
                {
                    double doubleValue = 0;
                    try
                    {
                        doubleValue = (double)Convert.ToDecimal(Lap.LapMetrics[Key]);
                    }
                    catch (InvalidCastException)
                    {
                        continue;
                    }
                    double newValue = ConvertFieldsToImperial(new KeyValuePair<string, double>(Key, doubleValue));
                    Lap.LapMetrics[Key] = newValue;
                }
            }
        }

        private static void ConvertRecordFields(List<Record> records)
        {
            foreach (Record ThisRecord in records)
            {
                List<string> Keys = new List<string>(ThisRecord.RecordMetrics.Keys);
                foreach (string Key in Keys)
                {
                    double doubleValue = 0;
                    try
                    {
                        doubleValue = (double)Convert.ToDecimal(ThisRecord.RecordMetrics[Key]);
                    }
                    catch (InvalidCastException)
                    {
                        continue;
                    }
                    double newValue = ConvertFieldsToImperial(new KeyValuePair<string, double>(Key, doubleValue));
                    ThisRecord.RecordMetrics[Key] = newValue;
                }
            }
        }

        // Invariant may be the wrong choice here depending on how .fit handles channel names in different regions.
        private static double ConvertFieldsToImperial(KeyValuePair<string, double> metric)
        {
            if (metric.Key.ToLowerInvariant().Contains("speed"))
            {
                return metric.Value * SPEED_MPS_FROM_MPH;
            }
            else if (metric.Key.ToLowerInvariant().Contains("distance"))
            {
                return metric.Value * DISTANCE_METERS_TO_MILES;
            }
            else if (metric.Key.ToLowerInvariant().Contains("altitude"))
            {
                return metric.Value * DISTANCE_METERS_TO_FEET;
            }
            return metric.Value;
        }
    }
}