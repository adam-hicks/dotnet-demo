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
                DerivedDataExtractor.ExtractDataChannels(workout);
                FitUnitConverter.ConvertWorkoutToImperial(workout);
                AnalyzeWorkout(workout);
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
            string[] channelTypes = { "Power", "HeartRate" };
            foreach (string channelType in channelTypes)
            {
                System.Predicate<DataChannel> dataTypeFilter = channel => channel.DataType.Equals(channelType);
                DataChannel dataChannel = workout.DataChannels.Find(dataTypeFilter);
                dataChannel.MaxEfforts = new DataChannelStatsCalculator(dataChannel.Data).CalculateAllEfforts();
                ConsoleView.ReportEfforts(dataChannel);
            }
        }
    }
}