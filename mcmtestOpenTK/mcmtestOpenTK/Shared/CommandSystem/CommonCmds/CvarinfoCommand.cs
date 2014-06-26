using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.Shared.CommandSystem.CommonCmds
{
    class CvarinfoCommand: AbstractCommand
    {
        public CvarinfoCommand()
        {
            Name = "cvarinfo";
            Arguments = "[CVar to get info on]";
            Description = "Shows information on a specified CVar, or all of them if one isn't specified.";
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 1)
            {
                entry.Info("Listing <{color.emphasis}>" + entry.Output.CVarSys.CVars.Count + "<{color.base}> CVars...");
                for (int i = 0; i < entry.Output.CVarSys.CVars.Count; i++)
                {
                    CVar cvar = entry.Output.CVarSys.CVars[i];
                    entry.Info("<{color.emphasis}>" + (i + 1).ToString() + "<{color.simple}>)<{color.emphasis}> " + TagParser.Escape(cvar.Info()));
                }
            }
            else
            {
                string target = entry.GetArgument(0).ToLower();
                List<CVar> cvars = new List<CVar>();
                for (int i = 0; i < entry.Output.CVarSys.CVars.Count; i++)
                {
                    if (entry.Output.CVarSys.CVars[i].Name.StartsWith(target))
                    {
                        cvars.Add(entry.Output.CVarSys.CVars[i]);
                    }
                }
                if (cvars.Count == 0)
                {
                    entry.Bad("CVar '<{color.emphasis}>" + TagParser.Escape(target) + "<{color.base}>' does not exist!");
                }
                else
                {
                    entry.Info("Listing <{color.emphasis}>" + cvars.Count + "<{color.base}> CVars...");
                    for (int i = 0; i < cvars.Count; i++)
                    {
                        CVar cvar = cvars[i];
                        entry.Info("<{color.emphasis}>" + (i + 1).ToString() + "<{color.simple}>)<{color.emphasis}> " + TagParser.Escape(cvar.Info()));
                    }
                }
            }
        }
    }
}
