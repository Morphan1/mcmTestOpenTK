using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.Client.Networking.PacketsOut
{
    public class PingPacketOut: AbstractPacketOut
    {
        byte PingID;

        public PingPacketOut(byte _PingID)
        {
            ID = 2;
            PingID = _PingID;
        }

        public override byte[] ToBytes()
        {
            return new byte[5] { (byte)'P', (byte)'I', (byte)'N', (byte)'G', PingID };
        }
    }
}
