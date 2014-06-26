using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using mcmtestOpenTK.Shared;
using System.Threading;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut;
using mcmtestOpenTK.ServerSystem.CommonHandlers;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers
{
    public class NewConnection
    {
        /// <summary>
        /// Cap off at 100 KB for sanity.
        /// </summary>
        public const int MAX_PACKET_SIZE = 1024 * 100;

        /// <summary>
        /// Bytes of data waiting from this connection.
        /// </summary>
        public byte[] ReceivedSoFar = new byte[0];

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

        /// <summary>
        /// The relevant player object, if any.
        /// </summary>
        Player player = null;

        /// <summary>
        /// A textual representation of the IP address that started this connection.
        /// </summary>
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
        /// How long this connection has been running.
        /// </summary>
        public float TimeAlive = 0;

        /// <summary>
        /// Ticks the connection.
        /// </summary>
        public void Tick()
        {
            try
            {
                TimeAlive += Server.DeltaF;
                if (Type != ConnectionType.GAME)
                {
                    if (TimeAlive > 10f)
                    {
                        SysConsole.Output(OutputType.INFO, "[Net] " + IP + " failed to connect: time out");
                        IsAlive = false;
                        Sock.Close(1);
                        return;
                    }
                }
                else
                {
                    if (!Sock.Connected)
                    {
                        if (!IsAlive)
                        {
                            SysConsole.Output(OutputType.INFO, "[Net] " + IP + " failed to connect: socket died");
                        }
                        IsAlive = false;
                        Sock.Close(1);
                        return;
                    }
                    if (!player.IsIdentified && TimeAlive > 10f)
                    {
                        SysConsole.Output(OutputType.INFO, "[Net] " + IP + " failed to connect: time out / no identification");
                        IsAlive = false;
                        player.Kick("IDENTIFY TIMEOUT");
                        Sock.Close(1);
                        return;
                    }
                    else
                    {
                        PlayerHandler.TickSend(player);
                    }
                }
                if (!Sock.Connected)
                {
                    if (IsAlive)
                    {
                        SysConsole.Output(OutputType.INFO, "[Net] " + IP + " failed to connect: forcibly disconnected");
                    }
                    IsAlive = false;
                    Sock.Close(1);
                    return;
                }
                int avail = Sock.Available;
                if (avail <= 0)
                {
                    return;
                }
                if (avail + ReceivedSoFar.Length >= MAX_PACKET_SIZE)
                {
                    // NOPE NOPE NOPE.
                    SysConsole.Output(OutputType.INFO, "[Net] " + IP + " failed to connect: massive packet");
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
                    SysConsole.Output(OutputType.INFO, "[Net] " + IP + " failed to connect: invalid packet");
                    IsAlive = false;
                    Sock.Close();
                }
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException)
                {
                    throw ex;
                }
                IsAlive = false;
                if (Type == ConnectionType.GAME)
                {
                    if (player != null)
                    {
                        player.Kick("INTERNAL NETWORK ERROR");
                        player.IsAlive = false;
                    }
                }
                SysConsole.Output(OutputType.INFO, "[Net] " + IP + " failed to connect: internal error: " + ex.Message);
                if (!(ex is SocketException))
                {
                    ErrorHandler.HandleError("Player disconnect", ex);
                }
                try
                {
                    Sock.Close();
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        ConnectionType RecognizeType()
        {
            if (ReceivedSoFar.Length < 5)
            {
                return ConnectionType.UNKNOWN;
            }
            if (ReceivedSoFar[0] == (byte)'G')
            {
                if (ReceivedSoFar[1] == (byte)'E' && ReceivedSoFar[2] == (byte)'T'
                    && ReceivedSoFar[3] == (byte)' ' && ReceivedSoFar[4] == (byte)'/')
                {
                    return ConnectionType.HTTP;
                }
                if (ReceivedSoFar[1] == (byte)'A' && ReceivedSoFar[2] == (byte)'M'
                    && ReceivedSoFar[3] == (byte)'E' && ReceivedSoFar[4] == (byte)0)
                {
                    return ConnectionType.GAME;
                }
            }
            else if (ReceivedSoFar[0] == (byte)'P')
            {
                if (ReceivedSoFar[1] == (byte)'I' && ReceivedSoFar[2] == (byte)'N'
                    && ReceivedSoFar[3] == (byte)'G' && ReceivedSoFar[4] == (byte)0)
                {
                    return ConnectionType.PING;
                }
                if (ReceivedSoFar[1] == (byte)'O' && ReceivedSoFar[2] == (byte)'S'
                    && ReceivedSoFar[3] == (byte)'T' && ReceivedSoFar[4] == (byte)' ')
                {
                    return ConnectionType.HTTP;
                }
            }
            else if (ReceivedSoFar[0] == (byte)'H')
            {
                if (ReceivedSoFar[1] == (byte)'E' && ReceivedSoFar[2] == (byte)'A'
                    && ReceivedSoFar[3] == (byte)'D' && ReceivedSoFar[4] == (byte)' ')
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
                        WebHandler output = WebHandler.GetWebpage(Encoding.UTF8.GetString(ReceivedSoFar));
                        Sock.Send(output.ToBytes());
                        SysConsole.Output(OutputType.INFO, "[Net] " + IP + " successfully sent HTTP " + output.BaseRequest + ", response: " + output.Status);
                        IsAlive = false;
                        Sock.Close(5);
                    }
                }
            }
        }

        void HandlePing()
        {
            byte[] name = FileHandler.encoding.GetBytes(ServerCVar.v_name.Value);
            byte[] toret = new byte[4 + name.Length];
            BitConverter.GetBytes(name.Length).CopyTo(toret, 0);
            name.CopyTo(toret, 4);
            Sock.Send(toret);
            SysConsole.Output(OutputType.INFO, "[Net] " + IP + " successfully sent ping, responding with basic info.");
            IsAlive = false;
            Sock.Close(5);
        }

        void HandleGame()
        {
            if (player == null)
            {
                if (ReceivedSoFar.Length == 5)
                {
                    ReceivedSoFar = new byte[0];
                }
                else
                {
                    byte[] temp = new byte[ReceivedSoFar.Length - 5];
                    Array.Copy(ReceivedSoFar, 5, temp, 0, ReceivedSoFar.Length - 5);
                }
                player = new Player();
                player.Network = this;
                SysConsole.Output(OutputType.INFO, "[Net] " + IP + " successfully sent GAME CONNECT! Starting connection system...");
                player.Init();
            }
            PlayerHandler.UpdateTick(player);
        }

        /// <summary>
        /// Immediately closes the connection.
        /// </summary>
        public void Disconnect()
        {
            if (!IsAlive)
            {
                return;
            }
            Sock.Close(5);
            IsAlive = false;
            if (Type == ConnectionType.GAME)
            {
                if (player != null)
                {
                    player.IsAlive = false;
                }
            }
            if (player != null && player.IsIdentified)
            {
                SysConsole.Output(OutputType.INFO, "[Net] " + player.Username + "/" + IP + " disconnected.");
            }
            else
            {
                SysConsole.Output(OutputType.INFO, "[Net] " + IP + " disconnected.");
            }
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
