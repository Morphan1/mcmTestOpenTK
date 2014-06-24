using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.TagHandlers;
using mcmtestOpenTK.ServerSystem.CommandHandlers;

namespace mcmtestOpenTK.Shared.CommandSystem.QueueCmds
{
    class CallCommand: AbstractCommand
    {
        public CallCommand()
        {
            Name = "call";
            Arguments = "<function to call>";
            Description = "Runs a function.";
            IsFlow = true;
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 1)
            {
                ShowUsage(entry);
            }
            else
            {
                string fname = entry.GetArgument(0);
                if (fname == "\0CALLBACK")
                {
                    return;
                }
                CommandScript script = entry.Queue.CommandSystem.GetFunction(fname);
                if (script != null)
                {
                    entry.Good("Calling '<{color.emphasis}>" + TagParser.Escape(fname) + "<{color.base}>'...");
                    List<CommandEntry> block = script.GetEntries();
                    block.Add(new CommandEntry("call \0CALLBACK", null, entry,
                            this, new List<string> { "\0CALLBACK" }, "call"));
                    entry.Queue.AddCommandsNow(block);
                }
                else
                {
                    entry.Bad("Cannot call function '<{color.emphasis}>" + TagParser.Escape(fname) + "<{color.base}>': it does not exist!");
                }
            }
        }
    }
}
