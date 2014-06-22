using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.CommandHandlers;
using System.Threading;
using System.Diagnostics;
using mcmtestOpenTK.ServerSystem.GameHandlers;

namespace mcmtestOpenTK.ServerSystem.GlobalHandlers
{
    public partial class Server
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
        /// How many ticks pass in one second.
        /// </summary>
        public static int FPS;

        /// <summary>
        /// The primary server world.
        /// </summary>
        public static World MainWorld;

        /// <summary>
        /// The current global time.
        /// </summary>
        public static long GlobalTickTime;

        static long GlobalTickNote;
    }
}
