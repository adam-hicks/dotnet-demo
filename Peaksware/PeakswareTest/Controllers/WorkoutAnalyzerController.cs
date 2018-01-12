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
                System.Predicate<DataChannel> powerDataType = channel => channel.DataType.Equals("Power");
                DataChannel powerDataChannel = workout.DataChannels.Find(powerDataType);
                powerDataChannel.MaxEfforts = new DataChannelStatsCalculator(workout.DataChannels.Find(channel => channel.DataType.Equals("Power")).Data).CalculateAllEfforts();
                ConsoleView.ReportEfforts(powerDataChannel.MaxEfforts);
            }
            else { ConsoleView.Print("Exiting..."); }

        }

        private Workout RetrieveData()
        {
            Workout workout;
            string inputFile;
            string msg = "Please input filename with path or Q to exit (leave blank for default file): ";
            do
            {
                inputFile = ConsoleView.GetFileName(msg);
                if (inputFile.Equals("Q") || inputFile.Equals("q"))
                {
                    return null;
                }
                workout = FitImport.ImportData(inputFile);
                msg = "Specified file could not be read. Please try again: ";
            } while (workout != null);
            return workout;
        }
    }
}