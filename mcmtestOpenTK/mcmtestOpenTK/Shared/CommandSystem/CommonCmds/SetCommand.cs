using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.Shared.CommandSystem.CommonCmds
{
    class SetCommand: AbstractCommand
    {
        public SetCommand()
        {
            Name = "set";
            Arguments = "<CVar to set> <new value> (force)";
            Description = "Modifies the value of a specified CVar, or creates a new one.";
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 2)
            {
                ShowUsage(entry);
            }
            else
            {
                string target = entry.GetArgument(0);
                string newvalue = entry.GetArgument(1);
                bool force = (entry.Arguments.Count > 2 && entry.GetArgument(2) == "force");
                CVar cvar = entry.Output.CVarSys.AbsoluteSet(target, newvalue);
                if (cvar.Flags.HasFlag(CVarFlag.ServerControl))
                {
                    if (!force)
                    {
                        entry.Bad("CVar '<{color.emphasis}>" + TagParser.Escape(cvar.Name)
                            + "<{color.base}>' cannot be modified, it is server controlled!");
                        return;
                    }
                }
                if (cvar.Flags.HasFlag(CVarFlag.ReadOnly))
                {
                    entry.Bad("CVar '<{color.emphasis}>" + TagParser.Escape(cvar.Name)
                        + "<{color.base}>' cannot be modified, it is a read-only system variable!");
                }
                else if (cvar.Flags.HasFlag(CVarFlag.InitOnly) && !entry.Output.Initializing)
                {
                    entry.Bad("CVar '<{color.emphasis}>" + TagParser.Escape(cvar.Name)
                        + "<{color.base}>' cannot be modified after game initialization.");
                }
                else if (cvar.Flags.HasFlag(CVarFlag.Delayed) && !entry.Output.Initializing)
                {
                    entry.Good("<{color.info}>CVar '<{color.emphasis}>" + TagParser.Escape(cvar.Name) +
                        "<{color.info}>' is delayed, and its value will be calculated after the game is reloaded.");
                }
                else
                {
                    entry.Good("<{color.info}>CVar '<{color.emphasis}>" + TagParser.Escape(cvar.Name) +
                        "<{color.info}>' set to '<{color.emphasis}>" + TagParser.Escape(cvar.Value) + "<{color.info}>'.");
                }
            }
        }
    }
}
