using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;
using mcmtestOpenTK.ServerSystem.CommonHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut
{
    public class SetcvarPacketOut : AbstractPacketOut
    {
        string CVar;
        string Value;

        public SetcvarPacketOut(string _cvar, string _value)
        {
            ID = 11;
            CVar = _cvar;
            Value = _value;
        }

        public override byte[] ToBytes()
        {
            byte[] text = FileHandler.encoding.GetBytes(Value);
            byte[] toret = new byte[text.Length + 4];
            BitConverter.GetBytes(NetStringManager.GetStringID(CVar)).CopyTo(toret, 0);
            text.CopyTo(toret, 4);
            return toret;
        }
    }
}
