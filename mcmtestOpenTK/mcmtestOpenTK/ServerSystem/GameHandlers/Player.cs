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
        /// The name of this player.
        /// </summary>
        public string Username = "";

        /// <summary>
        /// The session this player used to log in.
        /// </summary>
        public string Session = "";

        /// <summary>
        /// Whether the user has successfully logged in.
        /// </summary>
        public bool IsIdentified = false;

        /// <summary>
        /// Call when the user has fully identified, to let them into the server.
        /// </summary>
        public void Identified()
        {
            if (IsIdentified)
            {
                return;
            }
            SysConsole.Output(OutputType.INFO, "Client '" + Username + "' is now identified!");
            IsIdentified = true;
            Send(new PingPacketOut(this));
        }

        /// <summary>
        /// Immediately kicks the player for the specified reason.
        /// </summary>
        /// <param name="reason">Why the player was kicked.</param>
        public void Kick(string reason)
        {
            if (IsAlive)
            {
                Send(new DisconnectPacketOut(this, reason));
                SysConsole.Output(OutputType.INFO, "Player " + Username + "/" + Network.IP + " was kicked: " + reason);
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
        }
    }
}
