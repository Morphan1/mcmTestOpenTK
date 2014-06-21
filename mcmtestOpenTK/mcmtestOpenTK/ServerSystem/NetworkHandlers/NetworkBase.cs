using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;
using mcmtestOpenTK.ServerSystem.CommonHandlers;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers
{
    public class NetworkBase
    {
        /// <summary>
        /// The socket that handles all network accepting / listening.
        /// </summary>
        public static Socket MainSocket;

        /// <summary>
        /// All new connections that need to be ticked.
        /// </summary>
        public static List<NewConnection> WaitingConnections;

        static Object ConnectionLock = new Object();

        /// <summary>
        /// Prepares the network system.
        /// </summary>
        public static bool Init()
        {
            try
            {
                SysConsole.Output(OutputType.INIT, "Preparing network string manager...");
                NetStringManager.Init();
                SysConsole.Output(OutputType.INIT, "Creating socket...");
                MainSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
                MainSocket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
                MainSocket.Bind(new IPEndPoint(IPAddress.IPv6Any, ServerCVar.n_port.ValueI));
                MainSocket.Listen(100);
                SysConsole.Output(OutputType.INIT, "Socket created successfully, listening on port " + ServerCVar.n_port.ValueI + "...");
                WaitingConnections = new List<NewConnection>();
                Thread thread = new Thread(new ThreadStart(NetworkMain));
                thread.Name = "Server_NetworkListener";
                thread.Start();
                return true;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError("NetworkBase/Init", ex);
                SysConsole.Output(OutputType.ERROR, "Could not create socket and bind to port " + ServerCVar.n_port.ValueI + ", shutting down...");
                return false;
            }
        }

        static void NetworkMain()
        {
            while (true)
            {
                Socket Gotten = MainSocket.Accept();
                // TODO: Limit active connections
                lock (ConnectionLock)
                {
                    WaitingConnections.Add(new NewConnection(Gotten));
                }
            }
        }

        /// <summary>
        /// Ticks all waiting connections.
        /// </summary>
        public static void Tick()
        {
            lock (ConnectionLock)
            {
                for (int i = 0; i < WaitingConnections.Count; i++)
                {
                    WaitingConnections[i].Tick();
                    if (!WaitingConnections[i].IsAlive)
                    {
                        WaitingConnections.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        /// <summary>
        /// Sends a packet to all online players.
        /// </summary>
        /// <param name="packet">The packet to send</param>
        public static void SendToAllPlayers(AbstractPacketOut packet)
        {
            foreach (Player player in Server.MainWorld.Players)
            {
                player.Send(packet);
            }
        }
    }
}
