using System.Collections.Generic;

namespace PeakswareTest.Models
{
    public class Workout
    {
        public Session Session { get; set; }
        public List<Lap> Laps { get; set; }
        public List<Record> Records { get; set; }
		public List<DataChannel> DataChannels { get; set; }

        public Workout() {
            Laps = new List<Lap>();
            Records = new List<Record>();
            DataChannels = new List<DataChannel>();
        }
    }
}