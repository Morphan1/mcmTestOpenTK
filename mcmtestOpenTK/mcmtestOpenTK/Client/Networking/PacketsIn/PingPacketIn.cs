using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.CommandHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.Networking.PacketsOut;
using mcmtestOpenTK.Client.GlobalHandler;

namespace mcmtestOpenTK.Client.Networking.PacketsIn
{
    class PingPacketIn: AbstractPacketIn
    {
        byte ID;

        public override void FromBytes(byte[] input)
        {
            if (input.Length == 5 && input[0] == (byte)'P' &&
                input[1] == (byte)'I' && input[2] == (byte)'N' && input[3] == (byte)'G')
            {
                ID = input[4];
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }
        }

        public override void Execute()
        {
            if (!IsValid)
            {
                return;
            }
            NetworkBase.IsActive = true;
            NetworkBase.Ping = NetworkBase.pingbump;
            NetworkBase.pingbump = 0;
            NetworkBase.Send(new PingPacketOut(ID));
        }
    }
}
