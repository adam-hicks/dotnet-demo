using System.Collections.Generic;

namespace PeakswareTest.Models
{
    public class DataChannel
    {
        public string DataType { get; set; }
        public int AverageValue { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public Dictionary<int, int> MaxEfforts { get; set; }
        public Dictionary<double, double> Data { get; set; }
    }
}