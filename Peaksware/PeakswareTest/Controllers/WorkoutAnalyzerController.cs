using PeakswareTest.Data;
using PeakswareTest.Views;
using PeakswareTest.Models;
using PeakswareTest.Business_Logic;

namespace PeakswareTest.Controllers

{
    public class WorkoutAnalyzerController
    {
        public void Run()
        {
            ConsoleView.Welcome();
			// If the user does not provide input, a default file is used. If bad input is given, useable input is asked for. If user chooses to quit, RetrieveData will return null.
            Workout workout = RetrieveData();
            if (workout != null)
            {
                // This design allows for different measurement systems to be added and applied through dependancy injection
                FitUnitConverter.ConvertWorkoutToImperial(workout);
                // Plotting specific data types (HR, Power, etc) is easier to handle if separated out compared to .fit's stream of Records, so data is organized into channels here.
                DerivedDataExtractor.ExtractDataChannels(workout);
                // Stats Calculator is not static due to threading. There may be a better way to handle threading the calculations given more time to research it.
                new DataChannelStatsCalculator(workout.DataChannels).CalculateEffortsForAllChannels();
                ConsoleView.PrintWorkoutSummary(workout);
            }
            else { ConsoleView.Print("Exiting..."); }

        }

        // This could be refactored to be moved from the controller to the data access layer 
        private Workout RetrieveData()
        {
            Workout workout = null;
            string inputFile;
            string msg = "Please input filename with path or Q to exit (leave blank for default file): ";
            // I decided to loop for user input and give the option of quitting or using a default file. A web interface allowing for file selection should not need the loop.
            do
            {
                inputFile = ConsoleView.GetFileName(msg);
                if (inputFile != null)
                {
                    // Invariant is used assuming Q or q will be used across all applicable languages
                    if (inputFile.ToUpperInvariant().Equals("Q"))
                    {
                        return null;
                    }
                    // If there is an input other than quit or doesn't end in .fit (checked in view), it is the responsibility of the data access layer to determine if the file path/name is available.
                    workout = FitImportFull.ImportData(inputFile);
                }
                msg = "Specified file could not be read. Please try again: ";
            } while (workout == null);
            return workout;
        }
    }
}