using System.Collections.Generic;
using PeakswareTest.Models;
using PeakswareTest.Views;

namespace PeakswareTest.Controllers

{
    public class WorkoutAnalyzerController
    {

        public void run()
        {
            // Greet user and ask for filename
            string inputFile = ConsoleView.getFileName();
            // create data model using filename
            WorkoutModel.importData(inputFile);
            // find max effort
            //int effortTimeMinutes = 20;
            //summary.findMaxEffort(effortTimeMinutes);
            // add amx effort to workout summary
            // report summary to user
        }
    }
}