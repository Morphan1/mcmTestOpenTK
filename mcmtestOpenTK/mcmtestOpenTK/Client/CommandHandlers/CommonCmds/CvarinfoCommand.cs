using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.Client.CommandHandlers.CommonCmds
{
    class CvarinfoCommand: AbstractCommand
    {
        public CvarinfoCommand()
        {
            Name = "cvarinfo";
            Arguments = "[CVar to get info on]";
            Description = "Shows information on a specified CVar, or all of them if one isn't specified.";
            IsDebug = true;
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 1)
            {
                entry.Output.Good("Listing <{color.emphasis}>" + CVar.CVars.Count + "<{color.base}> CVars...");
                for (int i = 0; i < CVar.CVars.Count; i++)
                {
                    CVar cvar = CVar.CVars[i];
                    entry.Output.Good("<{color.emphasis}>" + (i + 1).ToString() + "<{color.simple}>)<{color.emphasis}> " + TagParser.Escape(cvar.Info()));
                }
            }
            else
            {
                string target = entry.GetArgument(0);
                CVar cvar = CVar.Get(target);
                if (cvar == null)
                {
                    entry.Bad("CVar '<{color.emphasis}>" + TagParser.Escape(target) + "<{color.base}>' does not exist!");
                }
                else
                {
                    entry.Output.Good("<{color.emphasis}>" + TagParser.Escape(cvar.Info()));
                }
            }
        }
    }
}
