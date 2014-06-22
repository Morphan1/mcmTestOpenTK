using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Input;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.UIHandlers;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    public class KeyHandler
    {
        static List<Key>[] HardBinds;

        static Dictionary<Key, string> Binds;

        static Dictionary<string, Key> keynames;

        /// <summary>
        /// Prepare key handler.
        /// </summary>
        public static void Init()
        {
            HardBinds = new List<Key>[6];
            for (int i = 0; i < HardBinds.Length; i++)
            {
                HardBinds[i] = new List<Key>();
            }
            HardBinds[(int)KeyBind.FORWARD].Add(Key.W);
            HardBinds[(int)KeyBind.BACK].Add(Key.S);
            HardBinds[(int)KeyBind.LEFT].Add(Key.A);
            HardBinds[(int)KeyBind.RIGHT].Add(Key.D);
            HardBinds[(int)KeyBind.DOWN].Add(Key.C);
            HardBinds[(int)KeyBind.UP].Add(Key.Space);
            keynames = new Dictionary<string, Key>();
            keynames.Add("a", Key.A); keynames.Add("b", Key.B); keynames.Add("c", Key.C);
            keynames.Add("d", Key.D); keynames.Add("e", Key.E); keynames.Add("f", Key.F);
            keynames.Add("g", Key.G); keynames.Add("h", Key.H); keynames.Add("i", Key.I);
            keynames.Add("j", Key.J); keynames.Add("k", Key.K); keynames.Add("l", Key.L);
            keynames.Add("m", Key.M); keynames.Add("n", Key.N); keynames.Add("o", Key.O);
            keynames.Add("p", Key.P); keynames.Add("q", Key.Q); keynames.Add("r", Key.R);
            keynames.Add("s", Key.S); keynames.Add("t", Key.T); keynames.Add("u", Key.U);
            keynames.Add("v", Key.V); keynames.Add("w", Key.W); keynames.Add("x", Key.X);
            keynames.Add("y", Key.Y); keynames.Add("z", Key.Z); keynames.Add("1", Key.Number1);
            keynames.Add("2", Key.Number2); keynames.Add("3", Key.Number3); keynames.Add("4", Key.Number4);
            keynames.Add("5", Key.Number5); keynames.Add("6", Key.Number6); keynames.Add("7", Key.Number7);
            keynames.Add("8", Key.Number8); keynames.Add("9", Key.Number9); keynames.Add("0", Key.Number0);
            keynames.Add("lalt", Key.AltLeft); keynames.Add("ralt", Key.AltRight);
            keynames.Add("f1", Key.F1); keynames.Add("f2", Key.F2); keynames.Add("f3", Key.F3);
            keynames.Add("f4", Key.F4); keynames.Add("f5", Key.F5); keynames.Add("f6", Key.F6);
            keynames.Add("f7", Key.F7); keynames.Add("f8", Key.F8); keynames.Add("f9", Key.F9);
            keynames.Add("f10", Key.F10); keynames.Add("f11", Key.F11); keynames.Add("f12", Key.F12);
            keynames.Add("enter", Key.Enter); keynames.Add("end", Key.End); keynames.Add("home", Key.Home);
            keynames.Add("insert", Key.Insert); keynames.Add("delete", Key.Delete); keynames.Add("pause", Key.Pause);
            keynames.Add("lshift", Key.ShiftLeft); keynames.Add("rshift", Key.ShiftRight); keynames.Add("tab", Key.Tab);
            keynames.Add("caps", Key.CapsLock); keynames.Add("lctrl", Key.ControlLeft); keynames.Add("rctrl", Key.ControlRight);
            keynames.Add(",", Key.Comma); keynames.Add(".", Key.Period); keynames.Add("/", Key.Slash);
            keynames.Add("backslash", Key.BackSlash); keynames.Add("-", Key.Minus); keynames.Add("=", Key.Plus);
            keynames.Add("backspace", Key.BackSpace); keynames.Add("semicolon", Key.Semicolon); keynames.Add("'", Key.Quote);
            keynames.Add("[", Key.BracketLeft); keynames.Add("]", Key.BracketRight); keynames.Add("kp1", Key.Keypad1);
            keynames.Add("kp2", Key.Keypad2); keynames.Add("kp3", Key.Keypad3); keynames.Add("kp4", Key.Keypad4);
            keynames.Add("kp5", Key.Keypad5); keynames.Add("kp6", Key.Keypad6); keynames.Add("kp7", Key.Keypad7);
            keynames.Add("kp8", Key.Keypad8); keynames.Add("kp9", Key.Keypad9); keynames.Add("kp0", Key.Keypad0);
            keynames.Add("kpenter", Key.KeypadEnter); keynames.Add("kpmultiply", Key.KeypadMultiply);
            keynames.Add("kpadd", Key.KeypadAdd); keynames.Add("kpsubtract", Key.KeypadSubtract);
            keynames.Add("kpdivide", Key.KeypadDivide); keynames.Add("kpperiod", Key.KeypadPeriod);
        }

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

    public enum KeyBind: int
    {
        FORWARD = 0,
        BACK = 1,
        LEFT = 2,
        RIGHT = 3,
        UP = 4,
        DOWN = 5
    }
}
