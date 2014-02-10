using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using mcmtestOpenTK.GraphicsHandlers;
using mcmtestOpenTK.CommonHandlers;

namespace mcmtestOpenTK.GlobalHandler
{
    public partial class MainGame
    {
        public static string KeyboardString = "";
        public static int KeyboardString_InitBS = 0;

        /// <summary>
        /// Called every time a key is pressed, adds to the Keyboard String.
        /// </summary>
        /// <param name="sender">Irrelevant</param>
        /// <param name="e">Holds the pressed key</param>
        static void PrimaryGameWindow_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (c == 13) // Enter key
            {
                c = '\n';
            }
            else if (c == 8) // Backspace
            {
                if (KeyboardString.Length == 0)
                {
                    KeyboardString_InitBS++;
                }
                else
                {
                    KeyboardString = KeyboardString.Substring(0, KeyboardString.Length - 1);
                }
                return;
            }
            else if (c < 32) // Any other controls
            {
                return;
            }
            Console.WriteLine("Press: " + c + " is " + ((int)c));
            KeyboardString += c.ToString();
        }
    }
}
