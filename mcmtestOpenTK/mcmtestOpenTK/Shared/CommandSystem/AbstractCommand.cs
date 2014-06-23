﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.TagHandlers;

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
        /// Whether the command is part of a script's flow rather than for normal client use.
        /// </summary>
        public bool IsFlow = false;

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="entry">Entry to be executed</param>
        public abstract void Execute(CommandEntry entry);

        /// <summary>
        /// Displays the usage information on a command to the console.
        /// </summary>
        /// <param name="entry">The CommandEntry data to get usage help from.</param>
        public static void ShowUsage(CommandEntry entry)
        {
            entry.Bad("<{color.emphasis}>" + TagParser.Escape(entry.Command.Name) + "<{color.base}>: " + TagParser.Escape(entry.Command.Description));
            entry.Bad("<{color.cmdhelp}>Usage: /" + TagParser.Escape(entry.Name) + " " + TagParser.Escape(entry.Command.Arguments));
            if (entry.Command.IsDebug)
            {
                entry.Bad("Note: This command is intended for debugging purposes.");
            }
        }
    }
}
