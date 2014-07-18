using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut
{
    public class PlayerPositionPacketOut: AbstractPacketOut
    {
        Entity entity;
        Location position;
        Location velocity;
        Location direction;
        ushort movement;
        double time;

        // TODO: Separate Position/direction/movement packets?
        public PlayerPositionPacketOut(Entity _entity, Location _position, Location _velocity, Location _direction, ushort _movement)
        {
            ID = 13;
            entity = _entity;
            position = _position;
            velocity = _velocity;
            direction = _direction;
            movement = _movement;
            time = Server.GlobalTickTime;
        }

        public override byte[] ToBytes()
        {
            byte[] toret = new byte[54];
            BitConverter.GetBytes(entity.UniqueID).CopyTo(toret, 0);
            position.ToBytes().CopyTo(toret, 8);
            velocity.ToBytes().CopyTo(toret, 20);
            direction.ToBytes().CopyTo(toret, 32);
            BitConverter.GetBytes(movement).CopyTo(toret, 44);
            BitConverter.GetBytes(time).CopyTo(toret, 46);
            return toret;
        }
    }
}
