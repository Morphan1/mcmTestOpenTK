using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;

namespace mcmtestOpenTK.Client.Networking
{
    public class NetworkBase
    {
        const int MAX_PACKET_SIZE = 1024 * 100;

        /// <summary>
        /// The IP currently connected/connecting to.
        /// </summary>
        public static IPEndPoint RemoteTarget;

        static Socket Sock = null;

        static bool Connected = false;

        /// <summary>
        /// Connects to a specified IP.
        /// </summary>
        /// <param name="IP">The IP to connect to</param>
        /// <returns>Whether the connection is possible</returns>
        public static bool Connect(string IP)
        {
            try
            {
                IPEndPoint target = NetworkUtil.StringToIP(IP);
                if (target == null)
                {
                    return false;
                }
                Disconnect();
                RemoteTarget = target;
                if (target.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    Sock = NetworkUtil.Createv6Socket();
                }
                else
                {
                    Sock = NetworkUtil.CreateSocket();
                }
                Sock.Blocking = false;
                Sock.SendBufferSize = MAX_PACKET_SIZE;
                Sock.ReceiveBufferSize = MAX_PACKET_SIZE;
                SocketAsyncEventArgs SAEA = new SocketAsyncEventArgs();
                SAEA.RemoteEndPoint = target;
                Connected = false;
                Sock.ConnectAsync(SAEA);
                return true;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError("NetworkBase/Connect", ex);
                return false;
            }
        }

        /// <summary>
        /// Handles any server-connection related updates.
        /// </summary>
        public static void Tick()
        {
            if (Sock == null)
            {
                Connected = false;
                return;
            }
            if (Sock.Connected && !Connected)
            {
                Connected = true;
                UIConsole.WriteLine(TextStyle.Color_Outgood + "Connected to server!");
                Sock.Send(new byte[] { (byte)'G', (byte)'A', (byte)'M', (byte)'E', (byte)0 });
            }
            else if (Connected && !Sock.Connected)
            {
                Connected = false;
                UIConsole.WriteLine(TextStyle.Color_Outbad + "Server disconnected!");
            }
        }

        /// <summary>
        /// Disconnects from the server.
        /// </summary>
        public static void Disconnect()
        {
            if (!Connected)
            {
                return;
            }
            if (Sock == null || !Sock.Connected)
            {
                return;
            }
            Sock.Close(5);
            UIConsole.WriteLine(TextStyle.Color_Importantinfo + "Disconnected from server!");
        }
    }
}
