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
using mcmtestOpenTK.Client.GraphicsHandlers;

namespace mcmtestOpenTK.Client.Networking.PacketsIn
{
    class YourPositionPacketIn: AbstractPacketIn
    {
        Location position;
        Location velocity;
        double time;
        bool jumped;

        public override void FromBytes(byte[] input)
        {
            if (input.Length != 33)
            {
                IsValid = false;
                return;
            }
            time = BitConverter.ToDouble(input, 0);
            position = Location.FromBytes(input, 8);
            velocity = Location.FromBytes(input, 20);
            jumped = input[32] == 1;
            IsValid = true;
        }

        static Location lastforced;
        
        public override void Execute()
        {
            if (!IsValid)
            {
                return;
            }
            //MainGame.SpawnEntity(new Bullet() { Position = position, LifeTicks = 600, start = lastforced });
            if (time > MainGame.GlobalTickTime)
            {
                // Just ignore.
                return;
            }
            Player.player.ApplyMovement(position, velocity, time, jumped);
            lastforced = position;
        }
    }
}
