using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GameplayHandlers;
using mcmtestOpenTK.Client.GlobalHandler;

namespace mcmtestOpenTK.Client.Networking.PacketsIn
{
    class TeleportPacketIn: AbstractPacketIn
    {
        Location loc;

        public override void FromBytes(byte[] input)
        {
            if (input.Length == 12)
            {
                loc = Location.FromBytes(input, 0);
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }
        }

        public override void Execute()
        {
            MainGame.Spawned = true;
            Player.player.Position = loc;
            Player.player.Velocity = Location.Zero;
        }
    }
}
