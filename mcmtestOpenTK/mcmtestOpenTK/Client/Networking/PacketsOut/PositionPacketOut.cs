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
        Vector3 Velocity;
        Vector3 Direction;

        public PositionPacketOut(Vector3 _Position, Vector3 _Velocity, Vector3 _Direction)
        {
            ID = 4;
            Position = _Position;
            Velocity = _Velocity;
            Direction = _Direction;
        }

        public override byte[] ToBytes()
        {
            byte[] ToSend = new byte[36];
            BitConverter.GetBytes(Position.X).CopyTo(ToSend, 0);
            BitConverter.GetBytes(Position.Y).CopyTo(ToSend, 4);
            BitConverter.GetBytes(Position.Z).CopyTo(ToSend, 8);
            BitConverter.GetBytes(Velocity.X).CopyTo(ToSend, 12);
            BitConverter.GetBytes(Velocity.Y).CopyTo(ToSend, 16);
            BitConverter.GetBytes(Velocity.Z).CopyTo(ToSend, 20);
            BitConverter.GetBytes(Direction.X).CopyTo(ToSend, 24);
            BitConverter.GetBytes(Direction.Y).CopyTo(ToSend, 28);
            BitConverter.GetBytes(Direction.Z).CopyTo(ToSend, 32);
            return ToSend;
        }
    }
}
