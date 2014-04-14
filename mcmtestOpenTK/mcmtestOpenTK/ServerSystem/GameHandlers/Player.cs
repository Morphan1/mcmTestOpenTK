using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.NetworkHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut;

namespace mcmtestOpenTK.ServerSystem.GameHandlers
{
    public class Player
    {
        /// <summary>
        /// The network Connection object for this player.
        /// </summary>
        public NewConnection Network;

        /// <summary>
        /// The current Ping ID, for networking.
        /// </summary>
        public byte PingID;

        /// <summary>
        /// Indicates that the player hasn't been kicked or dropped.
        /// </summary>
        public bool IsAlive = true;

        /// <summary>
        /// Immediately kicks the player for the specified reason.
        /// </summary>
        /// <param name="reason">Why the player was kicked.</param>
        public void Kick(string reason)
        {
            if (IsAlive)
            {
                // SEND KICK REASON
                SysConsole.Output(OutputType.INFO, "Player <Name>/" + Network.IP + " was kicked: " + reason);
                IsAlive = false;
                Network.Disconnect();
            }
        }

        /// <summary>
        /// Sends a data-packet to the player.
        /// </summary>
        /// <param name="Packet">The packet to send</param>
        public void Send(AbstractPacketOut Packet)
        {
            PlayerHandler.Send(this, Packet);
        }

        /// <summary>
        /// Prepares the player.
        /// </summary>
        public void Init()
        {
            Send(new HelloPacketOut(this));
            Send(new PingPacketOut(this));
        }
    }
}
