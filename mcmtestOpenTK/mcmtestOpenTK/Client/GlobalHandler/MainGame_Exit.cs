using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using mcmtestOpenTK.Client.CommonHandlers;
using System.Diagnostics;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    public partial class MainGame
    {
        /// <summary>
        /// Closes the game entirely.
        /// </summary>
        public static void Exit()
        {
            // PrimaryGameWindow.Exit();
            Process.GetCurrentProcess().Kill();
        }
    }
}
