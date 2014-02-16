using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.CommonHandlers;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    public partial class MainGame
    {
        public static string KeyboardString = "";
        public static int KeyboardString_InitBS = 0;
        public static bool KeyboardString_ControlDown = false;
        public static bool KeyboardString_CopyPressed = false;

        /// <summary>
        /// Called every time a key is pressed, adds to the Keyboard String.
        /// </summary>
        /// <param name="sender">Irrelevant</param>
        /// <param name="e">Holds the pressed key</param>
        static void PrimaryGameWindow_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar))
            {
                return;
            }
            KeyboardString += e.KeyChar;
        }

        /// <summary>
        /// Called every time a key is pressed down, handles control codes for the Keyboard String.
        /// </summary>
        /// <param name="sender">Irrelevant</param>
        /// <param name="e">Holds the pressed key</param>
        static void PrimaryGameWindow_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    KeyboardString += "\n";
                    break;
                case Key.BackSpace:
                    if (KeyboardString.Length == 0)
                    {
                        KeyboardString_InitBS++;
                    }
                    else
                    {
                        KeyboardString = KeyboardString.Substring(0, KeyboardString.Length - 1);
                    }
                    break;
                case Key.ControlLeft:
                case Key.ControlRight:
                    KeyboardString_ControlDown = true;
                    break;
                case Key.C:
                    if (KeyboardString_ControlDown)
                    {
                        KeyboardString_CopyPressed = true;
                    }
                    break;
                case Key.V:
                    if (KeyboardString_ControlDown)
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
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Called every time a key is released, handles control codes for the Keyboard String.
        /// </summary>
        /// <param name="sender">Irrelevant</param>
        /// <param name="e">Holds the pressed key</param>
        static void PrimaryGameWindow_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.ControlLeft:
                case Key.ControlRight:
                    KeyboardString_ControlDown = false;
                    break;
                default:
                    break;
            }
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
