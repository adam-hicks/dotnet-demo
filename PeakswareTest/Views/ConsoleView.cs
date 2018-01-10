using System;
using System.Collections.Generic;

namespace PeakswareTest.Views
{
    public class ConsoleView
    {
        public static string getFileName()
        {
            Console.WriteLine("TrainingPeaks C# Code Test");
            Console.WriteLine("Please input filename with path (<path>/<filename>.fit): ");
            string inputFile = Console.ReadLine();
            Console.WriteLine("Opening {0}", inputFile);
            return inputFile;
        }

        public static void reportEfforts(Dictionary<int, double> efforts)
        {
            foreach (KeyValuePair<int, double> effort in efforts)
            {
                Console.WriteLine("Best effor for {0} minutes during this ride was: {1} Watts!", effort.Key, effort.Value);
            }
        }
    }
}