using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;
using mcmtestOpenTK.ServerSystem.CommonHandlers;
using mcmtestOpenTK.ServerSystem.GameHandlers;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut
{
    public class GiveItemPacketOut: AbstractPacketOut
    {
        Item item;

        public GiveItemPacketOut(Item _item)
        {
            item = _item;
            ID = 15;
        }

        public override byte[] ToBytes()
        {
            // TODO: Slot number
            return item.GetBytes();
        }
    }
}
