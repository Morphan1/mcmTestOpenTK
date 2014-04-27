using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;
using mcmtestOpenTK.ServerSystem.CommonHandlers;

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
            return new byte[] { (byte)'H', (byte)'E', (byte)'L', (byte)'L', (byte)'O', ServerCVar.g_online.ValueB ? (byte)1 : (byte)0 };
        }
    }
}
