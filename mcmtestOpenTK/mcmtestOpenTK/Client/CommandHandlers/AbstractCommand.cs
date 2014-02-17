using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;

namespace mcmtestOpenTK.Client.CommandHandlers
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
        /// Executes the command.
        /// </summary>
        /// <param name="info">Information on command execution</param>
        public abstract void Execute(CommandInfo info);

        public static void ShowUsage(CommandInfo info)
        {
            UIConsole.WriteLine(TextStyle.Color_Commandhelp + "Usage: /" + info.Name + " " + info.Command.Arguments);
        }
    }
}
