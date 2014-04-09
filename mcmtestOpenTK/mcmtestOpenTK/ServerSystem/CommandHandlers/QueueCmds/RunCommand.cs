using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.ServerSystem.CommandHandlers.QueueCmds
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
                string fname = "serverscripts/" + info.GetArgument(0) + ".cfg";
                // TODO: Reformat output
                if (FileHandler.Exists(fname))
                {
                    string text = FileHandler.ReadText(fname);
                    SysConsole.Output(OutputType.SERVERINFO, TextStyle.Color_Outgood + "Running '" + TextStyle.Color_Separate + fname + TextStyle.Color_Outgood + "'...");
                    Commands.ExecuteCommands(text);
                }
                else
                {
                    SysConsole.Output(OutputType.SERVERINFO, TextStyle.Color_Outbad + "Cannot run script '" + TextStyle.Color_Separate + fname + TextStyle.Color_Outbad + "': file does not exist!");
                }
            }
        }
    }
}
