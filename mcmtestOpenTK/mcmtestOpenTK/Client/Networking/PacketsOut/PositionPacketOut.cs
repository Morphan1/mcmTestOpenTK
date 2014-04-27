using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using OpenTK;

namespace mcmtestOpenTK.Client.Networking.PacketsOut
{
    public class PositionPacketOut: AbstractPacketOut
    {
        Vector3 Position;

        public PositionPacketOut(Vector3 _Position)
        {
            ID = 4;
            Position = _Position;
        }

        public override byte[] ToBytes()
        {
            byte[] ToSend = new byte[12];
            BitConverter.GetBytes(Position.X).CopyTo(ToSend, 0);
            BitConverter.GetBytes(Position.Y).CopyTo(ToSend, 4);
            BitConverter.GetBytes(Position.Z).CopyTo(ToSend, 8);
            return ToSend;
        }
    }
}
