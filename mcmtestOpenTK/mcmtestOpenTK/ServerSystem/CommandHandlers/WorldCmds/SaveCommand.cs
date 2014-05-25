using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Shared.TagHandlers;
using mcmtestOpenTK.ServerSystem.GameHandlers;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;

namespace mcmtestOpenTK.ServerSystem.CommandHandlers.CommonCmds
{
    class SaveCommand: AbstractCommand
    {
        public SaveCommand()
        {
            Name = "save";
            Arguments = "<map name>";
            Description = "Saves the server world to file.";
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 1)
            {
                ShowUsage(entry);
            }
            else
            {
                string mapname = FileHandler.CleanFileName(entry.GetArgument(0));
                entry.Good("Saving map '<{color.emphasis}>" + TagParser.Escape(mapname) + "<{color.base}>'...");
                MapLoader.SaveMap(Server.MainWorld, mapname);
                entry.Good("Saved map '<{color.emphasis}>" + TagParser.Escape(mapname) + "<{color.base}>' successfully!");
            }
        }
    }
}
