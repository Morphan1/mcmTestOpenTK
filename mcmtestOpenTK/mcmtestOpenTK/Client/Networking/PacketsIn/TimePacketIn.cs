﻿using System;
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
        double servertime;
        public override void FromBytes(byte[] input)
        {
            if (input.Length == 8)
            {
                servertime = BitConverter.ToDouble(input, 0);
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
            double absd = Math.Abs(servertime - MainGame.GlobalTickTime);
            if (absd < 10 || absd > 3000)
            {
                MainGame.GlobalTickTime = servertime;
            }
            else
            {
                MainGame.GlobalTickTime += (servertime - MainGame.GlobalTickTime) / 10;
            }
        }
    }
}
