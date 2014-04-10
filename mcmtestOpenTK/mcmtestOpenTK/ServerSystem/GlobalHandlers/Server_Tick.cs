using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.CommandHandlers;
using System.Threading;
using System.Diagnostics;
using mcmtestOpenTK.ServerSystem.NetworkHandlers;

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
            NetworkBase.Tick();
            ConsoleHandler.CheckInput();
            ServerCommands.Tick();
        }
    }
}
