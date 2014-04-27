using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.ServerSystem.CommonHandlers;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.Global;
using mcmtestOpenTK.ServerSystem.GameHandlers.GameHelpers;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsIn
{
    class PositionPacketIn: AbstractPacketIn
    {
        Location Position;

        public override void FromBytes(Player player, byte[] input)
        {
            if (input.Length != 12)
            {
                IsValid = false;
                return;
            }
            float X = BitConverter.ToSingle(input, 0);
            float Y = BitConverter.ToSingle(input, 4);
            float Z = BitConverter.ToSingle(input, 8);
            Position = new Location(X, Y, Z);
            IsValid = true;
        }

        public override void Execute(Player player)
        {
            if (!IsValid)
            {
                return;
            }
            // TODO: Confirm validity, etc.
            player.Position = Position;
        }
    }
}
