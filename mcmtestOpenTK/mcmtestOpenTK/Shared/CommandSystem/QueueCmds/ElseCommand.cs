using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Shared.CommandSystem.QueueCmds
{
    class ElseCommand: AbstractCommand
    {
        public ElseCommand MainObject = null;

        public ElseCommand()
        {
            Name = "else";
            Arguments = "[if <true/false>]";
            Description = "Executes the following block of commands only if the previous if failed, and optionally if additional requirements are met.";
            MainObject = this;
        }

        public override void Execute(CommandEntry entry)
        {
            CommandEntry IfEntry = null;
            CommandEntry Holder = entry.Queue.LastCommand;
            while (IfEntry == null && Holder != null)
            {
                if (Holder.BlockOwner == entry.BlockOwner)
                {
                    if (Holder.Command == IfCommand.MainObject || Holder.Command == MainObject)
                    {
                        IfEntry = Holder;
                    }
                    break;
                }
                Holder = Holder.BlockOwner;
            }
            if (IfEntry == null)
            {
                entry.Output.Bad("ELSE invalid: IF command did not preceed!");
                return;
            }
            if (IfEntry.Result == 1)
            {
                entry.Output.Good("ELSE continuing, IF passed.");
                return;
            }
            if (entry.Arguments.Count >= 1)
            {
                string ifbit = entry.GetArgument(0);
                if (ifbit.ToLower() != "if")
                {
                    ShowUsage(entry);
                    return;
                }
                else
                {
                    string comparison = entry.GetArgument(1);
                    bool success = comparison.ToLower() == "true";
                    if (entry.Block != null)
                    {
                        if (success)
                        {
                            entry.Output.Good("Else if is true, executing...");
                            entry.Result = 1;
                            entry.Queue.AddCommandsNow(entry.Block);
                        }
                        else
                        {
                            entry.Output.Good("Else If is false, doing nothing!");
                        }
                    }
                }
            }
            else
            {
                if (entry.Block != null)
                {
                    entry.Output.Good("Else is valid, executing...");
                    entry.Result = 1;
                    entry.Queue.AddCommandsNow(entry.Block);
                }
                else
                {
                    entry.Output.Bad("ELSE invalid: No block follows!");
                }
            }
        }
    }
}
