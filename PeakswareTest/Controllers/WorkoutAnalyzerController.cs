using System.Collections.Generic;
using PeakswareTest.DAO;
using PeakswareTest.Views;
using PeakswareTest.DTO;

namespace PeakswareTest.Controllers

{
    public class WorkoutAnalyzerController
    {

        public void run()
        {
            // Greet user and ask for filename
            string inputFile = ConsoleView.getFileName();
            // Instantiate new workout model to hold results for the specific workout to be imported
            WorkoutDto workout = new WorkoutDto();
            // Retrieve workout data from fit file and store in data channels within the workout DTO. This could be modified to retrieve user-specified channels if available.
            workout.dataChannels = FitImportDao.importData(inputFile);
            // Find max efforts. This could be modified to allow the user to input or select the effort durations they are interested in.
            workout.dataChannels["Power"].calculateAllEfforts();
            // Report summary to user. If the view was reporting more information, the entire workout DTO might be passed to the view. Here, I only pass the relevant dictionary for efficiency.
            ConsoleView.reportEfforts(workout.getMaxEffortsForChannel("Power"));
        }
    }
}