using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers.Text;

namespace mcmtestOpenTK.Client.UIHandlers
{
    public class UIConsole
    {
        /// <summary>
        /// Holds the Graphics text object, for rendering.
        /// </summary>
        public static PieceOfText ConsoleText;

        /// <summary>
        /// How many lines the console should have.
        /// </summary>
        public static int Lines = 100;

        /// <summary>
        /// Prepares the console
        /// </summary>
        public static void InitConsole()
        {
            ConsoleText = new PieceOfText(Util.CopyText("\n", Lines), new Point(0, 0));
        }

        /// <summary>
        /// Writes text to the console.
        /// </summary>
        /// <param name="text">The text to be written</param>
        public static void WriteLine(string text)
        {
        }

        /// <summary>
        /// Renders the console.
        /// </summary>
        public static void Draw()
        {
        }
    }
}
