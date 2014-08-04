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
        bool force;
        public override void FromBytes(byte[] input)
        {
            if (input.Length == 9)
            {
                servertime = BitConverter.ToDouble(input, 0);
                force = input[8] == 1;
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
            if (force)
            {
                MainGame.GlobalTickTime = servertime;
                return;
            }
            double timedif = servertime - MainGame.GlobalTickTime;
            if (timedif < -5f || timedif > 5f)
            {
                MainGame.GlobalTickTime = servertime;
            }
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
