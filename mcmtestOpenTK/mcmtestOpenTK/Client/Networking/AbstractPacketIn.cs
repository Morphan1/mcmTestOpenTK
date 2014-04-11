using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.Client.Networking
{
    public abstract class AbstractPacketIn
    {
        /// <summary>
        /// Construct the packet object from a byte array.
        /// </summary>
        /// <param name="input">The received byte array</param>
        public abstract void FromBytes(byte[] input);

        /// <summary>
        /// Executes the changes describe in the packet.
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Indicates whether the packet was successfully constructed.
        /// </summary>
        public bool IsValid = false;
    }
}
