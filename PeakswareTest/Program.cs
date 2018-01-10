using System;
using PeakswareTest.Models;

namespace PeakswareTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TrainingPeaks C# Code Test");

            if (args.Length != 1)
            {
                Console.WriteLine("Usage: PeakswareTest.exe <filename>");
                return;
            }
            WorkoutModel.importData(args[0]);
        }
    }
}
