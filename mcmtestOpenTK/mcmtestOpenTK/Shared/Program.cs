using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if !SERVER_ONLY
using mcmtestOpenTK.Client.GlobalHandler;
#endif
using mcmtestOpenTK.ServerSystem.Global;

namespace mcmtestOpenTK.Shared
{
    class Program
    {
        /// <summary>
        /// Central entry point for all forms of the program
        /// </summary>
        /// <param name="args">Command line arguments</param>
        [STAThread]
        static void Main(string[] args)
        {
            // Prepare utilities before doing anything else!
            Utilities.Init();
            List<string> system_arguments = args.ToList();
#if !SERVER_ONLY
            if (args.Length > 0 && args[0].ToLower() == "server")
            {
#endif
                system_arguments.RemoveAt(0);
                Server.ServerInit(system_arguments);
                return;
#if !SERVER_ONLY
            }
            MainGame.Client_Main(system_arguments);
            return;
#endif
        }
    }
}
