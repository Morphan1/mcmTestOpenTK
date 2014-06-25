using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsIn
{
    class MovementPacketIn : AbstractPacketIn
    {
        double Time;
        byte movement;
        float yaw;
        float pitch;

        public override void FromBytes(Player player, byte[] input)
        {
            if (input.Length != 17)
            {
                IsValid = false;
            }
            else
            {
                Time = BitConverter.ToDouble(input, 0);
                yaw = BitConverter.ToSingle(input, 8);
                pitch = BitConverter.ToSingle(input, 12);
                movement = input[16];
                IsValid = true;
            }
        }

        public override void Execute(Player player)
        {
            if (!IsValid)
            {
                return;
            }
            // TODO: Use time too!
            player.Forward = (movement & 1) == 1;
            player.Back = (movement & 2) == 2;
            player.Left = (movement & 4) == 4;
            player.Right = (movement & 8) == 8;
            player.Up = (movement & 16) == 16;
            player.Down = (movement & 32) == 32;
            player.Direction.X = yaw;
            player.Direction.Y = pitch;
        }
    }
}
