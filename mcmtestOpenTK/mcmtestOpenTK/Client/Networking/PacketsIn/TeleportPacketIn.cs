using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GameplayHandlers;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

namespace mcmtestOpenTK.Client.Networking.PacketsIn
{
    class TeleportPacketIn: AbstractPacketIn
    {
        Location loc;
        Location dir;
        double time;

        public override void FromBytes(byte[] input)
        {
            if (input.Length == 32)
            {
                loc = Location.FromBytes(input, 0);
                dir = Location.FromBytes(input, 12);
                time = BitConverter.ToDouble(input, 24);
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }
        }

        public override void Execute()
        {
            if (!MainGame.Spawned)
            {
                MainGame.SetScreen(ScreenMode.Game);
            }
            MainGame.Spawned = true;
            Player.player.Position = loc;
            Player.player.Direction = dir;
            Player.player.Velocity = Location.Zero;
            MainGame.GlobalTickTime = time;
        }
    }
}
