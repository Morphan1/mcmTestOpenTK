using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.CommandHandlers;

namespace mcmtestOpenTK.Shared.CommandSystem.QueueCmds
{
    class RunCommand: AbstractCommand
    {
        public RunCommand()
        {
            Name = "run";
            Arguments = "<script to run>";
            Description = "Runs a script file.";
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 1)
            {
                ShowUsage(entry);
            }
            else
            {
                string fname = (entry.Output is ServerOutputter ? "serverscripts/" : "scripts/") + entry.GetArgument(0) + ".cfg";
                if (FileHandler.Exists(fname))
                {
                    string text = FileHandler.ReadText(fname);
                    entry.Output.WriteLine(TextStyle.Color_Outgood + "Running '" + TextStyle.Color_Separate + fname + TextStyle.Color_Outgood + "'...");
                    entry.Queue.CommandSystem.ExecuteCommands(text);
                }
                else
                {
                    entry.Output.WriteLine(TextStyle.Color_Outbad + "Cannot run script '" + TextStyle.Color_Separate + fname + TextStyle.Color_Outbad + "': file does not exist!");
                }
            }
        }
    }
}
