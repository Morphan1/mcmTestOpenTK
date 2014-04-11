using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.Client.Networking
{
    public abstract class AbstractPacketOut
    {
        /// <summary>
        /// Converts the packets information to a sendable byte array.
        /// </summary>
        public abstract byte[] ToBytes();

        /// <summary>
        /// The ID of this packet.
        /// </summary>
        public byte ID = 0;
    }
}
