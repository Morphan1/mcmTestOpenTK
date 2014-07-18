using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.CommandHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.Networking.PacketsOut;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

namespace mcmtestOpenTK.Client.Networking.PacketsIn
{
    class SpawnPacketIn: AbstractPacketIn
    {
        EntityType type;
        Location position;
        ulong id;
        byte[] LeftOver;

        public override void FromBytes(byte[] input)
        {
            if (input.Length < 21)
            {
                IsValid = false;
                return;
            }
            type = (EntityType)input[0];
            id = BitConverter.ToUInt64(input, 1);
            position = Location.FromBytes(input, 9);
            LeftOver = new byte[input.Length - 21];
            if (LeftOver.Length > 0)
            {
                Array.Copy(input, 21, LeftOver, 0, input.Length - 21);
            }
            IsValid = true;
        }

        public override void Execute()
        {
            if (!IsValid)
            {
                return;
            }
            MainGame.SpawnEntity(type, id, position, LeftOver);
        }
    }
}
