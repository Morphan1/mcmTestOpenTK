using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.NetworkHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut;
using mcmtestOpenTK.ServerSystem.GameHandlers.GameHelpers;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;

namespace mcmtestOpenTK.ServerSystem.GameHandlers.Entities
{
    public class Player: MovingEntity
    {
        public Player(): base(EntityType.PLAYER)
        {
            ToSend = new List<byte[]>();
        }

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
        /// A list of all packets waiting to be sent.
        /// </summary>
        public List<byte[]> ToSend;

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
            Spawn(Server.MainWorld);
        }

        void Spawn(World world)
        {
            for (int i = 0; i < world.Entities.Count; i++)
            {
                Send(new SpawnPacketOut(world.Entities[i]));
            }
            world.Spawn(this);
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
            if (!IsAlive)
            {
                return;
            }
            PlayerHandler.Send(this, Packet);
        }

        /// <summary>
        /// Prepares the player.
        /// </summary>
        public void Init()
        {
            Send(new HelloPacketOut(this));
        }

        public override void Tick()
        {
            if (!IsAlive || !Network.IsAlive)
            {
                IsAlive = false;
                Network.Disconnect();
                world.Destroy(this);
                return;
            }
            base.Tick();
        }

        public override void Kill()
        {
        }

        public override byte[] GetData()
        {
            // TODO: Name, etc.
            return new byte[0];
        }

        public override bool HandleVariable(string varname, string vardata)
        {
            return false;
        }
    }
}
