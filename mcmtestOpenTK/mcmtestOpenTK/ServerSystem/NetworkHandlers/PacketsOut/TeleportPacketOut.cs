using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut
{
    class TeleportPacketOut : AbstractPacketOut
    {
        Location loc;
        double time;
        public TeleportPacketOut(Location target)
        {
            ID = 9;
            loc = target;
            time = Server.GlobalTickTime;
        }

        public override byte[] ToBytes()
        {
            byte[] bytes = new byte[20];
            loc.ToBytes().CopyTo(bytes, 0);
            BitConverter.GetBytes(time).CopyTo(bytes, 12);
            return bytes;
        }
    }
}
