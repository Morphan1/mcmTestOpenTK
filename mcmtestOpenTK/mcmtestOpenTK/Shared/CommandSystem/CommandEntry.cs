using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.Shared.CommandSystem
{
    public class CommandEntry
    {
        /// <summary>
        /// The command itself.
        /// </summary>
        public string CommandLine;

        /// <summary>
        /// If the command has a block of { braced } commands, this will contain that block.
        /// </summary>
        public List<CommandEntry> Block;

        /// <summary>
        /// What command entry object owns this entry, if any.
        /// </summary>
        public CommandEntry BlockOwner = null;

        public CommandEntry(string _command, List<CommandEntry> _block, CommandEntry _owner)
        {
            CommandLine = _command;
            Block = _block;
            BlockOwner = _owner;
        }

        /// <summary>
        /// The command name input by the user.
        /// </summary>
        public string Name = "";

        /// <summary>
        /// The command that should execute this input.
        /// </summary>
        public AbstractCommand Command;

        /// <summary>
        /// The arguments input by the user.
        /// </summary>
        public List<string> Arguments;

        /// <summary>
        /// The command queue this command is running inside.
        /// </summary>
        public CommandQueue Queue;

        /// <summary>
        /// The object to use for any console / debug output.
        /// </summary>
        public Outputter Output;

        /// <summary>
        /// A result set by the command, if any.
        /// </summary>
        public int Result = 0;

        /// <summary>
        /// Gets an argument at a specified place, handling any tags.
        /// </summary>
        /// <param name="place">The argument place number</param>
        /// <returns>The parsed argument</returns>
        public string GetArgument(int place)
        {
            if (place >= Arguments.Count || place < 0)
            {
                throw new ArgumentOutOfRangeException("Value must be greater than 0 and less than command input argument count");
            }
            return Queue.CommandSystem.TagSystem.ParseTags(Arguments[place], TextStyle.Color_Simple, null);
        }

        /// <summary>
        /// Gets all arguments piled together into a string.
        /// </summary>
        /// <returns>The combined string</returns>
        public string AllArguments()
        {
            return Queue.CommandSystem.TagSystem.ParseTags(Utilities.Concat(Arguments), TextStyle.Color_Simple, null);
        }
    }
}
