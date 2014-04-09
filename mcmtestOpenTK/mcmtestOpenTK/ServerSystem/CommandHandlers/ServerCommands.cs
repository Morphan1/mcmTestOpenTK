using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.ServerSystem.CommandHandlers.CommonCmds;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;

namespace mcmtestOpenTK.ServerSystem.CommandHandlers
{
    class ServerCommands
    {
        /// <summary>
        /// The Commands object that all commands actually go to.
        /// </summary>
        public static Commands CommandSystem;

        /// <summary>
        /// Prepares the command system, registering all base commands.
        /// </summary>
        public static void Init()
        {
            CommandSystem = new Commands();
            CommandSystem.Output = new ServerOutputter();
            CommandSystem.Init();

            // Common
            CommandSystem.RegisterCommand(new QuitCommand());
        }

        /// <summary>
        /// Advances any running command queues.
        /// </summary>
        public static void Tick()
        {
            CommandSystem.Tick(Server.DeltaF);
        }

        /// <summary>
        /// Executes an arbitrary list of command inputs (separated by newlines, semicolons, ...)
        /// </summary>
        /// <param name="commands">The command string to parse</param>
        public static void ExecuteCommands(string commands)
        {
            CommandSystem.ExecuteCommands(commands);
        }
    }
}
