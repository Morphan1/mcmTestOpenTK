using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.Client.Networking.PacketsOut
{
    class MovementPacketOut : AbstractPacketOut
    {
        public static ushort GetControlShort(bool Forward, bool Back, bool Left, bool Right, bool Up, bool Down, bool Slow)
        {
            return (ushort)((Forward ? 1 : 0) | (Back ? 2 : 0) | (Left ? 4 : 0) | (Right ? 8 : 0) | (Up ? 16 : 0) | (Down ? 32 : 0) | (Slow ? 64: 0));
        }

        double Time;
        ushort Movement;
        float dir;
        float dirpitch;

        public MovementPacketOut(double _time, ushort controlshort, float yaw, float pitch)
        {
            ID = 4;
            Time = _time;
            Movement = controlshort;
            dir = yaw;
            dirpitch = pitch;
        }

        public override byte[] ToBytes()
        {
            byte[] tosend = new byte[18];
            BitConverter.GetBytes(Time).CopyTo(tosend, 0);
            BitConverter.GetBytes(dir).CopyTo(tosend, 8);
            BitConverter.GetBytes(dirpitch).CopyTo(tosend, 12);
            BitConverter.GetBytes(Movement).CopyTo(tosend, 16);
            return tosend;
        }
    }
}
