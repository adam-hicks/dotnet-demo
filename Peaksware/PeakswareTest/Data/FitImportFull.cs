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
        private static string[] channelsOfInterest = { "Power", "HeartRate", "Cadence" };

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
                InitializeDataChannels();

                Decode decodeDemo = new Decode();
                MesgBroadcaster mesgBroadcaster = new MesgBroadcaster();

                // Connect the Broadcaster to our event source (in this case the Decoder)
                decodeDemo.MesgEvent += mesgBroadcaster.OnMesg;

                // Subscribe to message events of interest by connecting to the Broadcaster
                mesgBroadcaster.MesgEvent += OnMesg;
                //mesgBroadcaster.RecordMesgEvent += OnRecordMesg;
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

        //static void OnRecordMesg(object sender, MesgEventArgs e)
        //{
        //    Console.WriteLine("=============================================================================================");
        //    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        //    Console.WriteLine("Entering OnRecordMesg Method");
        //    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        //    var record = (RecordMesg)e.mesg;

        //    WriteFieldWithOverrides(record, RecordMesg.FieldDefNum.HeartRate);
        //    WriteFieldWithOverrides(record, RecordMesg.FieldDefNum.Cadence);
        //    WriteFieldWithOverrides(record, RecordMesg.FieldDefNum.Speed);
        //    WriteFieldWithOverrides(record, RecordMesg.FieldDefNum.Distance);

        //    foreach (string channelType in channelsOfInterest)
        //    {
        //        var data = record.GetFieldValue(channelType);
        //        if (data != null)
        //        {
        //            double timeOffset = GetTimeOffset(record.GetTimestamp().GetDateTime());
        //            dataChannels.Find(channel => channel.DataType.Equals(channelType)).Data.Add(timeOffset, Convert.ToInt32(data));
        //        }
        //    }

        //}

        //private static void WriteFieldWithOverrides(Mesg mesg, byte fieldNumber)
        //{
        //    Field profileField = Profile.GetField(mesg.Num, fieldNumber);
        //    bool nameWritten = false;

        //    if (null == profileField)
        //    {
        //        return;
        //    }

        //    IEnumerable<FieldBase> fields = mesg.GetOverrideField(fieldNumber);
        //    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        //    Console.WriteLine("Writing Field With Overrides");
        //    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

        //    foreach (FieldBase field in fields)
        //    {
        //        if (!nameWritten)
        //        {
        //            Console.WriteLine("   {0}", profileField.GetName());
        //            nameWritten = true;
        //        }

        //        if (field is Field)
        //        {
        //            if (profileField.GetName() == "Speed") {
        //                field.SetValue((double)Convert.ToDecimal(field.GetValue()) * SPEED_MPH_FROM_MPS);
        //            } else if(profileField.GetName() == "Distance") {
        //                field.SetValue((double)Convert.ToDecimal(field.GetValue()) * DISTANCE_METERS_TO_MILES);
        //            }
        //            Console.WriteLine("      native: {0}", field.GetValue());
        //        }
        //        else
        //        {
        //            Console.WriteLine("      override: {0}", field.GetValue());
        //        }
        //    }
        //}

        static void OnMesg(object sender, MesgEventArgs e)
        {
            Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("Entering OnMesg Method");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

            Console.WriteLine("OnMesg: Received Mesg with global ID#{0}, its name is {1}", e.mesg.Num, e.mesg.Name);

            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("OnMesg - Writing Fields");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

            int i = 0;
            foreach (Field field in e.mesg.Fields)
            {
                for (int j = 0; j < field.GetNumValues(); j++)
                {
                    switch (field.GetName())
                    {
                        case "Speed":
                            field.SetValue((double)Convert.ToDecimal(field.GetValue()) * SPEED_MPH_FROM_MPS);
                            break;
                        case "Distance":
                            field.SetValue((double)Convert.ToDecimal(field.GetValue()) * DISTANCE_METERS_TO_MILES);
                            break;
                        case "Altitude":
                            field.SetValue((double)Convert.ToDecimal(field.GetValue()) * DISTANCE_METERS_TO_FEET);
                            break;
                        default:
                            break;
                    }

                    Console.WriteLine("\tField{0} Index{1} (\"{2}\" Field#{4}) Value: {3} (raw value {5})",
                        i,
                        j,
                        field.GetName(),
                        field.GetValue(j),
                        field.Num,
                        field.GetRawValue(j));
                }

                i++;
            }

            if (mesgCounts.ContainsKey(e.mesg.Num))
            {
                mesgCounts[e.mesg.Num]++;
            }
            else
            {
                mesgCounts.Add(e.mesg.Num, 1);
            }
			Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
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
