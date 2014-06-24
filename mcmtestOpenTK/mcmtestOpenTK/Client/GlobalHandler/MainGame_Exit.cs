using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using mcmtestOpenTK.Client.CommonHandlers;
using System.Diagnostics;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    public partial class MainGame
    {
        static void PrimaryGameWindow_Closed(object sender, EventArgs e)
        {
            Program.CurrentDomain_ProcessExit(null, null);
        }

        /// <summary>
        /// Closes the game entirely.
        /// </summary>
        public static void Exit()
        {
            PrimaryGameWindow.Exit();
            // Environment.Exit(0);
            // Process.GetCurrentProcess().Kill();
        }
    }
}
