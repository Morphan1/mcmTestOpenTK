using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Shared.CommandSystem.QueueCmds
{
    class IfCommand: AbstractCommand
    {
        public IfCommand()
        {
            Name = "if";
            Arguments = "<true/false>";
            Description = "Executes the following block of commands only if the input is true.";
            IsFlow = true;
        }

        public override void Execute(CommandEntry entry)
        {
            entry.Result = 0;
            if (entry.Arguments.Count < 1)
            {
                ShowUsage(entry);
            }
            else
            {
                string comparison = entry.GetArgument(0);
                bool success = comparison.ToLower() == "true";
                if (entry.Block != null)
                {
                    if (success)
                    {
                        entry.Good("If is true, executing...");
                        entry.Result = 1;
                        entry.Queue.AddCommandsNow(entry.Block);
                    }
                    else
                    {
                        entry.Good("If is false, doing nothing!");
                    }
                }
                else
                {
                    entry.Bad("If invalid: No block follows!");
                }
            }
        }
    }
}
