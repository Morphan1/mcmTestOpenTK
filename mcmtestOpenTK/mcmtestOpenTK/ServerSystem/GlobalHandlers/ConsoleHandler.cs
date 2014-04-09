using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.CommandHandlers;
using mcmtestOpenTK.Shared;
using System.Threading;

namespace mcmtestOpenTK.ServerSystem.GlobalHandlers
{
    public class ConsoleHandler
    {
        static Object holder = new Object();

        static List<string> CommandInput;

        /// <summary>
        /// Prepare the console listener.
        /// </summary>
        public static void Init()
        {
            CommandInput = new List<string>();
            Thread thread = new Thread(new ThreadStart(ListenLoop));
            thread.Start();
        }

        static void ListenLoop()
        {
            while (true)
            {
                string read = Console.ReadLine();
                lock (holder)
                {
                    CommandInput.Add(read);
                }
            }
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
                    Commands.ExecuteCommands(commandsinput[i]);
                }
            }
        }
    }
}
