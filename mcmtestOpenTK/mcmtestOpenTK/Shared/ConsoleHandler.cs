using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.CommandHandlers;
using mcmtestOpenTK.Shared;
using System.Threading;
using System.Diagnostics;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

namespace mcmtestOpenTK.Shared
{
    public class ConsoleHandler
    {
        static Object holder = new Object();

        static List<string> CommandInput;

        public static string read = "";

        public static bool HandlerActive = false;

        public static int pos = 0;

        /// <summary>
        /// Prepare the console listener.
        /// </summary>
        public static void Init()
        {
            CommandInput = new List<string>();
            Thread thread = new Thread(new ThreadStart(ListenLoop));
            thread.Name = "System_ConsoleListener";
            HandlerActive = true;
            thread.Start();
        }

        static void ListenLoop()
        {
            while (true)
            {
                ConsoleKeyInfo pressed = Console.ReadKey(true);
                if (pressed.Key == ConsoleKey.Enter)
                {
                    lock (holder)
                    {
                        CommandInput.Add(read);
                    }
                    SysConsole.WriteLine(">" + read);
                    read = "";
                    pos = 0;
                }
                else if (pressed.Key == ConsoleKey.Backspace)
                {
                    if (pos > 0)
                    {
                        read = read.Substring(0, pos - 1) + read.Substring(pos);
                        pos--;
                    }
                }
                else if (pressed.Key == ConsoleKey.Delete)
                {
                    if (pos < read.Length)
                    {
                        read = read.Substring(0, pos) + read.Substring(pos + 1);
                    }
                }
                else if (pressed.Key == ConsoleKey.LeftArrow)
                {
                    if (pos > 0)
                    {
                        pos--;
                    }
                }
                else if (pressed.Key == ConsoleKey.RightArrow)
                {
                    if (pos < read.Length)
                    {
                        pos++;
                    }
                }
                else if (pressed.Key == ConsoleKey.Home)
                {
                    pos = 0;
                }
                else if (pressed.Key == ConsoleKey.End)
                {
                    pos = read.Length;
                }
                else if ((pressed.Key >= ConsoleKey.F1 && pressed.Key <= ConsoleKey.F24)
                    || pressed.Key == ConsoleKey.Escape)
                {
                    // Do nothing
                }
                // TODO: Up/Down arrows
                // TODO: Other special keys
                else
                {
                    read = read.Substring(0, pos) + pressed.KeyChar + read.Substring(pos);
                    pos++;
                }
                Update();
            }
        }

        public static void Update()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(">" + read + " ");
            // TODO: Replace this nonsense with scrolling, or multi-line working
            if (pos + 1 >= Console.BufferWidth)
            {
                Console.SetBufferSize(Console.BufferWidth + 1, Console.BufferHeight);
            }
            Console.SetCursorPosition(pos + 1, Console.CursorTop);
        }

        /// <summary>
        /// Checks for any console input, and handles appropriately.
        /// </summary>
        public static void CheckInput()
        {
            List<string> commandsinput = null;
            lock (holder)
            {
                if (CommandInput.Count > 0)
                {
                    commandsinput = CommandInput;
                    CommandInput = new List<string>();
                }
            }
            if (commandsinput != null)
            {
                for (int i = 0; i < commandsinput.Count; i++)
                {
                    // TODO: Generic client/server shared method?
                    ServerCommands.ExecuteCommands(Utilities.CleanStringInput(commandsinput[i]));
                }
            }
        }
    }
}
