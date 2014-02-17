using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
