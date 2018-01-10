using System;
using System.IO;
using System.Collections.Generic;
using Dynastream.Fit;

namespace PeakswareTest.Models
{
    public class WorkoutModel
    {
        private static Dictionary<double, ushort> _powerChannel;
        private static System.DateTime? _start;

        public static void importData(string filename)
        {
            // Attempt to open .FIT file
            using (var fitSource = new FileStream(filename, FileMode.Open))
            {
                Console.WriteLine("Opening {0}", filename);

                _powerChannel = new Dictionary<double, ushort>();

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
                    Console.WriteLine("Decoding...");
                    decodeDemo.Read(fitSource);

                    Console.WriteLine("Decoded FIT file {0}", filename
    );
                }
                else
                {
                    Console.WriteLine("Integrity Check Failed {0}", filename
    );
                }
            }
        }

        static void OnRecordMesg(object sender, MesgEventArgs e)
        {
            var record = (RecordMesg)e.mesg;

            var power = record.GetFieldValue("Power");

            if (power != null)
            {
                _powerChannel.Add(GetTimeOffset(record.GetTimestamp().GetDateTime()), (ushort)power);
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
