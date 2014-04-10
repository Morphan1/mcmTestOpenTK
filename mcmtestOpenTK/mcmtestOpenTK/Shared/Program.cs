using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
#if !SERVER_ONLY
using mcmtestOpenTK.Client.GlobalHandler;
#endif
using mcmtestOpenTK.ServerSystem.GlobalHandlers;

namespace mcmtestOpenTK.Shared
{
    class Program
    {
        /// <summary>
        /// The name of the game.
        /// </summary>
        public static string Title = "mcmTestOpenTK v0.01";

        /// <summary>
        /// The process the game is running in.
        /// </summary>
        public static Process CurrentProcess = null;

        /// <summary>
        /// A window handle for the console
        /// </summary>
        public static IntPtr ConsoleHandle = IntPtr.Zero;

        /// <summary>
        /// Central entry point for all forms of the program
        /// </summary>
        /// <param name="args">Command line arguments</param>
        [STAThread]
        static void Main(string[] args)
        {
            // Identify the current process.
            CurrentProcess = Process.GetCurrentProcess();
            ConsoleHandle = CurrentProcess.MainWindowHandle;
            // Prepare utilities before doing handling the client/server.
            Utilities.Init();
            List<string> system_arguments = args.ToList();
#if !SERVER_ONLY
            if (args.Length > 0 && args[0].ToLower() == "server")
            {
                SysConsole.Output(OutputType.INIT, "Preparing server (requested from client launch pattern)...");
                system_arguments.RemoveAt(0);
#else
            SysConsole.Output(OutputType.INIT, "Preparing server...");
#endif
            Server.ServerInit(system_arguments);
            SysConsole.Output(OutputType.INFO, "Server ended!");
                return;
#if !SERVER_ONLY
            }
            SysConsole.Output(OutputType.INIT, "Preparing client...");
            MainGame.Client_Main(system_arguments);
            return;
#endif
        }
    }
}
