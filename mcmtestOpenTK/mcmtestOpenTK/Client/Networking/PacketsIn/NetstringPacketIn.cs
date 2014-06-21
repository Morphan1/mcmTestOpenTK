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
    class NetstringPacketIn: AbstractPacketIn
    {
        int ID;

        string text;

        public override void FromBytes(byte[] input)
        {
            if (input.Length > 4)
            {
                IsValid = true;
                ID = BitConverter.ToInt32(input, 0);
                text = FileHandler.encoding.GetString(input, 4, input.Length - 4);
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
            NetStringManager.AddString(ID, text);
        }
    }
}
