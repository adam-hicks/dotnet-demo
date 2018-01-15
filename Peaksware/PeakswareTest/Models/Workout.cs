using System;
using System.Collections.Generic;

namespace PeakswareTest.Models
{
    public class Workout
    {
        public DateTime? Timestamp { get; set; }
        public Session Session { get; set; }
        public List<Lap> Laps { get; set; }
        public List<Record> Records { get; set; }
        public List<DataChannel> DataChannels { get; set; }

        public Workout()
        {
            Laps = new List<Lap>();
            Records = new List<Record>();
            DataChannels = new List<DataChannel>();
        }

        // Started applying Law of Demeter. Should not have to chain method calls to retrieve properties of objects contained within containing object.
        public string getDuration()
        {
            double seconds = (double)Convert.ToDecimal(Session.SessionMetrics["TotalElapsedTime"]);
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return time.ToString(@"hh\:mm\:ss");
        }

        public string getTotalDistance()
        {
            return String.Format("{0:0.00}", (double)Convert.ToDecimal(Session.SessionMetrics["TotalDistance"]));
        }
    }
}