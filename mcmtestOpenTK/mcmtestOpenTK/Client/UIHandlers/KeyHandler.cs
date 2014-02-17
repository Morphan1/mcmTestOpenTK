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
    public class KeyHandler
    {
        public static string KeyboardString = "";
        public static int InitBS = 0;
        public static bool ControlDown = false;
        public static bool CopyPressed = false;
        public static bool TogglerPressed = false;

        /// <summary>
        /// Called every time a key is pressed, adds to the Keyboard String.
        /// </summary>
        /// <param name="sender">Irrelevant</param>
        /// <param name="e">Holds the pressed key</param>
        public static void PrimaryGameWindow_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar))
            {
                return;
            }
            if (e.KeyChar == '`' || e.KeyChar == '~')
            {
                TogglerPressed = true;
                return;
            }
            KeyboardString += e.KeyChar;
        }

        /// <summary>
        /// Called every time a key is pressed down, handles control codes for the Keyboard String.
        /// </summary>
        /// <param name="sender">Irrelevant</param>
        /// <param name="e">Holds the pressed key</param>
        public static void PrimaryGameWindow_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    KeyboardString += "\n";
                    break;
                case Key.BackSpace:
                    if (KeyboardString.Length == 0)
                    {
                        InitBS++;
                    }
                    else
                    {
                        KeyboardString = KeyboardString.Substring(0, KeyboardString.Length - 1);
                    }
                    break;
                case Key.ControlLeft:
                case Key.ControlRight:
                    ControlDown = true;
                    break;
                case Key.C:
                    if (ControlDown)
                    {
                        CopyPressed = true;
                    }
                    break;
                case Key.V:
                    if (ControlDown)
                    {
                        KeyboardString += System.Windows.Forms.Clipboard.GetText(System.Windows.Forms.TextDataFormat.Text).Replace('\r', ' ');//.Replace('\n', ' ');
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
        public static void PrimaryGameWindow_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.ControlLeft:
                case Key.ControlRight:
                    ControlDown = false;
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
            InitBS = 0;
            CopyPressed = false;
            TogglerPressed = false;
        }
    }
}
