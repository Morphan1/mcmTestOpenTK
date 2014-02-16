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
        [STAThread]
        static void Main(string[] args)
        {
#if !SERVER_ONLY
            if (args.Length > 0 && args[0].ToLower() == "server")
            {
#endif
                // TODO: Server_Main();
                Console.WriteLine("Server!");
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
