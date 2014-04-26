using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut
{
    public class DisconnectPacketOut: AbstractPacketOut
    {
        Player player;

        string Reason;

        public DisconnectPacketOut(Player _player, string _reason)
        {
            ID = 255;
            player = _player;
            Reason = _reason;
        }

        public override byte[] ToBytes()
        {
            return FileHandler.encoding.GetBytes(Reason);
        }
    }
}
