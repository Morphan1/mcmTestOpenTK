﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.CommandHandlers;

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
            if (!conn.IsAlive)
            {
                return;
            }
        CheckAgain:
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
            switch (ID)
            {
                    // TODO!
                default:
                    ServerCommands.CommandSystem.Output.Bad("<{color.warning}>Invalid packet from server (ID: <{color.emphasis}>" + ID + "<{color.warning}>)!");
                    return;
            }
            Handler.FromBytes(player, Packet);
            if (Handler.IsValid)
            {
                Handler.Execute(player);
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
            Send(player, holder);
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
            player.Network.Sock.Send(Packet);
        }

    }
}
