using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut
{
    public class DisconnectPacketOut: AbstractPacketOut
    {
        string Reason;

        public DisconnectPacketOut(string _reason)
        {
            ID = 255;
            Reason = _reason;
        }

        public override byte[] ToBytes()
        {
            return FileHandler.encoding.GetBytes(Reason);
        }
    }
}
