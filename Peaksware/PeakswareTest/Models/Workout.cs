using System;
using System.Collections.Generic;

namespace PeakswareTest.Models
{
    public class Workout
    {
        public Session session;
        public List<DataChannel> DataChannels { get; set; }
        public List<Lap> Laps { get; set;} 
    }
}