using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.CommandHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.Networking.PacketsOut;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.Networking.Global;

namespace mcmtestOpenTK.Client.Networking.PacketsIn
{
    class TimePacketIn: AbstractPacketIn
    {
        long servertime;
        public override void FromBytes(byte[] input)
        {
            if (input.Length == 8)
            {
                servertime = BitConverter.ToInt64(input, 0);
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
            MainGame.GlobalTickNote = servertime;
            MainGame.GlobalTickTime = servertime;
            MainGame.globaltickdelta = 0.0f;
        }
    }
}
