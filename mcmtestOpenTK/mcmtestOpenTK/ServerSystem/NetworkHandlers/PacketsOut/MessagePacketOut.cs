using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;
using mcmtestOpenTK.ServerSystem.CommonHandlers;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut
{
    public class MessagePacketOut: AbstractPacketOut
    {
        public string Message;

        public MessagePacketOut(string _message)
        {
            ID = 6;
            Message = _message;
        }

        public override byte[] ToBytes()
        {
            return FileHandler.encoding.GetBytes(Message);
        }
    }
}
