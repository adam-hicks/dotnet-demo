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
    }
}