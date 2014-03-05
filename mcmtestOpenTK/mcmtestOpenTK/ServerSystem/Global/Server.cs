using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.ServerSystem.Global
{
    public class Server
    {
        /// <summary>
        /// Global entry point, should never be directly called!
        /// </summary>
        /// <param name="args">Command line input args</param>
        public static void ServerInit(List<string> arguments)
        {
            Console.WriteLine("Server!");
            Console.ReadLine();
        }
    }
}
