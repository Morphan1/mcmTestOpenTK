using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.GameHandlers.GameHelpers;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut
{
    public class PositionPacketOut: AbstractPacketOut
    {
        Entity entity;
        Location position;
        Location velocity;
        Location direction;

        // TODO: Separate Position/direction/movement packets?
        public PositionPacketOut(Entity _entity, Location _position, Location _velocity, Location _direction)
        {
            ID = 4;
            entity = _entity;
            position = _position;
            velocity = _velocity;
            direction = _direction;
        }

        public override byte[] ToBytes()
        {
            byte[] toret = new byte[44];
            BitConverter.GetBytes(entity.UniqueID).CopyTo(toret, 0);
            position.ToBytes().CopyTo(toret, 8);
            velocity.ToBytes().CopyTo(toret, 20);
            direction.ToBytes().CopyTo(toret, 32);
            return toret;
        }
    }
}
