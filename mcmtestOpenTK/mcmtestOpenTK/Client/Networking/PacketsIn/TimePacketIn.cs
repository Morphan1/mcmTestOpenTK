using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.CommandHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.Networking.PacketsOut;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.Networking.OneOffs;

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
            double timedif = servertime - MainGame.GlobalTickTime;
            if (timedif < -1f)
            {
                MainGame.GlobalTickTime -= 0.1f;
            }
            else if (timedif > 1f)
            {
                MainGame.GlobalTickTime += 0.1f;
            }
            else if (timedif < -0.01f)
            {
                MainGame.GlobalTickTime -= 0.01f;
            }
            else if (timedif > 0.01f)
            {
                MainGame.GlobalTickTime += 0.01f;
            }
        }
    }
}
