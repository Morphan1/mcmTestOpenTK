using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if !SERVER_ONLY
using mcmtestOpenTK.Client.GlobalHandler;
#endif

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
#if !SERVER_ONLY
            if (args.Length > 0 && args[0].ToLower() == "server")
            {
#endif
                // TODO: Server_Main();
                Console.WriteLine("Server! Hello!");
                Console.ReadLine();
                return;
#if !SERVER_ONLY
            }
            MainGame.Client_Main(args);
            return;
#endif
        }
    }
}
