using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.Shared.CommandSystem.QueueCmds
{
    // <--[command]
    // @Name function
    // @Arguments <name of function>/stop (quiet_fail)
    // @Short Creates a new function of the following command block, and adds it to the script cache.
    // @Updated 2014/06/23
    // @Authors mcmonkey
    // @Group Queue
    // @Description
    // The function command will define the included command block to be a function which can be activated
    // by the <@link command call>call<@/link> command.
    // Add the 'quiet_fail' argument to not produce an error message if the function already exists.
    // Use "function stop" inside a function to end the function call without killing the queue that started it.
    // TODO: Explain more!
    // @Example
    // // This example creates function "helloworld" which, when called, echos "hello world", then stops before it echos a "!"
    // function helloworld
    // {
    //     echo "<{var[repeat_index]}>/<{var[repeat_total]}>";
    // }
    // @Example
    // TODO: More examples!
    // -->
    class FunctionCommand : AbstractCommand
    {
        public FunctionCommand()
        {
            Name = "function";
            Arguments = "<name of function>/stop (quiet_fail)";
            Description = "Creates a new function of the following command block, and adds it to the script cache.";
            IsFlow = true;
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 1)
            {
                ShowUsage(entry);
                return;
            }
            string name = entry.GetArgument(0).ToLower();
            if (name == "stop")
            {
                bool hasnext = false;
                for (int i = 0; i < entry.Queue.CommandList.Count; i++)
                {
                    if (entry.Queue.GetCommand(i).CommandLine == "call \0CALLBACK")
                    {
                        hasnext = true;
                        break;
                    }
                }
                if (hasnext)
                {
                    entry.Good("Stopping function call.");
                    while (entry.Queue.CommandList.Count > 0)
                    {
                        if (entry.Queue.GetCommand(0).CommandLine == "call \0CALLBACK")
                        {
                            entry.Queue.RemoveCommand(0);
                            break;
                        }
                        entry.Queue.RemoveCommand(0);
                    }
                }
                else
                {
                    entry.Bad("Cannot stop function call: not in one!");
                }
                return;
            }
            if (entry.Block == null)
            {
                entry.Bad("Function invalid: No block follows!");
                return;
            }
            if (entry.Queue.CommandSystem.Functions.ContainsKey(name))
            {
                if (entry.Arguments.Count > 1 && entry.GetArgument(1).ToLower() == "quiet_fail")
                {
                    entry.Good("Function '<{color.emphasis}>" + TagParser.Escape(name) + "<{color.base}>' already exists!");
                }
                else
                {
                    entry.Bad("Function '<{color.emphasis}>" + TagParser.Escape(name) + "<{color.base}>' already exists!");
                }
            }
            else
            {
                entry.Good("Function '<{color.emphasis}>" + TagParser.Escape(name) + "<{color.base}>' defined.");
                entry.Queue.CommandSystem.Functions.Add(name, new CommandScript(name, CommandScript.DisOwn(entry.Block, entry)));
            }
        }
    }
}
