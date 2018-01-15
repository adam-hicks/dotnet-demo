using System;
using System.Collections.Generic;
using System.IO;
using Dynastream.Fit;
using PeakswareTest.Models;

namespace PeakswareTest.Data
{
    public static class FitImportFull
    {


        static Dictionary<ushort, int> mesgCounts = new Dictionary<ushort, int>();
        private static System.DateTime? _start;
        private static Workout workout = new Workout();

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
            return workout;
        }

        static void OnMesg(object sender, MesgEventArgs e)
        {
            var sessionMesg = (SessionMesg)e.mesg;
            foreach (Field field in e.mesg.Fields)
            {
                for (int j = 0; j < field.GetNumValues(); j++)
                {
                    StoreDataInAppropriateModel(e.mesg.Name, field);
                }
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

        private static void StoreDataInAppropriateModel(string dataType, Field field)
        {
            switch (dataType)
            {
                case "Session":
                    Session session = new Session();
                    session.SessionMetrics.Add(field.GetName(), field.GetValue());
                    workout.Session = session;
                    break;
                case "Lap":
                    Lap lap = new Lap();
                    lap.LapMetrics.Add(field.GetName(), field.GetValue());
                    workout.Laps.Add(lap);
                    break;
                case "Record":
                    Record record = new Record();
                    record.RecordMetrics.Add(field.GetName(), field.GetValue());
                    workout.Records.Add(record);
                    break;
                default:
                    break;
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
