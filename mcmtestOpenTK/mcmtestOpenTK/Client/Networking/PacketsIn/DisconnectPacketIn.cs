using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.CommandHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.Networking.PacketsOut;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

namespace mcmtestOpenTK.Client.Networking.PacketsIn
{
    class DisconnectPacketIn: AbstractPacketIn
    {
        string reason;

        public override void FromBytes(byte[] input)
        {
            reason = FileHandler.encoding.GetString(input);
            IsValid = true;
        }

        public override void Execute()
        {
            if (!IsValid)
            {
                return;
            }
            UIConsole.WriteLine("Kicked from server: " + reason);
            NetworkBase.Disconnect("kicked");
        }
    }
}
