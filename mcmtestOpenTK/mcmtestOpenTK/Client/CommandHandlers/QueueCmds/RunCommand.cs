using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;

namespace mcmtestOpenTK.Client.CommandHandlers.QueueCmds
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
                string fname = "scripts/" + info.GetArgument(0) + ".cfg";
                if (FileHandler.Exists(fname))
                {
                    string text = FileHandler.ReadText(fname);
                    UIConsole.WriteLine(TextStyle.Color_Outgood + "Running '" + TextStyle.Color_Separate + fname + TextStyle.Color_Outgood + "'...");
                    Commands.ExecuteCommands(text);
                }
                else
                {
                    UIConsole.WriteLine(TextStyle.Color_Outbad + "Cannot run script '" + TextStyle.Color_Separate + fname + TextStyle.Color_Outbad + "': file does not exist!");
                }
            }
        }
    }
}
