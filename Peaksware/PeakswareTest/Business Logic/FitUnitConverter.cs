using System;
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
            ConvertLapFields();
            ConvertSessionFields();
            ConvertDataChannelFields();
        }

        private static void ConvertLapFields()
        {
            throw new NotImplementedException();
        }

        private static void ConvertSessionFields()
        {
            throw new NotImplementedException();
        }

        private static void ConvertDataChannelFields()
        {
            throw new NotImplementedException();
        }

        private static void ConvertFieldsToImperial(Field field)
        {
            if (field.GetName().Contains("Speed"))
            {
                field.SetValue((double)Convert.ToDecimal(field.GetValue()) * SPEED_MPH_FROM_MPS);
            }
            else if (field.GetName() == "Distance")
            {
                field.SetValue((double)Convert.ToDecimal(field.GetValue()) * DISTANCE_METERS_TO_MILES);
            }
            else if (field.GetName().Contains("Altitude"))
            {
                field.SetValue((double)Convert.ToDecimal(field.GetValue()) * DISTANCE_METERS_TO_FEET);
            }
        }
    }
}