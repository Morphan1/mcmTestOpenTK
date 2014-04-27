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
            if (tickdelta > 1.0f)
            {
                FPS = ticknumber;
                ticknumber = 0;
                tickdelta = 0.0f;
            }

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
        }
    }
}
