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
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsIn
{
    class PositionPacketIn: AbstractPacketIn
    {
        Location Position;
        Location Velocity;
        Location Direction;

        public override void FromBytes(Player player, byte[] input)
        {
            if (input.Length != 36)
            {
                IsValid = false;
                return;
            }
            Position = Location.FromBytes(input, 0);
            Velocity = Location.FromBytes(input, 12);
            Direction = Location.FromBytes(input, 24);
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
            player.Velocity = Velocity;
            player.Direction = Direction;
        }
    }
}
