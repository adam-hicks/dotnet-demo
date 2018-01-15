using System;
using System.Collections.Generic;

namespace PeakswareTest.Models
{
    public class Lap
    {
        public DateTime Timestamp { get; set; }
        public Dictionary<string, object> LapMetrics { get; set; }
        public Lap() => LapMetrics = new Dictionary<string, object>();
    }
}
