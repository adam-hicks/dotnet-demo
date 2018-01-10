using System;
using System.IO;
using System.Collections.Generic;
using Dynastream.Fit;
using PeakswareTest.DTO;

namespace PeakswareTest.DAO
{
    public class WorkoutDataDao
    {
        private static Dictionary<string, IDataChannel> _dataChannels;
        private static System.DateTime? _start;

        public static Dictionary<string, IDataChannel> importData(string filename)
        {
            // Attempt to open .FIT file
            using (var fitSource = new FileStream(filename, FileMode.Open))
            {
                Console.WriteLine("Opening {0}", filename);
                initializeDataChannels();

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
                    throw new Exception(".fit data could not be read from selected file");
                }
            }
            return _dataChannels;
        }

        private static void initializeDataChannels()
        {
            _dataChannels = new Dictionary<string, IDataChannel>();
            IDataChannel powerChannel = new PowerDataChannel();
            powerChannel.setData(new Dictionary<double, ushort>());
            _dataChannels.Add("Power", powerChannel);
        }

        static void OnRecordMesg(object sender, MesgEventArgs e)
        {
            var record = (RecordMesg)e.mesg;

            var power = record.GetFieldValue("Power");

            if (power != null)
            {
                _dataChannels["Power"].getData().Add(GetTimeOffset(record.GetTimestamp().GetDateTime()), (ushort)power);
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
