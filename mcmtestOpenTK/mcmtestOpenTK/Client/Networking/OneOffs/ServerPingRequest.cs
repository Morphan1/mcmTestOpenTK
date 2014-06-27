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
using mcmtestOpenTK.Client.GlobalHandler;

namespace mcmtestOpenTK.Client.Networking.OneOffs
{
    public class ServerPingRequest : NetPing
    {
        /// <summary>
        /// Sends a message to the target server, requesting basic information.
        /// </summary>
        /// <param name="Announce">Whether to announce the result in the console</param>
        /// <param name="server">The server to connect to</param>
        /// <param name="port">The port of the server to connect to</param>
        /// <returns>The GlobalNetwork object created for this time request</returns>
        public static ServerPingRequest SendPing(bool Announce, string server, int port)
        {
            ServerPingRequest gtr = new ServerPingRequest();
            gtr.ShouldAnnounce = Announce;
            gtr.Address = server;
            gtr.Port = port;
            NetworkingObjects.Add(gtr);
            gtr.Send();
            return gtr;
        }

        /// <summary>
        /// The time returned by a time request.
        /// </summary>
        public string Time = null;

        /// <summary>
        /// The address connected to.
        /// </summary>
        public string Address;

        /// <summary>
        /// The port of the server connected to.
        /// </summary>
        public int Port;

        /// <summary>
        /// Whether the current time should be announced when it is read.
        /// </summary>
        public bool ShouldAnnounce = false;

        Socket socket;

        string Error = null;

        Object Locker = new Object();

        DateTime Sent;

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
                Sent = DateTime.Now;
                socket.Connect(Address, Port);
                byte[] SendMe = new byte[5] { (byte)'P', (byte)'I', (byte)'N', (byte)'G', 0 };
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
                    UIConsole.WriteLine(TextStyle.Color_Error + "Ping request failed with message: " + TextStyle.Color_Separate + Error);
                    return;
                }
            }
            if (socket.Connected)
            {
                int avail = socket.Available;
                if (avail > 4 && length == -1)
                {
                    byte[] bytes = new byte[4];
                    socket.Receive(bytes, 4, SocketFlags.None);
                    length = BitConverter.ToInt32(bytes, 0);
                    avail -= 4;
                }
                if (avail >= length && length != -1)
                {
                    byte[] bytes = new byte[length];
                    socket.Receive(bytes, length, SocketFlags.None);
                    string name = FileHandler.encoding.GetString(bytes);
                    PingedServer serv = new PingedServer(name, ((int)DateTime.Now.Subtract(Sent).TotalMilliseconds), Address, Port);
                    Screen_Servers.AddServer(serv);
                    if (ShouldAnnounce)
                    {
                        UIConsole.WriteLine(TextStyle.Color_Importantinfo + "Response from server! Name: " +
                            TextStyle.Color_Separate + name + TextStyle.Color_Importantinfo + ", ping "
                            + TextStyle.Color_Separate + serv.Ping + TextStyle.Color_Importantinfo + "ms.");
                    }
                    ready = true;
                }
            }
        }

        int length = -1;

        byte[] gotten = new byte[4];

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
