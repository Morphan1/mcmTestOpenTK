using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.CommandHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.Networking.PacketsOut;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.Networking.OneOffs;
using mcmtestOpenTK.Client.GameplayHandlers;

namespace mcmtestOpenTK.Client.Networking.PacketsIn
{
    class GiveItemPacketIn: AbstractPacketIn
    {
        Item item;
        public override void FromBytes(byte[] input)
        {
            if (input.Length >= 4 + 4 + 4 + 4 + 4 + 4 + 1 + 4 + 4)
            {
                item = Item.FromBytes(input);
                IsValid = item != null;
            }
            else
            {
                IsValid = false;
            }
        }

        public static Item LastItem = null;

        public override void Execute()
        {
            if (!IsValid)
            {
                return;
            }
            LastItem = item;
        }
    }
}
