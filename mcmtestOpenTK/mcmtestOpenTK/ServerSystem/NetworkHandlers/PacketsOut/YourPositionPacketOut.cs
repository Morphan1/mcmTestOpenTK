using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut
{
    public class YourPositionPacketOut: AbstractPacketOut
    {
        double Time;
        Location position;
        Location velocity;

        public YourPositionPacketOut(double _time, Location _position, Location _velocity)
        {
            ID = 10;
            Time = _time;
            position = _position;
            velocity = _velocity;
        }

        public override byte[] ToBytes()
        {
            byte[] toret = new byte[32];
            BitConverter.GetBytes(Time).CopyTo(toret, 0);
            position.ToBytes().CopyTo(toret, 8);
            velocity.ToBytes().CopyTo(toret, 20);
            return toret;
        }
    }
}
