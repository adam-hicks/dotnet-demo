using System.Collections.Generic;

namespace PeakswareTest.Models
{
    public class Session
    {
        public Dictionary<string, object> SessionMetrics { get; set; }
        public Session() => new Dictionary<string, object>();
    }
}