using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut
{
    class TeleportPacketOut : AbstractPacketOut
    {
        Location loc;
        public TeleportPacketOut(Location target)
        {
            ID = 9;
            loc = target;
        }

        public override byte[] ToBytes()
        {
            return loc.ToBytes();
        }
    }
}
