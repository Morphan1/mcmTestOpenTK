using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Shared.CommandSystem
{
    public abstract class AbstractCommand
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
        /// <param name="info">Information on command execution</param>
        public abstract void Execute(CommandInfo info);

        /// <summary>
        /// Displays the usage information on a command to the console.
        /// </summary>
        /// <param name="info">The CommandInfo data to get usage help from.</param>
        public static void ShowUsage(CommandInfo info)
        {
            info.Output.WriteLine(TextStyle.Color_Separate + info.Command.Name + TextStyle.Color_Outgood + ": " + info.Command.Description);
            info.Output.WriteLine(TextStyle.Color_Commandhelp + "Usage: /" + info.Name + " " + info.Command.Arguments);
            if (info.Command.IsDebug)
            {
                info.Output.WriteLine("Note: This command is intended for debugging purposes.");
            }
        }
    }
}
