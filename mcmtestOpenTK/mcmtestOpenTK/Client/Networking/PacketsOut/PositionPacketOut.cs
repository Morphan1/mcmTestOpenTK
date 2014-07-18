using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

namespace mcmtestOpenTK.Client.Networking.PacketsOut
{
    public class PositionPacketOut: AbstractPacketOut
    {
        Location Position;
        Location Velocity;
        Location Direction;

        public PositionPacketOut(Location _Position, Location _Velocity, Location _Direction)
        {
            ID = 4;
            Position = _Position;
            Velocity = _Velocity;
            Direction = _Direction;
        }

        public override byte[] ToBytes()
        {
            byte[] ToSend = new byte[36];
            Position.ToBytes().CopyTo(ToSend, 0);
            Velocity.ToBytes().CopyTo(ToSend, 12);
            Direction.ToBytes().CopyTo(ToSend, 24);
            return ToSend;
        }
    }
}
