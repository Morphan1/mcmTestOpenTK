using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;
using mcmtestOpenTK.ServerSystem.CommonHandlers;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut
{
    public class TimePacketOut: AbstractPacketOut
    {
        public TimePacketOut()
        {
            ID = 8;
        }

        public override byte[] ToBytes()
        {
            return BitConverter.GetBytes(Server.GlobalTickTime);
        }
    }
}
