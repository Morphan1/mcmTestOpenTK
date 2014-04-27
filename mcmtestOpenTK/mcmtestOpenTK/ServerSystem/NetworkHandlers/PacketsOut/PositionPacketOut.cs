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

        public PositionPacketOut(Entity _entity, Location _position)
        {
            ID = 4;
            entity = _entity;
            position = _position;
        }

        public override byte[] ToBytes()
        {
            byte[] toret = new byte[20];
            BitConverter.GetBytes(entity.UniqueID).CopyTo(toret, 0);
            position.ToBytes().CopyTo(toret, 8);
            return toret;
        }
    }
}
