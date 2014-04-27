using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut
{
    public class PingPacketOut: AbstractPacketOut
    {
        byte PingID;

        Player player;

        public PingPacketOut(Player _player)
        {
            ID = 2;
            PingID = (byte)Utilities.random.Next(256);
            player = _player;
        }

        public override byte[] ToBytes()
        {
            player.PingID = PingID;
            return new byte[5] { (byte)'P', (byte)'I', (byte)'N', (byte)'G', PingID };
        }
    }
}
