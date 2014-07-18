using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

namespace mcmtestOpenTK.Client.Networking.PacketsOut
{
    public class CommandPacketOut: AbstractPacketOut
    {
        string Arguments;

        public CommandPacketOut(string _arguments)
        {
            ID = 5;
            Arguments = _arguments;
        }

        public override byte[] ToBytes()
        {
            return FileHandler.encoding.GetBytes(Arguments);
        }
    }
}
