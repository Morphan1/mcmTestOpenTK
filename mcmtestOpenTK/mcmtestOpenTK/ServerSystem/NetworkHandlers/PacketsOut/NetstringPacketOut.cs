using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut
{
    public class NetstringPacketOut: AbstractPacketOut
    {
        int NetID;

        string NetText;

        public NetstringPacketOut(int _netID, string _netText)
        {
            ID = 7;
            NetID = _netID;
            NetText = _netText;
        }

        public override byte[] ToBytes()
        {
            byte[] stringdata = FileHandler.encoding.GetBytes(NetText);
            byte[] bytes = new byte[4 + stringdata.Length];
            BitConverter.GetBytes(NetID).CopyTo(bytes, 0);
            stringdata.CopyTo(bytes, 4);
            return bytes;
        }
    }
}
