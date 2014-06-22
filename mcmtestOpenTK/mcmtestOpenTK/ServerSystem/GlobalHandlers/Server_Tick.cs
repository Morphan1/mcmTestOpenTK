using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.CommandHandlers;
using System.Threading;
using System.Diagnostics;
using mcmtestOpenTK.ServerSystem.NetworkHandlers;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.Global;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut;

namespace mcmtestOpenTK.ServerSystem.GlobalHandlers
{
    public partial class Server
    {
        static int ticknumber = 0;
        static double tickdelta = 0;

        /// <summary>
        /// Tick the entire server.
        /// </summary>
        public static void Tick(double ticktime)
        {
            // Record delta: always first!
            Delta = ticktime;
            DeltaF = (float)Delta;

            // Calculate FPS: always first!
            ticknumber++;
            tickdelta += Delta;
            if (tickdelta >= 1.0f)
            {
                FPS = ticknumber;
                ticknumber = 0;
                tickdelta -= 1.0f;
                GlobalTickNote += 1000;
                GlobalTickTime = GlobalTickNote - (int)(tickdelta * 1000);
            }
            GlobalTickTime += (int)(Delta * 1000);

            // Update global networking
            GlobalNetwork.Tick();

            // Update networking
            NetworkBase.Tick();

            // Update console input
            ConsoleHandler.CheckInput();

            // Update command system
            ServerCommands.Tick();

            // Update the world
            MainWorld.Tick();
            
            // Tell all players what time it is
            NetworkBase.SendToAllPlayers(new TimePacketOut());

            // Update networking again for speed's sake
            NetworkBase.Tick();

        }
    }
}
