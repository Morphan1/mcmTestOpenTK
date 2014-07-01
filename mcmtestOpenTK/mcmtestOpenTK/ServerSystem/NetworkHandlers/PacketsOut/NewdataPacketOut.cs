using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut
{
    public class NewdataPacketOut: AbstractPacketOut
    {
        Entity entity;
        byte[] data;

        // TODO: Separate Position/direction/movement packets?
        public NewdataPacketOut(Entity _entity, byte[] _data)
        {
            ID = 14;
            entity = _entity;
            data = _data;
        }

        public override byte[] ToBytes()
        {
            byte[] toret = new byte[8 + data.Length];
            BitConverter.GetBytes(entity.UniqueID).CopyTo(toret, 0);
            data.CopyTo(toret, 8);
            return toret;
        }
    }
}
