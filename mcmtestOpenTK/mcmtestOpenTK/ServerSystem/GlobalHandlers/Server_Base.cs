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
using mcmtestOpenTK.ServerSystem.CommonHandlers;

namespace mcmtestOpenTK.ServerSystem.GlobalHandlers
{
    public partial class Server
    {
        static List<string> CMDArgs;

        /// <summary>
        /// Global entry point, should never be directly called!
        /// </summary>
        /// <param name="args">Command line input args</param>
        public static void ServerInit(List<string> arguments)
        {
            CMDArgs = arguments;
            // Create the data saving thread
            SysConsole.Output(OutputType.INIT, "Starting up save-irrelevant-data thread...");
            Thread datathr = new Thread(new ThreadStart(SaveIrrelevantData));
            datathr.Name = "SaveIrrelevantData";
            Program.ThreadsToClose.Add(datathr);
            datathr.Start();
            SysConsole.Output(OutputType.INIT, "Server starting...");
            if (!ServerLoad())
            {
                return;
            }
            GlobalTickTime = 0;
            SysConsole.Output(OutputType.INIT, "Loaded! Ticking...");
            double TARGETFPS = 20d; // TODO: CVar?
            Stopwatch Counter = new Stopwatch();
            Stopwatch DeltaCounter = new Stopwatch();
            DeltaCounter.Start();
            double TotalDelta = 0;
            double CurrentDelta = 0d;
            double TargetDelta = 0d;
            int targettime = 0;
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
                TARGETFPS = ServerCVar.g_fps.ValueD;
                if (TARGETFPS < 1 || TARGETFPS > 10000)
                {
                    ServerCVar.g_fps.Set("20");
                    TARGETFPS = 20;
                }
                TargetDelta = (1d / TARGETFPS);
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
                targettime = (int)((1000d / TARGETFPS) - Counter.ElapsedMilliseconds);
                // Only sleep at all if we're not lagging
                if (targettime > 0)
                {
                    // Try to sleep for the target time - very imprecise, thus we deal with precision inside the tick code
                    Thread.Sleep(targettime);
                }
            }
        }

        static void SaveIrrelevantData()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(100);
                    if (ConfigStr.Length > 0)
                    {
                        FileHandler.WriteText("serverconfig.cfg", ConfigStr);
                        ConfigStr = "";
                    }
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    SysConsole.Output(OutputType.ERROR, "Error handling irrelevant save data: " + ex.ToString());
                }
            }
        }
    }
}
