using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.Shared.CommandSystem.QueueCmds
{
    // <--[command]
    // @Name scriptcache
    // @Arguments clear scripts/functions/all
    // @Short Modifies the state of the script cache, EG clearing it.
    // @Updated 2014/06/23
    // @Authors mcmonkey
    // @Description
    // The ScriptCache command can be used to clear all standard scripts in the script cache,
    // or of all functions in the script cache.
    // Specify "all" to affect all cache types.
    // TODO: Explain more!
    // TODO: ScriptCache info tags!
    // @Example
    // // This example clears the script cache of all standard scripts
    // scriptcache clear scripts
    // @Example
    // TODO: More examples!
    // -->
    class ScriptCacheCommand : AbstractCommand
    {
        public ScriptCacheCommand()
        {
            Name = "scriptcache";
            Arguments = "clear scripts/functions/all";
            Description = "Modifies the state of the script cache, EG clearing it.";
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 1)
            {
                ShowUsage(entry);
                return;
            }
            string type = entry.GetArgument(0).ToLower();
            if (type == "clear")
            {
                if (entry.Arguments.Count < 2)
                {
                    ShowUsage(entry);
                    return;
                }
                string target = entry.GetArgument(1).ToLower();
                if (target == "scripts")
                {
                    int count = entry.Queue.CommandSystem.Scripts.Count;
                    entry.Queue.CommandSystem.Scripts.Clear();
                    entry.Good("Script cache cleared of <{color.emphasis}>" +
                        count + "<{color.base}> script" + (count == 1 ? ".": "s."));
                }
                else if (target == "functions")
                {
                    int count = entry.Queue.CommandSystem.Functions.Count;
                    entry.Queue.CommandSystem.Functions.Clear();
                    entry.Good("Script cache cleared of <{color.emphasis}>" +
                        count + "<{color.base}> function" + (count == 1 ? "." : "s."));
                }
                else if (target == "all")
                {
                    int countScripts = entry.Queue.CommandSystem.Scripts.Count;
                    int countFunctions = entry.Queue.CommandSystem.Functions.Count;
                    entry.Queue.CommandSystem.Scripts.Clear();
                    entry.Queue.CommandSystem.Functions.Clear();
                    entry.Good("Script cache cleared of <{color.emphasis}>" +
                        countScripts + "<{color.base}> script" + (countScripts == 1 ? "," : "s,")
                        + " and <{color.emphasis}>" + countFunctions + "<{color.base}> function" +
                        (countFunctions == 1 ? ".": "s."));
                }
                else
                {
                    ShowUsage(entry);
                }
            }
            else
            {
                ShowUsage(entry);
            }
        }
    }
}
