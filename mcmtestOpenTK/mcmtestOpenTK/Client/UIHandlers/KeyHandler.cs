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
using mcmtestOpenTK.Client.UIHandlers;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    public class KeyHandler
    {
        /// <summary>
        /// All text that was written since the information was last retrieved.
        /// </summary>
        public static string KeyboardString = "";
        static string _KeyboardString = "";

        /// <summary>
        /// How many backspaces were pressed, excluding ones that modified the KeyboardString.
        /// </summary>
        public static int InitBS = 0;
        static int _InitBS = 0;

        /// <summary>
        /// Whether the control key is currently down, primarily for internal purposes.
        /// </summary>
        public static bool ControlDown = false;
        static bool _ControlDown = false;

        /// <summary>
        /// Whether COPY (CTRL+C) was pressed.
        /// </summary>
        public static bool CopyPressed = false;
        static bool _CopyPressed = false;

        /// <summary>
        /// Whether the console toggling key (~) was pressed.
        /// </summary>
        public static bool TogglerPressed = false;
        static bool _TogglerPressed = false;

        /// <summary>
        /// The number of times PageUp was pressed minus the number of times PageDown was pressed.
        /// </summary>
        public static int Pages = 0;
        static int _Pages = 0;

        /// <summary>
        /// The number of times the UP arrow was pressed minus the number of times the DOWN arrow was pressed.
        /// </summary>
        public static int Scrolls = 0;
        static int _Scrolls = 0;

        /// <summary>
        /// The number of times the RIGHT arrow was pressed minus the number of times the LEFT arrow was pressed.
        /// </summary>
        public static int LeftRights = 0;
        static int _LeftRights = 0;

        static Object Locker = new Object();

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
            lock (Locker)
            {
                if (e.KeyChar == '`' || e.KeyChar == '~')
                {
                    _TogglerPressed = true;
                    return;
                }
                _KeyboardString += e.KeyChar;
            }
        }

        /// <summary>
        /// Called every time a key is pressed down, handles control codes for the Keyboard String.
        /// </summary>
        /// <param name="sender">Irrelevant</param>
        /// <param name="e">Holds the pressed key</param>
        public static void PrimaryGameWindow_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            lock (Locker)
            {
                switch (e.Key)
                {
                    case Key.Enter:
                        _KeyboardString += "\n";
                        break;
                    case Key.Tab:
                        _KeyboardString += "    ";
                        break;
                    case Key.PageUp:
                        _Pages++;
                        break;
                    case Key.PageDown:
                        _Pages--;
                        break;
                    case Key.Up:
                        _Scrolls++;
                        break;
                    case Key.Down:
                        _Scrolls--;
                        break;
                    case Key.Left:
                        _LeftRights--;
                        break;
                    case Key.Right:
                        _LeftRights++;
                        break;
                    case Key.End:
                        _LeftRights = 9000;
                        break;
                    case Key.Home:
                        _LeftRights = -9000;
                        break;
                    case Key.ControlLeft:
                    case Key.ControlRight:
                        _ControlDown = true;
                        break;
                    case Key.C:
                        if (_ControlDown)
                        {
                            _CopyPressed = true;
                        }
                        break;
                    case Key.BackSpace:
                        if (_KeyboardString.Length == 0)
                        {
                            _InitBS++;
                        }
                        else
                        {
                            _KeyboardString = _KeyboardString.Substring(0, _KeyboardString.Length - 1);
                        }
                        break;
                    case Key.V:
                        if (_ControlDown)
                        {
                            string copied;
                            copied = System.Windows.Forms.Clipboard.GetText().Replace('\r', ' ');
                            if (copied.Length > 0 && copied.EndsWith("\n"))
                            {
                                copied = copied.Substring(0, copied.Length - 1);
                            }
                            _KeyboardString += copied;
                            for (int i = 0; i < _KeyboardString.Length; i++)
                            {
                                if (_KeyboardString[i] < 32 && _KeyboardString[i] != '\n')
                                {
                                    _KeyboardString = _KeyboardString.Substring(0, i) +
                                        _KeyboardString.Substring(i + 1, _KeyboardString.Length - (i + 1));
                                    i--;
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Called every time a key is released, handles control codes for the Keyboard String.
        /// </summary>
        /// <param name="sender">Irrelevant</param>
        /// <param name="e">Holds the pressed key</param>
        public static void PrimaryGameWindow_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            lock (Locker)
            {
                switch (e.Key)
                {
                    case Key.ControlLeft:
                    case Key.ControlRight:
                        _ControlDown = false;
                        break;
                    default:
                        break;
                }
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
            LeftRights = 0;
            lock (Locker)
            {
                _KeyboardString = "";
                _InitBS = 0;
                _CopyPressed = false;
                _TogglerPressed = false;
                _Pages = 0;
                _Scrolls = 0;
                _LeftRights = 0;
            }
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
            lock (Locker)
            {
                KeyboardString = _KeyboardString;
                InitBS = _InitBS;
                ControlDown = _ControlDown;
                CopyPressed = _CopyPressed;
                TogglerPressed = _TogglerPressed;
                Pages = _Pages;
                Scrolls = _Scrolls;
                LeftRights = _LeftRights;
            }
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
