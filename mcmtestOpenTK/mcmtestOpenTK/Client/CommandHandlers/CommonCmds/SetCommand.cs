using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.Client.CommandHandlers.CommonCmds
{
    class SetCommand: AbstractCommand
    {
        public SetCommand()
        {
            Name = "set";
            Arguments = "<CVar to set> <new value>";
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
                CVar cvar = CVar.AbsoluteSet(target, newvalue);
                if (cvar.Flags.HasFlag(CVarFlag.ReadOnly))
                {
                    entry.Bad("CVar '<{color.emphasis}>" + TagParser.Escape(target) + "<{color.base}>' cannot be modified, it is a read-only system variable!");
                }
                else if (cvar.Flags.HasFlag(CVarFlag.Delayed) && !MainGame.Initializing)
                {
                    entry.Good("<{color.info}>CVar '<{color.emphasis}>" + TagParser.Escape(target) +
                        "<{color.info}>' is delayed, and its value will be calculated after the game is reloaded.");
                }
            }
        }
    }
}
