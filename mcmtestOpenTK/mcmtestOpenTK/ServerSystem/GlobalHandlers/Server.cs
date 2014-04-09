using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.CommandHandlers;
using System.Threading;
using System.Diagnostics;

namespace mcmtestOpenTK.ServerSystem.GlobalHandlers
{
    public class Server
    {
        /// <summary>
        /// How many seconds have passed since the last tick.
        /// </summary>
        public static double Delta;

        /// <summary>
        /// How many seconds have passed since the last tick.
        /// </summary>
        public static float DeltaF;

        /// <summary>
        /// Global entry point, should never be directly called!
        /// </summary>
        /// <param name="args">Command line input args</param>
        public static void ServerInit(List<string> arguments)
        {
            SysConsole.Output(OutputType.INIT, "Server starting...");
            SysConsole.Output(OutputType.INIT, "Preparing command system...");
            ServerCommands.Init();
            SysConsole.Output(OutputType.INIT, "Preparing console listener...");
            ConsoleHandler.Init();
            int TARGETFPS = 20; // TODO: CVar?
            Stopwatch Counter = new Stopwatch();
            Stopwatch DeltaCounter = new Stopwatch();
            DeltaCounter.Start();
            while (true)
            {
                Counter.Reset();
                Counter.Start();
                DeltaCounter.Stop();
                Tick(((double)DeltaCounter.ElapsedTicks) / ((double)Stopwatch.Frequency));
                DeltaCounter.Reset();
                DeltaCounter.Start();
                Counter.Stop();
                int ms = (int)Counter.ElapsedMilliseconds;
                int targettime = (1000 / TARGETFPS) - ms;
                if (targettime > 0)
                {
                    Thread.Sleep(targettime);
                }
            }
        }

        /// <summary>
        /// Tick the entire server.
        /// </summary>
        public static void Tick(double ticktime)
        {
            Delta = ticktime;
            DeltaF = (float)Delta;
            ConsoleHandler.CheckInput();
            ServerCommands.Tick();
        }
    }
}
