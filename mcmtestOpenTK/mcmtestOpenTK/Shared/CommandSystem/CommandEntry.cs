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
        public string Command;

        /// <summary>
        /// If the command has a block of { braced } commands, this will contain that block.
        /// </summary>
        public List<CommandEntry> Block;

        /// <summary>
        /// What command entry object owns this entry, if any.
        /// </summary>
        public CommandEntry BlockOwner = null;

        /// <summary>
        /// The command info created as this entry was executed.
        /// </summary>
        public CommandInfo Info = null;

        public CommandEntry(string _command, List<CommandEntry> _block, CommandEntry _owner)
        {
            Command = _command;
            Block = _block;
            BlockOwner = _owner;
        }
    }
}
