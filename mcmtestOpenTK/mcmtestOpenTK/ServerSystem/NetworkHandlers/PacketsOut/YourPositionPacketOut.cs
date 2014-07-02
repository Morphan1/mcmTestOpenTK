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
        bool jumped;

        public YourPositionPacketOut(double _time, Location _position, Location _velocity, bool _jumped)
        {
            ID = 10;
            Time = _time;
            position = _position;
            velocity = _velocity;
            jumped = _jumped;
        }

        public override byte[] ToBytes()
        {
            byte[] toret = new byte[33];
            BitConverter.GetBytes(Time).CopyTo(toret, 0);
            position.ToBytes().CopyTo(toret, 8);
            velocity.ToBytes().CopyTo(toret, 20);
            toret[32] = jumped ? (byte)1 : (byte)0;
            return toret;
        }
    }
}
