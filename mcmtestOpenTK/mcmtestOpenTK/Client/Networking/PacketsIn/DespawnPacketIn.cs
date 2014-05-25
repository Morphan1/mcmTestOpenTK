using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.CommandHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.Networking.PacketsOut;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.GameplayHandlers.Entities;

namespace mcmtestOpenTK.Client.Networking.PacketsIn
{
    class DespawnPacketIn: AbstractPacketIn
    {
        ulong id;

        public override void FromBytes(byte[] input)
        {
            if (input.Length != 8)
            {
                IsValid = false;
                return;
            }
            id = BitConverter.ToUInt64(input, 0);
            IsValid = true;
        }

        public override void Execute()
        {
            if (!IsValid)
            {
                return;
            }
            Entity e = MainGame.GetEntity(id);
            if (e == null)
            {
                UIConsole.WriteLine("Tried and failed to remove entity " + id);
            }
            MainGame.Destroy(e);
        }
    }
}
