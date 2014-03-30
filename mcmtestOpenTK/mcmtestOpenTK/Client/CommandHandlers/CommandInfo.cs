using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.TagHandlers;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.CommandHandlers
{
    public class CommandInfo
    {
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
        public List<string> Arguments = null;

        public CommandInfo(string _name, AbstractCommand _command, List<string> _arguments)
        {
            Name = _name;
            Command = _command;
            Arguments = _arguments;
        }

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
            return TagParser.ParseTags(Arguments[place], TextStyle.Color_Simple, null, null);
        }

        /// <summary>
        /// Gets all arguments piled together into a string.
        /// </summary>
        /// <returns>The combined string</returns>
        public string AllArguments()
        {
            return TagParser.ParseTags(Utilities.Concat(Arguments), TextStyle.Color_Simple, null, null);
        }
    }
}
