using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Shared.TagHandlers;
using mcmtestOpenTK.ServerSystem.GameHandlers;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

namespace mcmtestOpenTK.ServerSystem.CommandHandlers.CommonCmds
{
    class LoadCommand: AbstractCommand
    {
        public LoadCommand()
        {
            Name = "load";
            Arguments = "<map name>";
            Description = "Clears the server world and loads a map.";
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
                entry.Good("Loading map '<{color.emphasis}>" + TagParser.Escape(mapname) + "<{color.base}>'...");
                bool worked = MapLoader.LoadMap(Server.MainWorld, mapname);
                if (worked)
                {
                    entry.Good("Loaded map '<{color.emphasis}>" + TagParser.Escape(mapname) + "<{color.base}>' successfully!");
                }
                else
                {
                    entry.Bad("Failed to load map '<{color.emphasis}>" + TagParser.Escape(mapname) + "<{color.base}>': unknown or invalid map name!");
                }
            }
        }
    }
}
