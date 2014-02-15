using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using mcmtestOpenTK.CommonHandlers;

namespace mcmtestOpenTK.GlobalHandler
{
    public partial class MainGame
    {

        /// <summary>
        /// Closes the game entirely.
        /// </summary>
        public static void Exit()
        {
            PrimaryGameWindow.Exit();
        }
    }
}
