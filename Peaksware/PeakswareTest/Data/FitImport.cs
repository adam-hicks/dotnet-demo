using System;
using System.IO;
using System.Collections.Generic;
using Dynastream.Fit;
using PeakswareTest.Models;

namespace PeakswareTest.Data
{
    public static class FitImport
    {
        private static List<DataChannel> dataChannels;
        private static System.DateTime? _start;
        private static string[] channelsOfInterest = { "Power", "HeartRate", "Cadence" };

        public static Workout ImportData(string filename)
        {
            // Attempt to open .FIT file
            if (!System.IO.File.Exists(filename))
            {
                return null;
            }
            using (var fitSource = new FileStream(filename, FileMode.Open))
            {
                InitializeDataChannels();

                Decode decodeDemo = new Decode();
                MesgBroadcaster mesgBroadcaster = new MesgBroadcaster();

                // Connect the Broadcaster to our event source (in this case the Decoder)
                decodeDemo.MesgEvent += mesgBroadcaster.OnMesg;

                // Subscribe to message events of interest by connecting to the Broadcaster
                mesgBroadcaster.RecordMesgEvent += new MesgEventHandler(OnRecordMesg);
                mesgBroadcaster.FileIdMesgEvent += new MesgEventHandler(OnFileIDMesg);

                bool status = decodeDemo.IsFIT(fitSource) && decodeDemo.CheckIntegrity(fitSource);
                // Process the file
                if (status)
                {
                    decodeDemo.Read(fitSource);
                }
                else
                {
                    return null;
                }
            }
            Workout workout = new Workout
            {
                StartTime = _start,
                DataChannels = dataChannels
            };
            return workout;
        }

        private static void InitializeDataChannels()
        {
            dataChannels = new List<DataChannel>();
            foreach (string channelType in channelsOfInterest)
            {
                DataChannel channel = new DataChannel
                {
                    Data = new Dictionary<double, int>(),
                    DataType = channelType
                };
                dataChannels.Add(channel);
            }

        }

        static void OnRecordMesg(object sender, MesgEventArgs e)
        {
            var record = (RecordMesg)e.mesg;
            foreach (string channelType in channelsOfInterest)
            {
                var data = record.GetFieldValue(channelType);
                if (data != null)
                {
                    double timeOffset = GetTimeOffset(record.GetTimestamp().GetDateTime());
                    dataChannels.Find(channel => channel.DataType.Equals(channelType)).Data.Add(timeOffset, Convert.ToInt32(data));
                }
            }

        }

        static double GetTimeOffset(System.DateTime date)
        {
            if (_start == null)
                return 0;

            return date.Subtract(_start.Value).TotalMilliseconds;
        }

        static void OnFileIDMesg(object sender, MesgEventArgs e)
        {
            FileIdMesg myFileId = (FileIdMesg)e.mesg;

            _start = myFileId.GetTimeCreated().GetDateTime();
        }
    }
}
