using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut
{
    public class DespawnPacketOut: AbstractPacketOut
    {
        ulong eID;

        public DespawnPacketOut(ulong _eID)
        {
            ID = 5;
            eID = _eID;
        }

        public override byte[] ToBytes()
        {
            return BitConverter.GetBytes(eID);
        }
    }
}
