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
            Print(msg);
            string inputFile = Console.ReadLine();
            inputFile = ValidateFileName(inputFile);
            // Revert to default if not specified
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
            SectionHeader("WORKOUT SUMMARY");
            ReportWorkout(workout);
            SectionHeader("EFFORTS");
            ReportEfforts(workout.DataChannels);
        }

        private static void SectionHeader(string HeaderTitle)
        {
            Console.WriteLine();
            Console.WriteLine("***********************");
            Console.WriteLine(HeaderTitle);
            Console.WriteLine("***********************");
        }

        private static void ReportWorkout(Workout workout)
        {
            Console.WriteLine("Duration:\t\t{0}", workout.getDuration());
            Console.WriteLine("Distance:\t\t{0} miles", workout.getTotalDistance());
        }

        public static void ReportEfforts(List<DataChannel> DataChannels)
        {
            Console.WriteLine("Type\t\t1 min\t\t5 min\t\t10 min\t\t15 min\t\t20 min\t\t");
            foreach (DataChannel ThisDataChannel in DataChannels)
            {
                if (ThisDataChannel.MaxEfforts == null)
                {
                    continue;
                }
                Dictionary<int, int> efforts = ThisDataChannel.MaxEfforts;
                string message = String.Format("{0,-10}", ThisDataChannel.DataType);
                foreach (KeyValuePair<int, int> effort in efforts)
                {
                    // For small numbers of concats, this is more efficient than string builder or +=
                    message = String.Concat(message, "\t" + String.Format("{0,-8}", effort.Value));
                }
                Console.WriteLine(message);
            }
        }
    }
}