using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers.Text;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.UIHandlers
{
    public class UIConsole
    {
        /// <summary>
        /// Holds the Graphics text object, for rendering.
        /// </summary>
        public static PieceOfText ConsoleText;

        /// <summary>
        /// Holds the TextRender used for rendering.
        /// </summary>
        public static TextRenderer textrender;

        /// <summary>
        /// How many lines the console should have.
        /// </summary>
        public static int Lines = 100;

        /// <summary>
        /// Any added text, for logging purposes.
        /// </summary>
        public static string NewText = "";

        /// <summary>
        /// Reference for locking the NewText variable.
        /// </summary>
        public static Object NewTextLock = new Object();

        /// <summary>
        /// Prepares the console.
        /// </summary>
        public static void InitConsole()
        {
            ConsoleText = new PieceOfText(Utilities.CopyText("\n", Lines), new Point(0, 0));
            textrender = new TextRenderer(MainGame.ScreenWidth, MainGame.ScreenHeight / 2);
            textrender.AddText(ConsoleText);
        }

        /// <summary>
        /// Writes text to the console.
        /// </summary>
        /// <param name="text">The text to be written</param>
        public static void WriteLine(string text)
        {
            text = text.Replace('\r', ' ');
            lock (NewTextLock)
            {
                NewText += text;
            }
            int lines = Utilities.CountCharacter(text, '\n');
            int linecount = 0;
            int i = 0;
            for (i = 0; i < ConsoleText.Text.Length; i++)
            {
                if (ConsoleText.Text[i] == '\n')
                {
                    linecount++;
                    if (linecount > lines)
                    {
                        break;
                    }
                }
            }
            ConsoleText.Text = ConsoleText.Text.Substring(0, i + 1) + text;
            textrender.modified = true;
        }

        /// <summary>
        /// Renders the console.
        /// </summary>
        public static void Draw()
        {
            textrender.Draw();
        }
    }
}
