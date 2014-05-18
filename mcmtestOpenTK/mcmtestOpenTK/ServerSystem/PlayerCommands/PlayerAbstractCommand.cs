using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.ServerSystem.PlayerCommands
{
    public abstract class PlayerAbstractCommand
    {
        /// <summary>
        /// The name of the command.
        /// </summary>
        public string Name = "NAME:UNSET";

        /// <summary>
        /// A short explanation of the arguments of the command.
        /// </summary>
        public string Arguments = "ARGUMENTS:UNSET";

        /// <summary>
        /// A short explanation of what the command does.
        /// </summary>
        public string Description = "DESCRIPTION:UNSET";

        /// <summary>
        /// Whether the command is for debugging purposes.
        /// </summary>
        public bool IsDebug = false;

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="entry">Entry to be executed</param>
        public abstract void Execute(PlayerCommandEntry entry);

        /// <summary>
        /// Displays the usage information on a command to the console.
        /// </summary>
        /// <param name="entry">The CommandEntry data to get usage help from.</param>
        public static void ShowUsage(PlayerCommandEntry entry)
        {
            entry.player.SendMessage(TextStyle.Color_Separate + entry.Command.Name + TextStyle.Color_Outbad + ": " + entry.Command.Description);
            entry.player.SendMessage(TextStyle.Color_Commandhelp + "Usage: /" + entry.Name + " " + entry.Command.Arguments);
        }
    }
}
