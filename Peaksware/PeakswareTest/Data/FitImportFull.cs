using System;
using System.Collections.Generic;
using System.IO;
using Dynastream.Fit;
using PeakswareTest.Models;

namespace PeakswareTest.Data
{
    public static class FitImportFull
    {
        private static readonly double SPEED_MPH_FROM_MPS = 2.24;
        private static readonly double DISTANCE_METERS_TO_MILES = .0006214;

        static Dictionary<ushort, int> mesgCounts = new Dictionary<ushort, int>();
        private static List<DataChannel> dataChannels;
        private static System.DateTime? _start;

        public static readonly double DISTANCE_METERS_TO_FEET = 3.281;

        public static Workout ImportData(string filename)
        {
            // Attempt to open .FIT file
            if (!System.IO.File.Exists(filename))
            {
                return null;
            }
            using (var fitSource = new FileStream(filename, FileMode.Open))
            {
                Decode decodeDemo = new Decode();
                MesgBroadcaster mesgBroadcaster = new MesgBroadcaster();

                // Connect the Broadcaster to our event source (in this case the Decoder)
                decodeDemo.MesgEvent += mesgBroadcaster.OnMesg;

                // Subscribe to message events of interest by connecting to the Broadcaster
                mesgBroadcaster.MesgEvent += OnMesg;
                mesgBroadcaster.FileIdMesgEvent += OnFileIDMesg;

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
                DataChannels = dataChannels
            };
            return workout;
        }

        static void OnMesg(object sender, MesgEventArgs e)
        {
            if (dataChannels == null)
            {
                dataChannels = new List<DataChannel>();
            }
            switch (e.mesg.Name)
            {
                case "Record":
                    RecordImport(e);
                    break;
                case "Lap":
                    RecordLap(e);
                    break;
                case "Session":
                    RecordSession(e);
                    break;
                default:
                    break;
            }

            if (mesgCounts.ContainsKey(e.mesg.Num))
            {
                mesgCounts[e.mesg.Num]++;
            }
            else
            {
                mesgCounts.Add(e.mesg.Num, 1);
            }
        }

        private static void RecordSession(MesgEventArgs e)
        {
            var sessionMesg = (SessionMesg)e.mesg;
            int i = 0;
            foreach (Field field in e.mesg.Fields)
            {
                for (int j = 0; j < field.GetNumValues(); j++)
                {
                    ConvertDefaultUnitsToImperial(field);
                    Session session = new Session();
                    session.SessionMetrics.Add(field.GetName(), (double)Convert.ToDecimal(field.GetValue()));
                }
                i++;
            }
        }

        private static void RecordLap(MesgEventArgs e)
        {
            var lapMesg = (LapMesg)e.mesg;
            int i = 0;
            foreach (Field field in e.mesg.Fields)
            {
                for (int j = 0; j < field.GetNumValues(); j++)
                {
                    ConvertDefaultUnitsToImperial(field);
                    System.Predicate<DataChannel> dataTypeFilter = channel => channel.DataType.Equals(field.GetName());
                    Lap lap = new Lap();
                    lap.LapMetrics.Add(field.GetName(), (double)Convert.ToDecimal(field.GetValue()));
                }

                i++;
            }
        }

        private static void RecordImport(MesgEventArgs e)
        {
            var record = (RecordMesg)e.mesg;
            int i = 0;
            foreach (Field field in e.mesg.Fields)
            {
                for (int j = 0; j < field.GetNumValues(); j++)
                {
                    ConvertDefaultUnitsToImperial(field);
                    System.Predicate<DataChannel> dataTypeFilter = channel => channel.DataType.Equals(field.GetName());
                    DataChannel dataChannel = null;
                    dataChannel = dataChannels.Find(dataTypeFilter);
                    if (dataChannel == null)
                    {
                        dataChannel = new DataChannel
                        {
                            DataType = field.GetName()
                        };
                    }
                    double timeOffset = GetTimeOffset(record.GetTimestamp().GetDateTime());
                    dataChannel.Data.Add(timeOffset, (double)Convert.ToDecimal(field.GetValue()));
                }

                i++;
            }
        }

        private static void ConvertDefaultUnitsToImperial(Field field)
        {
            if (field.GetName().Contains("Speed"))
            {
                field.SetValue((double)Convert.ToDecimal(field.GetValue()) * SPEED_MPH_FROM_MPS);
            }
            else if (field.GetName() == "Distance")
            {
                field.SetValue((double)Convert.ToDecimal(field.GetValue()) * DISTANCE_METERS_TO_MILES);
            }
            else if (field.GetName().Contains("Altitude"))
            {
                field.SetValue((double)Convert.ToDecimal(field.GetValue()) * DISTANCE_METERS_TO_FEET);
            }
            else
            {
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
