using System;
using System.Collections.Generic;

namespace PeakswareTest.Models
{
    public class Workout
    {
        public DateTime? StartTime { get; set; }
        public TimeSpan TotalTime { get; set; }
        public List<DataChannel> DataChannels { get; set; }
    }
}