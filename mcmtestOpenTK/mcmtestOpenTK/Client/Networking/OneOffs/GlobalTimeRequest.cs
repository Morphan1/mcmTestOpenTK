using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.Networking;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.Networking.OneOffs
{
    public class GlobalTimeRequest : NetPing
    {
        /// <summary>
        /// Sends a message to the Global Server, requesting the remote time.
        /// </summary>
        /// <param name="Announce">Whether to announce the result in the console</param>
        /// <returns>The NetPing object created for this time request</returns>
        public static GlobalTimeRequest RequestTime(bool Announce)
        {
            GlobalTimeRequest gtr = new GlobalTimeRequest();
            gtr.ShouldAnnounce = Announce;
            NetworkingObjects.Add(gtr);
            gtr.Send();
            return gtr;
        }

        /// <summary>
        /// The time returned by a time request.
        /// </summary>
        public string Time = null;

        /// <summary>
        /// Whether the current time should be announced when it is read.
        /// </summary>
        public bool ShouldAnnounce = false;

        Socket socket;

        string Error = null;

        Object Locker = new Object();

        public override void Send()
        {
            socket = NetworkUtil.CreateSocket();
            Thread thr = new Thread(new ThreadStart(ConnectMe));
            thr.Start();
        }

        void ConnectMe()
        {
            try
            {
                socket.Connect(NetPing.GlobalAddress, NetPing.GlobalPort);
                byte[] SendMe = new byte[5] { 27, 27, 27, 27, 27 };
                socket.Send(SendMe);
            }
            catch (Exception ex)
            {
                lock (Locker)
                {
                    Error = ex.Message;
                }
            }
        }

        public override void TickMe()
        {
            lock (Locker)
            {
                if (Error != null)
                {
                    TimeRan = NetPing.MaxRunTime;
                    KillQuietly = true;
                    UIConsole.WriteLine(TextStyle.Color_Error + "Global Time Request failed with message: " + TextStyle.Color_Separate + Error);
                    return;
                }
            }
            if (socket.Connected)
            {
                int avail = socket.Available;
                // TODO: Handle less terribly - length int!
                if (avail >= 3)
                {
                    byte[] bytes = new byte[avail];
                    socket.Receive(bytes, avail, SocketFlags.None);
                    Time = FileHandler.encoding.GetString(bytes);
                    if (ShouldAnnounce)
                    {
                        UIConsole.WriteLine(TextStyle.Color_Importantinfo + "Response from Global Server: " + TextStyle.Color_Separate + Time);
                    }
                    ready = true;
                }
            }
        }

        public override void Kill()
        {
            try
            {
                socket.Close();
            }
            catch (Exception)
            {
                // Irrelevant, we already want to scrap this entirely.
            }
        }
    }
}
