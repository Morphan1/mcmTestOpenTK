using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.CommandHandlers;

namespace mcmtestOpenTK.Shared.CommandSystem.QueueCmds
{
    class RepeatCommand : AbstractCommand
    {
        public RepeatCommand()
        {
            Name = "repeat";
            Arguments = "<times to repeat>";
            Description = "Executes the following block of commands a specified number of times.";
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 1)
            {
                ShowUsage(entry);
            }
            else
            {
                string count = entry.GetArgument(0);
                if (count == "CALLBACK")
                {
                    if (entry.BlockOwner.Command.Name == "repeat" || entry.BlockOwner.Block == null || entry.BlockOwner.Block.Count == 0
                        || entry.BlockOwner.Block[entry.BlockOwner.Block.Count - 1] != entry)
                    {
                        entry.BlockOwner.Index++;
                        if (entry.BlockOwner.Index > entry.BlockOwner.Result)
                        {
                            entry.Good("Repeating ending, reached target.");
                        }
                        else
                        {
                            entry.Good("Repeating at index <{color.emphasis}>" + entry.BlockOwner.Index + "/" + entry.BlockOwner.Result + "<{color.base}>...");
                            entry.Queue.SetVariable("repeat_index", entry.BlockOwner.Index.ToString());
                            entry.Queue.SetVariable("repeat_total", entry.BlockOwner.Result.ToString());
                            entry.Queue.AddCommandsNow(entry.BlockOwner.Block);
                        }
                    }
                    else
                    {
                        entry.Bad("Repeat CALLBACK invalid: not a real callback!");
                    }
                }
                else
                {
                    int target = Utilities.StringToInt(count);
                    if (entry.Result > 0)
                    {
                        entry.Block.RemoveAt(entry.Block.Count - 1);
                    }
                    entry.Result = target;
                    entry.Index = 1;
                    if (entry.Block != null)
                    {
                        entry.Good("Repeating <{color.emphasis}>" + target + "<{color.base}> times...");
                        CommandEntry callback = new CommandEntry("repeat CALLBACK", null, entry);
                        entry.Block.Add(callback);
                        entry.Queue.SetVariable("repeat_index", "1");
                        entry.Queue.SetVariable("repeat_total", target.ToString());
                        entry.Queue.AddCommandsNow(entry.Block);
                    }
                    else
                    {
                        entry.Bad("Repeat invalid: No block follows!");
                    }
                }
            }
        }
    }
}
