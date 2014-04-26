using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.Client.Networking.PacketsOut
{
    public class DisconnectPacketOut: AbstractPacketOut
    {
        public DisconnectPacketOut()
        {
            ID = 255;
        }

        public override byte[] ToBytes()
        {
            return new byte[0];
        }
    }
}
