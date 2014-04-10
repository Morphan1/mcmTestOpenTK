using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.TagHandlers;
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
                    entry.Good("Inserting '<{color.emphasis}>" + TagParser.Escape(fname) + "<{color.base}>'...");
                    entry.Queue.AddCommandsNow(CommandQueue.SeparateCommands(text));
                }
                else
                {
                    entry.Bad("Cannot insert script '<{color.emphasis}>" + TagParser.Escape(fname) + "<{color.base}>': file does not exist!");
                }
            }
        }
    }
}
