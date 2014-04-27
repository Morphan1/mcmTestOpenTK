using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.CommandHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.Networking.PacketsOut;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.UIHandlers;
using OpenTK;

namespace mcmtestOpenTK.Client.Networking.PacketsIn
{
    class SpawnPacketIn: AbstractPacketIn
    {
        EntityType type;
        Vector3 position;
        ulong id;

        public override void FromBytes(byte[] input)
        {
            if (input.Length != 21)
            {
                IsValid = false;
                return;
            }
            type = (EntityType)input[0];
            id = BitConverter.ToUInt64(input, 1);
            float X = BitConverter.ToSingle(input, 9);
            float Y = BitConverter.ToSingle(input, 13);
            float Z = BitConverter.ToSingle(input, 17);
            position = new Vector3(X, Y, Z);
            IsValid = true;
        }

        public override void Execute()
        {
            if (!IsValid)
            {
                return;
            }
            MainGame.SpawnEntity(type, id, position);
        }
    }
}
