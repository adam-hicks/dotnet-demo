using System;
using System.Collections.Generic;

namespace PeakswareTest.Models
{
    public class Session
    {
        public DateTime Timestamp { get; set; }
        public Dictionary<string, object> SessionMetrics { get; set; }
        public Session() => SessionMetrics = new Dictionary<string, object>();
    }
}