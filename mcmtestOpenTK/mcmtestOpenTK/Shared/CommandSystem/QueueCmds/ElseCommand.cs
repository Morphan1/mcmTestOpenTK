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

        public override void Execute(CommandInfo info)
        {
            CommandEntry IfEntry = null;
            CommandEntry Holder = info.Queue.LastCommand;
            while (IfEntry == null && Holder != null)
            {
                if (Holder.BlockOwner == info.Entry.BlockOwner)
                {
                    if (Holder.Info.Command == IfCommand.MainObject || Holder.Info.Command == MainObject)
                    {
                        IfEntry = Holder;
                    }
                    break;
                }
                Holder = Holder.BlockOwner;
            }
            if (IfEntry == null)
            {
                info.Output.WriteLine(TextStyle.Color_Outbad + "ELSE invalid: IF command did not preceed!");
                return;
            }
            if (IfEntry.Info.Result == 1)
            {
                info.Output.WriteLine(TextStyle.Color_Outbad + "ELSE continuing, IF passed.");
                return;
            }
            if (info.Arguments.Count >= 1)
            {
                string ifbit = info.GetArgument(0);
                if (ifbit.ToLower() != "if")
                {
                    ShowUsage(info);
                    return;
                }
                else
                {
                    string comparison = info.GetArgument(1);
                    bool success = comparison.ToLower() == "true";
                    if (info.Entry.Block != null)
                    {
                        if (success)
                        {
                            info.Output.WriteLine(TextStyle.Color_Outgood + "Else if is true, executing...");
                            info.Result = 1;
                            info.Queue.AddCommandsNow(info.Entry.Block);
                        }
                        else
                        {
                            info.Output.WriteLine(TextStyle.Color_Outgood + "Else If is false, doing nothing!");
                        }
                    }
                }
            }
            else
            {
                if (info.Entry.Block != null)
                {
                    info.Output.WriteLine(TextStyle.Color_Outgood + "Else is valid, executing...");
                    info.Result = 1;
                    info.Queue.AddCommandsNow(info.Entry.Block);
                }
                else
                {
                    info.Output.WriteLine(TextStyle.Color_Outbad + "ELSE invalid: No block follows!");
                }
            }
        }
    }
}
