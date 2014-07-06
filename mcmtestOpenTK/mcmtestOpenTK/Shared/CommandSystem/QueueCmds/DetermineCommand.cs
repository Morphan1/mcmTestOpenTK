using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.Shared.CommandSystem.CommonCmds
{
    class DetermineCommand: AbstractCommand
    {
        public DetermineCommand()
        {
            Name = "determine";
            Arguments = "[value to set on the queue]";
            Description = "Sets the value determined on the queue. If no argument is specified, sets null.";
            IsFlow = true;
        }

        public override void Execute(CommandEntry entry)
        {
            string determ = null;
            if (entry.Arguments.Count > 0)
            {
                determ = entry.GetArgument(0);
            }
            entry.Queue.Determination = determ;
            entry.Good("<{color.info}>Determination of the queue set to '<{color.emphasis}>" + TagParser.Escape(determ) + "<{color.info}>'.");
        }
    }
}
