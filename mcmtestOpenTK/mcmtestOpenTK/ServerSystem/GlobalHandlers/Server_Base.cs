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
    public partial class Server
    {
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
            double TotalDelta = 0;
            double CurrentDelta = 0d;
            double TargetDelta = 0d;
            int targettime = 0;
            SysConsole.Output(OutputType.INIT, "Loaded! Ticking...");
            while (true)
            {
                // Update the tick time usage counter
                Counter.Reset();
                Counter.Start();
                // Update the tick delta counter
                DeltaCounter.Stop();
                // Delta time = Elapsed ticks * (ticks/second)
                CurrentDelta = ((double)DeltaCounter.ElapsedTicks) / ((double)Stopwatch.Frequency);
                // How much time should pass between each tick ideally
                TargetDelta = (1d / (double)TARGETFPS);
                // How much delta has been built up
                TotalDelta += CurrentDelta;
                if (TotalDelta > TargetDelta * 10)
                {
                    // Lagging - cheat to catch up!
                    TargetDelta *= 3;
                }
                if (TotalDelta > TargetDelta * 10)
                {
                    // Lagging a /lot/ - cheat /extra/ to catch up!
                    TargetDelta *= 3;
                }
                // As long as there's more delta built up than delta wanted, tick
                while (TotalDelta > TargetDelta)
                {
                    Tick(TargetDelta);
                    TotalDelta -= TargetDelta;
                }
                // Begin the delta counter to find out how much time is /really/ slept for
                DeltaCounter.Reset();
                DeltaCounter.Start();
                // The tick is done, stop measuring it
                Counter.Stop();
                // Only sleep for target milliseconds/tick minus how long the tick took... this is imprecise but that's okay
                targettime = (1000 / TARGETFPS) - (int)Counter.ElapsedMilliseconds;
                // Only sleep at all if we're not lagging
                if (targettime > 0)
                {
                    // Try to sleep for the target time - very imprecise, thus we deal with precision inside the tick code
                    Thread.Sleep(targettime);
                }
            }
        }
    }
}
