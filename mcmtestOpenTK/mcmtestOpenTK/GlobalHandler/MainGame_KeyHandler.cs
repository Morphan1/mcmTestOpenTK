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
        public static bool KeyboardString_CopyPressed = false;

        /// <summary>
        /// Called every time a key is pressed, adds to the Keyboard String.
        /// </summary>
        /// <param name="sender">Irrelevant</param>
        /// <param name="e">Holds the pressed key</param>
        static void PrimaryGameWindow_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            Console.WriteLine("Press: " + (c == '\a' ? "\a": c.ToString()) + " is " + ((int)c));
            if (c == 13) // Enter key
            {
                c = '\n';
            }
            else if (c == 22) // CTRL-V (Paste)
            {
                KeyboardString += System.Windows.Forms.Clipboard.GetText(System.Windows.Forms.TextDataFormat.Text).Replace('\r', ' ').Replace('\n', ' ');
                for (int i = 0; i < KeyboardString.Length; i++)
                {
                    if (KeyboardString[i] < 32)
                    {
                        KeyboardString = KeyboardString.Substring(0, i) + KeyboardString.Substring(i + 1, KeyboardString.Length - (i + 1));
                        i--;
                    }
                }
            }
            else if (c == 3) // CTRL-C (Copy)
            {
                KeyboardString_CopyPressed = true;
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
            KeyboardString += c.ToString();
        }

        /// <summary>
        /// Clears all recorded KeyboardString information.
        /// </summary>
        public static void Clear()
        {
            KeyboardString = "";
            KeyboardString_InitBS = 0;
            KeyboardString_CopyPressed = false;
        }
    }
}
