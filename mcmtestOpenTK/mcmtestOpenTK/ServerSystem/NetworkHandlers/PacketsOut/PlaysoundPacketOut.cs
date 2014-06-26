using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;
using mcmtestOpenTK.ServerSystem.CommonHandlers;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut
{
    public class PlaysoundPacketOut : AbstractPacketOut
    {
        string sound;

        public PlaysoundPacketOut(string _sound)
        {
            ID = 12;
            sound = _sound;
        }

        public override byte[] ToBytes()
        {
            return BitConverter.GetBytes(NetStringManager.GetStringID(sound));
        }
    }
}
