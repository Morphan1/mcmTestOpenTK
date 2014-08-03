using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
#if !SERVER_ONLY
using mcmtestOpenTK.Client.GlobalHandler;
#endif
using mcmtestOpenTK.ServerSystem.GlobalHandlers;
using System.Threading;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

namespace mcmtestOpenTK.Shared
{
    class Program
    {
        /// <summary>
        /// The name of the game.
        /// </summary>
        public static string Title = "mcmTestOpenTK v0.02";

        /// <summary>
        /// The process the game is running in.
        /// </summary>
        public static Process CurrentProcess = null;

        /// <summary>
        /// A window handle for the console
        /// </summary>
        public static IntPtr ConsoleHandle = IntPtr.Zero;

        public static List<Thread> ThreadsToClose = new List<Thread>();

        public static bool ClientActive = false;

        public static Thread MainThread;

        /// <summary>
        /// Central entry point for all forms of the program
        /// </summary>
        /// <param name="args">Command line arguments</param>
        [STAThread]
        static void Main(string[] args)
        {
            // Make a shutdown hook
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            // Identify the current process.
            CurrentProcess = Process.GetCurrentProcess();
            MainThread = Thread.CurrentThread;
            ConsoleHandle = CurrentProcess.MainWindowHandle;
            // Prepare utilities before doing handling the client/server.
            Utilities.Init();
            List<string> system_arguments = args.ToList();
#if !SERVER_ONLY
            if (args.Length > 0 && args[0].ToLower() == "server")
#endif
            {
#if !SERVER_ONLY
                SysConsole.Output(OutputType.INIT, "Preparing server (requested from client launch pattern)...");
                system_arguments.RemoveAt(0);
#else
                SysConsole.Output(OutputType.INIT, "Preparing server...");
#endif
                Server.ServerInit(system_arguments);
                SysConsole.Output(OutputType.INFO, "Server ended!");
                return;
            }
#if !SERVER_ONLY
            SysConsole.Output(OutputType.INIT, "Preparing client...");
            ClientActive = true;
            MainGame.Client_Main(system_arguments);
            return;
#endif
        }

        public static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            while (ThreadsToClose.Count > 0)
            {
                try
                {
                    ThreadsToClose[0].Abort();
                }
                catch
                {
                    // Ignore
                }
                ThreadsToClose.RemoveAt(0);
            }
        }
    }
}
