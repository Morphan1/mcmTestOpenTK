using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.ServerSystem.PlayerCommands
{
    public class PlayerCommandEntry
    {
        /// <summary>
        /// The player executing this command.
        /// </summary>
        public Player player;

        /// <summary>
        /// The full command line.
        /// </summary>
        public string CommandLine;

        /// <summary>
        /// The command name input by the user.
        /// </summary>
        public string Name;

        /// <summary>
        /// The command that should execute this input.
        /// </summary>
        public PlayerAbstractCommand Command;

        /// <summary>
        /// The arguments input by the user.
        /// </summary>
        public List<string> Arguments;

        /// <summary>
        /// Gets all arguments piled together into a string.
        /// </summary>
        /// <returns>The combined string</returns>
        public string AllArguments()
        {
            return Utilities.Concat(Arguments);
        }
    }
}
