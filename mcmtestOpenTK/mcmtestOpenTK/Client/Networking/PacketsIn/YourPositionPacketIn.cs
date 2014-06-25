using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.CommandHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.Networking.PacketsOut;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.GameplayHandlers;
using mcmtestOpenTK.Client.GameplayHandlers.Entities;

namespace mcmtestOpenTK.Client.Networking.PacketsIn
{
    class YourPositionPacketIn: AbstractPacketIn
    {
        Location position;
        Location velocity;
        double time;

        public override void FromBytes(byte[] input)
        {
            if (input.Length != 32)
            {
                IsValid = false;
                return;
            }
            time = BitConverter.ToDouble(input, 0);
            position = Location.FromBytes(input, 8);
            velocity = Location.FromBytes(input, 20);
            IsValid = true;
        }

        public override void Execute()
        {
            if (!IsValid)
            {
                return;
            }
            if (time > MainGame.GlobalTickTime)
            {
                // Just ignore.
                return;
            }
            // Player.player.ApplyMovement(position, velocity, time);
            MainGame.SpawnEntity(new Bullet() { Position = position, Solid = false });
        }
    }
}
