using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Shared.CommandSystem.QueueCmds
{
    class IfCommand: AbstractCommand
    {
        public static IfCommand MainObject = null;

        public IfCommand()
        {
            Name = "if";
            Arguments = "<true/false>";
            Description = "Executes the following block of commands only if the input is true.";
            MainObject = this;
        }

        public override void Execute(CommandInfo info)
        {
            if (info.Arguments.Count < 1)
            {
                ShowUsage(info);
            }
            else
            {
                string comparison = info.GetArgument(0);
                bool success = comparison.ToLower() == "true";
                if (info.Entry.Block != null)
                {
                    if (success)
                    {
                        info.Output.Good("If is true, executing...");
                        info.Result = 1;
                        info.Queue.AddCommandsNow(info.Entry.Block);
                    }
                    else
                    {
                        info.Output.Good("If is false, doing nothing!");
                    }
                }
                else
                {
                    info.Output.Bad("IF invalid: No block follows!");
                }
            }
        }
    }
}
