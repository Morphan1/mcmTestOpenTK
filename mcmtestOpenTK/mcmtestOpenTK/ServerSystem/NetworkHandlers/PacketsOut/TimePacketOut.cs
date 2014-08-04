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
        bool force;
        public TimePacketOut(bool _force)
        {
            force = _force;
            ID = 8;
        }

        public override byte[] ToBytes()
        {
            byte[] bytes = new byte[9];
            BitConverter.GetBytes(Server.GlobalTickTime).CopyTo(bytes, 0);
            bytes[8] = (byte)(force ? 1: 0);
            return bytes;
        }
    }
}
