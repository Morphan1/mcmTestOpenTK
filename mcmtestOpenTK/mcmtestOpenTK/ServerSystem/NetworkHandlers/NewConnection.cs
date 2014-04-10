﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using mcmtestOpenTK.Shared;
using System.Threading;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers
{
    public class NewConnection
    {
        // Cap off at 100 KB for sanity.
        const int MAX_PACKET_SIZE = 1024 * 100;

        byte[] ReceivedSoFar = new byte[0];

        /// <summary>
        /// What type of connection this is.
        /// </summary>
        public ConnectionType Type = ConnectionType.UNKNOWN;

        /// <summary>
        /// The socket that formed this connection.
        /// </summary>
        public Socket Sock;

        /// <summary>
        /// Whether the connection is still alive.
        /// </summary>
        public bool IsAlive = true;

        public string IP;

        public NewConnection(Socket _sock)
        {
            Sock = _sock;
            Sock.Blocking = false;
            Sock.ReceiveBufferSize = MAX_PACKET_SIZE;
            Sock.SendBufferSize = MAX_PACKET_SIZE;
            IP = Sock.RemoteEndPoint.ToString();
        }

        /// <summary>
        /// Ticks the connection.
        /// </summary>
        public void Tick()
        {
            try
            {
                int avail = Sock.Available;
                if (avail <= 0)
                {
                    return;
                }
                if (avail + ReceivedSoFar.Length > MAX_PACKET_SIZE)
                {
                    // NOPE NOPE NOPE.
                    Sock.Close();
                    IsAlive = false;
                    return;
                }
                byte[] packet = new byte[avail];
                Sock.Receive(packet, avail, SocketFlags.None);
                byte[] temp = new byte[ReceivedSoFar.Length + packet.Length];
                ReceivedSoFar.CopyTo(temp, 0);
                packet.CopyTo(temp, ReceivedSoFar.Length);
                ReceivedSoFar = temp;
                packet = null;
                temp = null;
                if (Type == ConnectionType.UNKNOWN)
                {
                    Type = RecognizeType();
                }
                if (Type == ConnectionType.HTTP)
                {
                    HandleHTTP();
                }
                else if (Type == ConnectionType.PING)
                {
                    HandlePing();
                }
                else if (Type == ConnectionType.GAME)
                {
                    HandleGame();
                }
                else if (Type == ConnectionType.INVALID)
                {
                    Sock.Close();
                    IsAlive = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException)
                {
                    throw ex;
                }
                SysConsole.Output(OutputType.INFO, "Connection for " + IP + " failed: internal error: " + ex.Message);
                Sock.Close();
            }
        }

        ConnectionType RecognizeType()
        {
            if (ReceivedSoFar.Length < 4)
            {
                return ConnectionType.UNKNOWN;
            }
            if (ReceivedSoFar[0] == (byte)'G')
            {
                if (ReceivedSoFar[1] == (byte)'E' && ReceivedSoFar[2] == (byte)'T' && ReceivedSoFar[3] == (byte)' ')
                {
                    return ConnectionType.HTTP;
                }
            }
            else if (ReceivedSoFar[0] == (byte)'P')
            {
                if (ReceivedSoFar[1] == (byte)'I' && ReceivedSoFar[2] == (byte)'N' && ReceivedSoFar[3] == (byte)'G')
                {
                    return ConnectionType.PING;
                }
                if (ReceivedSoFar[1] == (byte)'O' && ReceivedSoFar[2] == (byte)'S' && ReceivedSoFar[3] == (byte)'T')
                {
                    return ConnectionType.HTTP;
                }
            }
            else if (ReceivedSoFar[0] == (byte)'G')
            {
                if (ReceivedSoFar[1] == (byte)'A' && ReceivedSoFar[2] == (byte)'M' && ReceivedSoFar[3] == (byte)'E')
                {
                    return ConnectionType.GAME;
                }
            }
            else if (ReceivedSoFar[0] == (byte)'H')
            {
                if (ReceivedSoFar[1] == (byte)'E' && ReceivedSoFar[2] == (byte)'A' && ReceivedSoFar[3] == (byte)'D')
                {
                    return ConnectionType.HTTP;
                }
            }
            return ConnectionType.INVALID;
        }

        void HandleHTTP()
        {
            for (int i = 0; i < ReceivedSoFar.Length - 1; i++)
            {
                if (ReceivedSoFar[i] == (byte)'\n')
                {
                    if (ReceivedSoFar[i + 1] == (byte)'\n' || ReceivedSoFar[i + 1] == (byte)'\r')
                    {
                        byte[] output = WebHandler.GetWebpage(Encoding.UTF8.GetString(ReceivedSoFar)).ToBytes();
                        Sock.Send(output);
                        Sock.Close(5);
                        IsAlive = false;
                    }
                }
            }
        }

        void HandlePing()
        {
        }

        void HandleGame()
        {
        }
    }

    public enum ConnectionType: byte
    {
        UNKNOWN = 0,
        HTTP = 1,
        PING = 2,
        GAME = 3,
        INVALID = 128
    }
}
