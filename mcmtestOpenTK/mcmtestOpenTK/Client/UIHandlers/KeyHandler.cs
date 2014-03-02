﻿using System;
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
using mcmtestOpenTK.Client.UIHandlers;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    public class KeyHandler
    {
        /// <summary>
        /// All text that was written since the information was last retrieved.
        /// </summary>
        public static string KeyboardString = "";

        /// <summary>
        /// How many backspaces were pressed, excluding ones that modified the KeyboardString.
        /// </summary>
        public static int InitBS = 0;

        /// <summary>
        /// Whether the control key is currently down, primarily for internal purposes.
        /// </summary>
        public static bool ControlDown = false;

        /// <summary>
        /// Whether COPY (CTRL+C) was pressed.
        /// </summary>
        public static bool CopyPressed = false;

        /// <summary>
        /// Whether the console toggling key (~) was pressed.
        /// </summary>
        public static bool TogglerPressed = false;

        /// <summary>
        /// The number of times PageUp was pressed minus the number of times PageDown was pressed.
        /// </summary>
        public static int Pages = 0;

        /// <summary>
        /// The number of times the UP arrow was pressed minus the number of times the DOWN arrow was pressed.
        /// </summary>
        public static int Scrolls = 0;

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
                case Key.Tab:
                    KeyboardString += "    ";
                    break;
                case Key.PageUp:
                    Pages++;
                    break;
                case Key.PageDown:
                    Pages--;
                    break;
                case Key.Up:
                    Scrolls++;
                    break;
                case Key.Down:
                    Scrolls--;
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
                case Key.V:
                    if (ControlDown)
                    {
                        string copied;
                        copied = System.Windows.Forms.Clipboard.GetText().Replace('\r', ' ');
                        if (copied.Length > 0 && copied.EndsWith("\n"))
                        {
                            copied = copied.Substring(0, copied.Length - 1);
                        }
                        KeyboardString += copied;
                        for (int i = 0; i < KeyboardString.Length; i++)
                        {
                            if (KeyboardString[i] < 32 && KeyboardString[i] != '\n')
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
            Pages = 0;
            Scrolls = 0;
        }

        public static KeyboardState CurrentKeyboard;
        public static KeyboardState PreviousKeyboard;

        /// <summary>
        /// Updates the keyboard state.
        /// </summary>
        public static void Tick()
        {
            PreviousKeyboard = CurrentKeyboard;
            CurrentKeyboard = Keyboard.GetState();
        }

        /// <summary>
        /// Checks whether the system is listening to keyboard input.
        /// </summary>
        /// <returns>Whether the keyboard is useable</returns>
        public static bool IsValid()
        {
            return MainGame.PrimaryGameWindow.Focused && !UIConsole.Open;
        }

        /// <summary>
        /// Checks whether a key is pressed down.
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>Whether it is down</returns>
        public static bool IsDown(Key key)
        {
            return IsValid() && CurrentKeyboard.IsKeyDown(key);
        }

        /// <summary>
        /// Checks whether a key was just pressed this tick.
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>Whether it was just pressed</returns>
        public static bool IsPressed(Key key)
        {
            return IsValid() && CurrentKeyboard.IsKeyDown(key) && !PreviousKeyboard.IsKeyDown(key);
        }
    }
}
