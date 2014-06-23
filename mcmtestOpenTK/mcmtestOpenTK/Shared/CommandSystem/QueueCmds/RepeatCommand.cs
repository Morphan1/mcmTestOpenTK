using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Shared.CommandSystem.QueueCmds
{
    // <--[command]
    // @Name repeat
    // @Arguments <times to repeat>/stop/next
    // @Short Executes the following block of commands a specified number of times.
    // @Updated 2014/06/23
    // @Authors mcmonkey
    // @Description
    // The repeat command will loop the given number of times and execute the included command block
    // each time it loops.
    // It can also be used to stop the looping via the 'stop' argument, or to jump to the next
    // entry in the list and restart the command block via the 'next' argument.
    // TODO: Explain more!
    // @Example
    // // This example runs through the list and echos "1/3", then "2/3", then "3/3" back to the console.
    // repeat 3
    // {
    //     echo "<{var[repeat_index]}>/<{var[repeat_total]}>";
    // }
    // @Example
    // // This example runs through the list and echos "1", then "1r", then "2", then "3", then "3r" back to the console.
    // repeat 3
    // {
    //     echo "<{var[repeat_index]}>"
    //     if <{var[repeat_index].equals[2]}>
    //     {
    //         repeat next;
    //     }
    //     echo "<{var[repeat_index]}>r"
    // }
    // @Example
    // // This example runs through the list and echos "1", then "2", then stops early back to the console.
    // repeat 3
    // {
    //     echo "<{var[repeat_index]}>"
    //     if <{var[repeat_index].equals[3]}>
    //     {
    //         repeat stop
    //     }
    // }
    // @Example
    // TODO: More examples!
    // @Tags
    // <{var[repeat_index]}> returns what iteration (numeric) the repeat is on.
    // <{var[repeat_total]}> returns what iteration (numeric) the repeat is aiming for, and will end on if not stopped early.
    // @BlockVars
    // repeat_index TextTag
    // repeat_total TextTag
    // -->
    class RepeatCommand : AbstractCommand
    {
        public RepeatCommand()
        {
            Name = "repeat";
            Arguments = "<times to repeat>/stop/next";
            Description = "Executes the following block of commands a specified number of times.";
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
                string count = entry.GetArgument(0);
                if (count == "\0CALLBACK")
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
                else if (count.ToLower() == "stop")
                {
                    entry.Good("Stopping repeat loop.");
                    while (entry.Queue.CommandList.Count > 0)
                    {
                        if (entry.Queue.CommandList[0].CommandLine == "repeat \0CALLBACK")
                        {
                            entry.Queue.CommandList.RemoveAt(0);
                            break;
                        }
                        entry.Queue.CommandList.RemoveAt(0);
                    }
                }
                else if (count.ToLower() == "next")
                {
                    entry.Good("Skipping to next repeat entry...");
                    while (entry.Queue.CommandList.Count > 0)
                    {
                        if (entry.Queue.CommandList[0].CommandLine == "repeat \0CALLBACK")
                        {
                            break;
                        }
                        entry.Queue.CommandList.RemoveAt(0);
                    }
                }
                else
                {
                    int target = Utilities.StringToInt(count);
                    if (target <= 0)
                    {
                        entry.Good("Not repeating.");
                    }
                    if (entry.Result > 0)
                    {
                        entry.Block.RemoveAt(entry.Block.Count - 1);
                    }
                    entry.Result = target;
                    entry.Index = 1;
                    if (entry.Block != null)
                    {
                        entry.Good("Repeating <{color.emphasis}>" + target + "<{color.base}> times...");
                        CommandEntry callback = new CommandEntry("repeat \0CALLBACK", null, entry,
                            this, new List<string> { "\0CALLBACK" }, "repeat");
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
