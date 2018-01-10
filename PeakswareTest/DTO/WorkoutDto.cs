using System;
using System.Collections.Generic;

namespace PeakswareTest.DTO
{
    public class WorkoutDto
    {
        public Dictionary<string, IDataChannel> dataChannels { get; set; }

        internal Dictionary<int, double> getMaxEffortsForChannel(string channelKey)
        {
            if(dataChannels.ContainsKey(channelKey)){
                return (dataChannels[channelKey]).getMaxEfforts();
            } else {
                throw new Exception("no power data found for this workout");
            }
        }
    }
}