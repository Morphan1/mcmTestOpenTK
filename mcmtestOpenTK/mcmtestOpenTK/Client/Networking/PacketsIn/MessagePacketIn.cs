using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.CommandHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.Networking.PacketsOut;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.Networking.OneOffs;
using mcmtestOpenTK.Client.UIHandlers;

namespace mcmtestOpenTK.Client.Networking.PacketsIn
{
    class MessagePacketIn: AbstractPacketIn
    {
        public string Message;

        public override void FromBytes(byte[] input)
        {
            Message = FileHandler.encoding.GetString(input);
            IsValid = true;
        }

        public override void Execute()
        {
            if (!IsValid)
            {
                return;
            }
            UIConsole.WriteLine(Message);
        }
    }
}
