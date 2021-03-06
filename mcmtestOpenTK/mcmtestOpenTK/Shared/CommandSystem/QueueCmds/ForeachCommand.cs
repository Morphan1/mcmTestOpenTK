﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.TagHandlers.Objects;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.Shared.CommandSystem.QueueCmds
{
    // <--[command]
    // @Name foreach
    // @Arguments start/stop/next [list to loop through]
    // @Short Executes the following block of commands once foreach item in the given list.
    // @Updated 2014/06/23
    // @Authors mcmonkey
    // @Group Queue
    // @Braces true
    // @Description
    // The foreach command will loop through the given list and run the included command block
    // once for each entry in the list.
    // It can also be used to stop the looping via the 'stop' argument, or to jump to the next
    // entry in the list and restart the command block via the 'next' argument.
    // TODO: Explain more!
    // @Example
    // // This example runs through the list and echos "one", then "two", then "three" back to the console.
    // foreach start one|two|three
    // {
    //     echo "<{var[foreach_value]}>"
    // }
    // @Example
    // // This example runs through the list and echos "one", then "oner", then "two", then "three", then "threer" back to the console.
    // foreach start one|two|three
    // {
    //     echo "<{var[foreach_value]}>"
    //     if <{var[foreach_value].equals[two]}>
    //     {
    //         foreach next
    //     }
    //     echo "<{var[foreach_value]}>r"
    // }
    // @Example
    // // This example runs through the list and echos "one", then "two", then stops early back to the console.
    // foreach start one|two|three
    // {
    //     echo "<{var[foreach_value]}>"
    //     if <{var[foreach_value].equals[three]}>
    //     {
    //         foreach stop
    //     }
    // }
    // @Example
    // TODO: More examples!
    // @Tags
    // <{var[foreach_index]}> returns what iteration (numeric) the foreach is on.
    // <{var[foreach_total]}> returns what iteration (numeric) the foreach is aiming for, and will end on if not stopped early.
    // <{var[foreach_value]}> returns the current item in the list.
    // <{var[foreach_list]}> returns the full list being looped through.
    // @BlockVars
    // foreach_index TextTag
    // foreach_total TextTag
    // foreach_value TextTag
    // foreach_list TextTag
    // -->
    class ForeachCommandData : AbstractCommandEntryData
    {
        public List<string> List;
        public int Index;
        public override AbstractCommandEntryData Duplicate()
        {
            ForeachCommandData toret = new ForeachCommandData();
            toret.List = new List<string>(List);
            toret.Index = Index;
            return toret;
        }
    }

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
                        ForeachCommandData data = (ForeachCommandData)entry.BlockOwner.Data;
                        data.Index++;
                        if (data.Index > data.List.Count)
                        {
                            entry.Good("Foreach loop ending, reached target.");
                        }
                        else
                        {
                            entry.Good("Foreach loop continuing at index <{color.emphasis}>" + data.Index + "/" + data.List.Count + "<{color.base}>...");
                            entry.Queue.SetVariable("foreach_index", data.Index.ToString());
                            entry.Queue.SetVariable("foreach_total", data.List.Count.ToString());
                            entry.Queue.SetVariable("foreach_value", data.List[data.Index - 1].ToString());
                            entry.Queue.SetVariable("foreach_list", new ListTag(data.List).ToString());
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
                    bool hasnext = false;
                    for (int i = 0; i < entry.Queue.CommandList.Count; i++)
                    {
                        if (entry.Queue.GetCommand(i).CommandLine == "foreach \0CALLBACK")
                        {
                            hasnext = true;
                            break;
                        }
                    }
                    if (hasnext)
                    {
                        entry.Good("Stopping foreach loop.");
                        while (entry.Queue.CommandList.Count > 0)
                        {
                            if (entry.Queue.GetCommand(0).CommandLine == "foreach \0CALLBACK")
                            {
                                entry.Queue.RemoveCommand(0);
                                break;
                            }
                            entry.Queue.RemoveCommand(0);
                        }
                    }
                    else
                    {
                        entry.Bad("Cannot stop foreach: not in one!");
                    }
                }
                else if (type.ToLower() == "next")
                {
                    bool hasnext = false;
                    for (int i = 0; i < entry.Queue.CommandList.Count; i++)
                    {
                        if (entry.Queue.GetCommand(i).CommandLine == "foreach \0CALLBACK")
                        {
                            hasnext = true;
                            break;
                        }
                    }
                    if (hasnext)
                    {
                        entry.Good("Skipping to next foreach entry...");
                        while (entry.Queue.CommandList.Count > 0)
                        {
                            if (entry.Queue.GetCommand(0).CommandLine == "foreach \0CALLBACK")
                            {
                                break;
                            }
                            entry.Queue.RemoveCommand(0);
                        }
                    }
                    else
                    {
                        entry.Bad("Cannot stop foreach: not in one!");
                    }
                }
                else if (type.ToLower() == "start" && entry.Arguments.Count > 1)
                {
                    ListTag list = new ListTag(entry.GetArgument(1));
                    int target = list.ListEntries.Count;
                    if (target <= 0)
                    {
                        entry.Good("Not looping.");
                        return;
                    }
                    ForeachCommandData data = new ForeachCommandData();
                    data.Index = 1;
                    data.List = list.ToStringList();
                    entry.Data = data;
                    if (entry.Block != null)
                    {
                        entry.Good("Foreach looping <{color.emphasis}>" + target + "<{color.base}> times...");
                        CommandEntry callback = new CommandEntry("foreach \0CALLBACK", null, entry,
                            this, new List<string> { "\0CALLBACK" }, "foreach", 0);
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
