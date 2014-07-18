using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut
{
    class TeleportPacketOut : AbstractPacketOut
    {
        Location loc;
        Location dir;
        double time;
        public TeleportPacketOut(Location target, Location direction)
        {
            ID = 9;
            loc = target;
            dir = direction;
            time = Server.GlobalTickTime;
        }

        public override byte[] ToBytes()
        {
            byte[] bytes = new byte[32];
            loc.ToBytes().CopyTo(bytes, 0);
            dir.ToBytes().CopyTo(bytes, 12);
            BitConverter.GetBytes(time).CopyTo(bytes, 24);
            return bytes;
        }
    }
}
