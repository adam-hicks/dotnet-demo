using PeakswareTest.Data;
using PeakswareTest.Views;
using PeakswareTest.Models;
using PeakswareTest.Business_Logic;
using System;

namespace PeakswareTest.Controllers

{
    public class WorkoutAnalyzerController
    {
        public void Run()
        {
            ConsoleView.Welcome();
            Workout workout = RetrieveData();
            if (workout != null)
            {
                FitUnitConverter.ConvertWorkoutToImperial(workout);
                DerivedDataExtractor.ExtractDataChannels(workout);
                new DataChannelStatsCalculator(workout.DataChannels).CalculateEffortsForAllChannels();
                ConsoleView.PrintWorkoutSummary(workout);
            }
            else { ConsoleView.Print("Exiting..."); }

        }

        private Workout RetrieveData()
        {
            Workout workout = null;
            string inputFile;
            string msg = "Please input filename with path or Q to exit (leave blank for default file): ";
            do
            {
                inputFile = ConsoleView.GetFileName(msg);
                if (inputFile != null)
                {
                    if (inputFile.ToUpperInvariant().Equals("Q"))
                    {
                        return null;
                    }
                    workout = FitImportFull.ImportData(inputFile);
                }
                msg = "Specified file could not be read. Please try again: ";
            } while (workout == null);
            return workout;
        }

        private static void AnalyzeWorkout(Workout workout)
        {
            
        }
    }
}