using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.CommandHandlers;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.Networking.PacketsIn
{
    class HelloPacketIn: AbstractPacketIn
    {
        public override void FromBytes(byte[] input)
        {
            SysConsole.Output(OutputType.CLIENTINFO, "Got " + input.Length + " bytes: " + Encoding.ASCII.GetString(input));
            if (input.Length == 5 && input[0] == (byte)'H' &&
                input[1] == (byte)'E' && input[2] == (byte)'L' && input[3] == (byte)'L' && input[4] == (byte)'O')
            {
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
            ClientCommands.CommandSystem.Output.Bad("Server sent HELLO!");
            // TODO
        }
    }
}
