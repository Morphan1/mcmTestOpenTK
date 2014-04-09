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

        public override void Execute(CommandInfo info)
        {
            if (info.Arguments.Count < 1)
            {
                ShowUsage(info);
            }
            else
            {
                string fname = (info.Output is ServerOutputter ? "serverscripts/": "scripts/") + info.GetArgument(0) + ".cfg";
                if (FileHandler.Exists(fname))
                {
                    string text = FileHandler.ReadText(fname);
                    info.Output.WriteLine(TextStyle.Color_Outgood + "Running '" + TextStyle.Color_Separate + fname + TextStyle.Color_Outgood + "'...");
                    info.Queue.CommandSystem.ExecuteCommands(text);
                }
                else
                {
                    info.Output.WriteLine(TextStyle.Color_Outbad + "Cannot run script '" + TextStyle.Color_Separate + fname + TextStyle.Color_Outbad + "': file does not exist!");
                }
            }
        }
    }
}
