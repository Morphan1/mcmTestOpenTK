using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.Networking.PacketsIn;
using mcmtestOpenTK.Client.CommandHandlers;
using mcmtestOpenTK.Client.Networking.PacketsOut;
using mcmtestOpenTK.Client.GlobalHandler;

namespace mcmtestOpenTK.Client.Networking
{
    public class NetworkBase
    {
        const int MAX_PACKET_SIZE = 1024 * 100;

        static byte[] ReceivedSoFar = new byte[0];

        /// <summary>
        /// The IP currently connected/connecting to.
        /// </summary>
        public static IPEndPoint RemoteTarget;

        static Socket Sock = null;

        static bool Connected = false;

        /// <summary>
        /// Whether networking is active (packets should be sent).
        /// </summary>
        public static bool IsActive = false;

        /// <summary>
        /// Whether we need to login to continue networking.
        /// </summary>
        public static bool WaitingToIdentify = false;

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
                Disconnect("Connect / reset");
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
                ClientCommands.CommandSystem.Output.Good("Connected to server! Communicating...");
                Sock.Send(new byte[] { (byte)'G', (byte)'A', (byte)'M', (byte)'E', (byte)0 });
                return;
            }
            else if (Connected && !Sock.Connected)
            {
                Disconnect("Connected=true but Socket not connected");
                return;
            }
            if (!Connected)
            {
                return;
            }
            int avail = Sock.Available;
            if (avail <= 0)
            {
                return;
            }
            SysConsole.Output(OutputType.CLIENTINFO, "Got " + avail + " bytes");
            if (avail + ReceivedSoFar.Length >= MAX_PACKET_SIZE)
            {
                // NOPE NOPE NOPE.
                Disconnect("Too much data");
                return;
            }
            byte[] packet = new byte[avail];
            Sock.Receive(packet, avail, SocketFlags.None);
            byte[] temp = new byte[ReceivedSoFar.Length + packet.Length];
            ReceivedSoFar.CopyTo(temp, 0);
            packet.CopyTo(temp, ReceivedSoFar.Length);
            ReceivedSoFar = temp;
        CheckAgain:
            if (!Connected)
            {
                return;
            }
            packet = null;
            temp = null;
            if (ReceivedSoFar.Length < 4)
            {
                return;
            }
            int len = BitConverter.ToInt32(ReceivedSoFar, 0);
            if (len > MAX_PACKET_SIZE || len <= 0)
            {
                // Corrupted data?
                Disconnect("Corrupted packet");
                return;
            }
            if (ReceivedSoFar.Length < 4 + len)
            {
                return;
            }
            packet = new byte[len];
            Array.Copy(ReceivedSoFar, 4, packet, 0, len);
            if (ReceivedSoFar.Length > len + 4)
            {
                temp = new byte[ReceivedSoFar.Length - (len + 4)];
                Array.Copy(ReceivedSoFar, len + 4, temp, 0, ReceivedSoFar.Length - (len + 4));
                ReceivedSoFar = temp;
                HandlePacket(packet);
                goto CheckAgain;
            }
            else
            {
                ReceivedSoFar = new byte[0];
                HandlePacket(packet);
            }
        }

        static void HandlePacket(byte[] Packet)
        {
            if (Packet.Length == 0)
            {
                return;
            }
            if (Packet.Length > MAX_PACKET_SIZE)
            {
                return;
            }
            byte ID = Packet[0];
            AbstractPacketIn Handler;
            switch (ID)
            {
                case 1:
                    Handler = new HelloPacketIn(); break;
                case 2:
                    Handler = new PingPacketIn(); break;
                case 3:
                    Handler = new SpawnPacketIn(); break;
                case 4:
                    Handler = new PositionPacketIn(); break;
                case 5:
                    Handler = new DespawnPacketIn(); break;
                case 6:
                    Handler = new MessagePacketIn(); break;
                case 255:
                    Handler = new DisconnectPacketIn(); break;
                default:
                    ClientCommands.CommandSystem.Output.Bad("<{color.warning}>Invalid packet from server (ID: <{color.emphasis}>" + ID + "<{color.warning}>)!");
                    return;
            }
            byte[] Holder = new byte[Packet.Length - 1];
            Array.Copy(Packet, 1, Holder, 0, Packet.Length - 1);
            Handler.FromBytes(Holder);
            if (Handler.IsValid)
            {
                Handler.Execute();
            }
            else
            {
                ClientCommands.CommandSystem.Output.Bad("<{color.warning}>Imperfect packet from server (ID: <{color.emphasis}>" + ID + "<{color.warning}>)!");
            }
        }

        /// <summary>
        /// Sends a packet to the server.
        /// </summary>
        /// <param name="Packet">The packet to send</param>
        public static void Send(AbstractPacketOut Packet)
        {
            Send(Packet.ID, Packet.ToBytes());
        }

        /// <summary>
        /// Sends a packet to the server.
        /// </summary>
        /// <param name="ID">The packet type ID</param>
        /// <param name="Packet">The packet to send</param>
        static void Send(byte ID, byte[] Packet)
        {
            byte[] holder = new byte[Packet.Length + 5];
            BitConverter.GetBytes(Packet.Length + 1).CopyTo(holder, 0);
            holder[4] = ID;
            Packet.CopyTo(holder, 5);
            Send(holder);
        }

        static void Send(byte[] Packet)
        {
            if (!Connected)
            {
                return;
            }
            if (Sock == null || !Sock.Connected)
            {
                return;
            }
            Sock.Send(Packet);
        }

        /// <summary>
        /// Disconnects from the server.
        /// </summary>
        public static void Disconnect(string reason)
        {
            IsActive = false;
            MainGame.DestroyWorld();
            ClientCommands.CommandSystem.Output.Bad("<{color.info}>Disconnected from server! Reason: " + reason);
            if (!Connected)
            {
                return;
            }
            WaitingToIdentify = false;
            ReceivedSoFar = new byte[0];
            Connected = false;
            if (Sock == null || !Sock.Connected)
            {
                return;
            }
            Send(new DisconnectPacketOut());
            Sock.Close(5);
        }

        /// <summary>
        /// Call when a proper session key is acquired.
        /// </summary>
        public static void Identify()
        {
            if (WaitingToIdentify)
            {
                WaitingToIdentify = false;
                Send(new IdentityPacketOut(MainGame.Username, MainGame.Session));
                MainGame.Session = "";
            }
        }
    }
}
