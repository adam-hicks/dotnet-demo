using System;
using System.Collections.Generic;

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
            if (inputFile.Equals(""))
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
                return "files/2012-05-31-11-17-12.fit";
            }
            else if (inputFile.Equals("Q") || inputFile.Equals("q"))
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

        public static void ReportEfforts(Dictionary<int, int> efforts)
        {
            foreach (KeyValuePair<int, int> effort in efforts)
            {
                Console.WriteLine("Best effort for {0} minutes during this ride was: {1} Watts!", effort.Key, effort.Value);
            }
        }
    }
}