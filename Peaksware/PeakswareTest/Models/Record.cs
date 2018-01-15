using System.Collections.Generic;

namespace PeakswareTest.Models
{
    public class Record
    {
        public Dictionary<string, object> RecordMetrics { get; set; }
        public Record() => RecordMetrics = new Dictionary<string, object>();
    }
}