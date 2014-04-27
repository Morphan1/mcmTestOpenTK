using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers
{
    public abstract class AbstractPacketIn
    {
        /// <summary>
        /// Construct the packet object from a byte array.
        /// </summary>
        /// <param name="player">The sending player</param>
        /// <param name="input">The received byte array</param>
        public abstract void FromBytes(Player player, byte[] input);

        /// <summary>
        /// Executes the changes describe in the packet.
        /// <param name="player">The sending packet</param>
        /// </summary>
        public abstract void Execute(Player player);

        /// <summary>
        /// Indicates whether the packet was successfully constructed.
        /// </summary>
        public bool IsValid = false;
    }
}
