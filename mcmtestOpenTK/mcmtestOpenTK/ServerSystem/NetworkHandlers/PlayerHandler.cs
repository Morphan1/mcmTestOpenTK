﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.CommandHandlers;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsIn;
using System.Threading;
using mcmtestOpenTK.Shared.CommandSystem;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers
{
    public class PlayerHandler
    {
        /// <summary>
        /// Called to handle new input from a player's connection.
        /// </summary>
        /// <param name="player">The Player object to tick</param>
        public static void UpdateTick(Player player)
        {
            NewConnection conn = player.Network;
        CheckAgain:
            if (!conn.IsAlive)
            {
                return;
            }
            if (conn.ReceivedSoFar.Length < 4)
            {
                return;
            }
            int len = BitConverter.ToInt32(conn.ReceivedSoFar, 0);
            if (len > NewConnection.MAX_PACKET_SIZE || len <= 0)
            {
                // Corrupted data?
                conn.Disconnect();
                return;
            }
            if (conn.ReceivedSoFar.Length < 4 + len)
            {
                return;
            }
            byte[] packet = new byte[len];
            Array.Copy(conn.ReceivedSoFar, 4, packet, 0, len);
            if (conn.ReceivedSoFar.Length > len + 4)
            {
                byte[] temp = new byte[conn.ReceivedSoFar.Length - (len + 4)];
                Array.Copy(conn.ReceivedSoFar, len + 4, temp, 0, conn.ReceivedSoFar.Length - (len + 4));
                conn.ReceivedSoFar = temp;
                HandlePacket(player, packet);
                goto CheckAgain;
            }
            else
            {
                conn.ReceivedSoFar = new byte[0];
                HandlePacket(player, packet);
            }
        }

        public static void TickSend(Player player)
        {
            NewConnection conn = player.Network;
            if (!conn.IsAlive)
            {
                return;
            }
            if (player.ToSend.Count > 0)
            {
                int Count = 0;
                int TotalLen = 0;
                for (int i = 0; i < player.ToSend.Count; i++)
                {
                    if (TotalLen + player.ToSend[i].Length > conn.Sock.SendBufferSize)
                    {
                        if (TotalLen == 0)
                        {
                            conn.Disconnect();
                        }
                        break;
                    }
                    Count++;
                    TotalLen += player.ToSend[i].Length;
                }
                byte[] created = new byte[TotalLen];
                int pos = 0;
                for (int i = 0; i < Count; i++)
                {
                    player.ToSend[i].CopyTo(created, pos);
                    pos += player.ToSend[i].Length;
                }
                player.ToSend.RemoveRange(0, Count);
                Send(player, created);
            }
        }

        static void HandlePacket(Player player, byte[] Packet)
        {
            if (Packet.Length == 0)
            {
                return;
            }
            if (Packet.Length > NewConnection.MAX_PACKET_SIZE)
            {
                return;
            }
            byte ID = Packet[0];
            AbstractPacketIn Handler;
            if (!player.IsIdentified && ID != 2 && ID != 3 && ID != 255)
            {
                SysConsole.Output(OutputType.WARNING, "Invalid player packet from " + player.Network.IP + " (ID: " + ID + ") (not identified!)");
                return;
            }
            switch (ID)
            {
                case 2:
                    Handler = new PingPacketIn(); break;
                case 3:
                    Handler = new IdentityPacketIn(); break;
                case 4:
                    Handler = new MovementPacketIn(); break;
                case 5:
                    Handler = new CommandPacketIn(); break;
                case 255:
                    Handler = new DisconnectPacketIn(); break;
                default:
                    SysConsole.Output(OutputType.WARNING, "Invalid packet from client (ID: " + ID + ")!");
                    return;
            }
            byte[] Holder = new byte[Packet.Length - 1];
            Array.Copy(Packet, 1, Holder, 0, Packet.Length - 1);
            Handler.FromBytes(player, Holder);
            if (Handler.IsValid)
            {
                Handler.Execute(player);
            }
            else
            {
                SysConsole.Output(OutputType.WARNING, "Invalid player packet from " + player.Network.IP + " (ID: " + ID + ")");
            }
        }

        /// <summary>
        /// Sends a packet to the server.
        /// </summary>
        /// <param name="Packet">The packet to send</param>
        public static void Send(Player player, AbstractPacketOut Packet)
        {
            Send(player, Packet.ID, Packet.ToBytes());
        }

        /// <summary>
        /// Sends a packet to the server.
        /// </summary>
        /// <param name="ID">The packet type ID</param>
        /// <param name="Packet">The packet to send</param>
        static void Send(Player player, byte ID, byte[] Packet)
        {
            byte[] holder = new byte[Packet.Length + 5];
            BitConverter.GetBytes(Packet.Length + 1).CopyTo(holder, 0);
            holder[4] = ID;
            Packet.CopyTo(holder, 5);
            player.ToSend.Add(holder);
        }

        static void Send(Player player, byte[] Packet)
        {
            if (!player.Network.IsAlive)
            {
                return;
            }
            if (player.Network.Sock == null || !player.Network.Sock.Connected)
            {
                return;
            }
            try
            {
                player.Network.Sock.Send(Packet);
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException)
                {
                    throw ex;
                }
                if (!player.IsAlive)
                {
                    SysConsole.Output(OutputType.INFO, "[Net] " + player.Username + "/" + player.Network.IP +
                        " failed connection: internal error: " + ex.Message);
                }
                player.IsAlive = false;
                player.Network.Disconnect();
                try
                {
                    player.Network.Sock.Close();
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

    }
}
