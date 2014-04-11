using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut
{
    public class HelloPacketOut: AbstractPacketOut
    {
        public HelloPacketOut(Player player)
        {
            ID = 1;
        }

        public override byte[] ToBytes()
        {
            return new byte[] { (byte)'H', (byte)'E', (byte)'L', (byte)'L', (byte)'O' };
        }
    }
}
