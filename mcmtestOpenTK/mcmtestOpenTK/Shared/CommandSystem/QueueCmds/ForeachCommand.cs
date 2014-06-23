using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.TagHandlers.Objects;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.Shared.CommandSystem.QueueCmds
{
    class ForeachCommand : AbstractCommand
    {
        public ForeachCommand()
        {
            Name = "foreach";
            Arguments = "start/stop/next [list to loop through]";
            Description = "Executes the following block of commands once foreach item in the given list.";
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
                string type = entry.GetArgument(0);
                if (type == "\0CALLBACK")
                {
                    if (entry.BlockOwner.Command.Name == "foreach" || entry.BlockOwner.Block == null || entry.BlockOwner.Block.Count == 0
                        || entry.BlockOwner.Block[entry.BlockOwner.Block.Count - 1] != entry)
                    {
                        entry.BlockOwner.Index++;
                        if (entry.BlockOwner.Index > entry.BlockOwner.Result)
                        {
                            entry.Good("Foreach loop ending, reached target.");
                        }
                        else
                        {
                            entry.Good("Foreach loop continuing at index <{color.emphasis}>" + entry.BlockOwner.Index + "/" + entry.BlockOwner.Result + "<{color.base}>...");
                            entry.Queue.SetVariable("foreach_index", entry.BlockOwner.Index.ToString());
                            entry.Queue.SetVariable("foreach_total", entry.BlockOwner.Result.ToString());
                            ListTag list = (ListTag)entry.BlockOwner.obj;
                            entry.Queue.SetVariable("foreach_value", list.ListEntries[entry.BlockOwner.Index - 1].ToString());
                            entry.Queue.SetVariable("foreach_list", list.ToString());
                            entry.Queue.AddCommandsNow(entry.BlockOwner.Block);
                        }
                    }
                    else
                    {
                        entry.Bad("Foreach CALLBACK invalid: not a real callback!");
                    }
                }
                else if (type.ToLower() == "stop")
                {
                    entry.Good("Stopping foreach loop.");
                    while (entry.Queue.CommandList.Count > 0)
                    {
                        if (entry.Queue.CommandList[0].CommandLine == "foreach \0CALLBACK")
                        {
                            break;
                        }
                        entry.Queue.CommandList.RemoveAt(0);
                    }
                }
                else if (type.ToLower() == "next")
                {
                    entry.Good("Skipping to next foreach entry...");
                    while (entry.Queue.CommandList.Count > 1)
                    {
                        if (entry.Queue.CommandList[1].CommandLine == "foreach \0CALLBACK")
                        {
                            break;
                        }
                        entry.Queue.CommandList.RemoveAt(0);
                    }
                }
                else if (type.ToLower() == "start" && entry.Arguments.Count > 1)
                {
                    ListTag list = new ListTag(entry.GetArgument(1));
                    int target = list.ListEntries.Count;
                    if (target <= 0)
                    {
                        entry.Good("Not looping.");
                    }
                    if (entry.Result > 0)
                    {
                        entry.Block.RemoveAt(entry.Block.Count - 1);
                    }
                    entry.Result = target;
                    entry.Index = 1;
                    entry.obj = list;
                    if (entry.Block != null)
                    {
                        entry.Good("Foreach looping <{color.emphasis}>" + target + "<{color.base}> times...");
                        CommandEntry callback = new CommandEntry("foreach \0CALLBACK", null, entry,
                            this, new List<string> { "\0CALLBACK" }, "foreach");
                        entry.Block.Add(callback);
                        entry.Queue.SetVariable("foreach_index", "1");
                        entry.Queue.SetVariable("foreach_total", target.ToString());
                        entry.Queue.SetVariable("foreach_value", list.ListEntries[0].ToString());
                        entry.Queue.SetVariable("foreach_list", list.ToString());
                        entry.Queue.AddCommandsNow(entry.Block);
                    }
                    else
                    {
                        entry.Bad("Foreach invalid: No block follows!");
                    }
                }
                else
                {
                    ShowUsage(entry);
                }
            }
        }
    }
}
