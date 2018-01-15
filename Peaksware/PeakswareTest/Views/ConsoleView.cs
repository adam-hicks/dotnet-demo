using System;
using System.Collections.Generic;
using PeakswareTest.Models;

namespace PeakswareTest.Views
{
    public static class ConsoleView
    {
        public static void Welcome()
        {
            Console.WriteLine("TrainingPeaks C# Code Test");
        }
        public static string GetFileName(string msg)
        {
            Console.WriteLine(msg);
            string inputFile = Console.ReadLine();
            inputFile = ValidateFileName(inputFile);
            if (inputFile != null && inputFile.Equals(""))
            {
                inputFile = "files/2012-05-31-11-17-12.fit";
            }
            return inputFile;
        }

        // Quick "front end" validation for efficiency
        private static string ValidateFileName(string inputFile)
        {
            if (inputFile.Equals(""))
            {
                Console.WriteLine("Using files/2012-05-31-11-17-12.fit...");
                return "files/2012-05-31-11-17-12.fit";
            }
            else if (inputFile.ToUpperInvariant().Equals("Q"))
            {
                return inputFile;
            }
            else if (!inputFile.EndsWith(".fit", StringComparison.Ordinal))
            {
                return null;
            }
            return inputFile;
        }

        internal static void Print(string msg)
        {
            Console.WriteLine(msg);
        }

        public static void PrintWorkoutSummary(Workout workout)
        {
            Console.WriteLine("***********************");
            Console.WriteLine("WORKOUT SUMMARY");
            Console.WriteLine("***********************");
            double seconds = (double)Convert.ToDecimal(workout.Session.SessionMetrics["TotalElapsedTime"]);
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            string str = time.ToString(@"hh\:mm\:ss");
            Console.WriteLine("Time:\t\t{0}", str);
            Console.WriteLine("Distance:\t{0}", workout.Session.SessionMetrics["TotalDistance"]);
        }

        public static void ReportEfforts(DataChannel dataChannel)
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n" + dataChannel.DataType + "\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Dictionary<int, int> efforts = dataChannel.MaxEfforts;
            string msg = GetMsg(dataChannel.DataType);
            foreach (KeyValuePair<int, int> effort in efforts)
            {
                Console.WriteLine(msg, effort.Key, effort.Value);
            }
        }

        private static string GetMsg(string dataType)
        {
            switch (dataType)
            {
                case "Power":
                    return "Best effort for {0} minutes during this ride was: {1} Watts!";
                case "HeartRate":
                    return "Max {0} minute heart rate for this ride was {1} BPM.";
                default:
                    return "Data channel name " + dataType + " not valid.";
            }
        }
    }
}