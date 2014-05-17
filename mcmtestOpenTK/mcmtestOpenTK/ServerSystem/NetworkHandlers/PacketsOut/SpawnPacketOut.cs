using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.GameHandlers.GameHelpers;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut
{
    public class SpawnPacketOut: AbstractPacketOut
    {
        Entity ent;

        public SpawnPacketOut(Entity _ent)
        {
            ID = 3;
            ent = _ent;
        }

        public override byte[] ToBytes()
        {
            byte[] entdata = ent.GetData();
            byte[] toret = new byte[21 + entdata.Length];
            toret[0] = (byte)ent.Type;
            BitConverter.GetBytes(ent.UniqueID).CopyTo(toret, 1);
            ent.Position.ToBytes().CopyTo(toret, 9);
            if (entdata.Length > 0)
            {
                entdata.CopyTo(toret, 21);
            }
            return toret;
        }
    }
}
