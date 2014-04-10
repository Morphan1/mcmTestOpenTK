using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.CommandHandlers;
using mcmtestOpenTK.Shared.TagHandlers;

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
                    entry.Good("Running '<{color.emphasis}>" + TagParser.Escape(fname) + "<{color.base}>'...");
                    entry.Queue.CommandSystem.ExecuteCommands(text);
                }
                else
                {
                    entry.Bad("Cannot run script '<{color.emphasis}>" + TagParser.Escape(fname) + "<{color.base}>': file does not exist!");
                }
            }
        }
    }
}
