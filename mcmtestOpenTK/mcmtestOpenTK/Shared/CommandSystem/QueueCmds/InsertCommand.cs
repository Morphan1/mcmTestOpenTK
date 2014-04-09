using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.CommandHandlers;

namespace mcmtestOpenTK.Shared.CommandSystem.QueueCmds
{
    class InsertCommand: AbstractCommand
    {
        public InsertCommand()
        {
            Name = "insert";
            Arguments = "<script to insert>";
            Description = "Inserts a script file to the current command queue.";
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
                    entry.Output.WriteLine(TextStyle.Color_Outgood + "Inserting '" + TextStyle.Color_Separate + fname + TextStyle.Color_Outgood + "'...");
                    entry.Queue.AddCommandsNow(CommandQueue.SeparateCommands(text));
                }
                else
                {
                    entry.Output.WriteLine(TextStyle.Color_Outbad + "Cannot insert script '" + TextStyle.Color_Separate + fname + TextStyle.Color_Outbad + "': file does not exist!");
                }
            }
        }
    }
}
