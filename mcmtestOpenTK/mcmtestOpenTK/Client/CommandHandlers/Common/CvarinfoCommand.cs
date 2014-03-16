using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.CommonHandlers;

namespace mcmtestOpenTK.Client.CommandHandlers.Common
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

        public override void Execute(CommandInfo info)
        {
            if (info.Arguments.Count < 1)
            {
                UIConsole.WriteLine(TextStyle.Color_Outgood + "Listing " + TextStyle.Color_Separate + CVar.CVars.Count +
                    TextStyle.Color_Outgood + " CVars...");
                for (int i = 0; i < CVar.CVars.Count; i++)
                {
                    CVar cvar = CVar.CVars[i];
                    UIConsole.WriteLine(TextStyle.Color_Separate + (i + 1).ToString() + TextStyle.Color_Simple + ") " +
                        TextStyle.Color_Separate + cvar.Info());
                }
            }
            else
            {
                string target = info.Arguments[0];
                CVar cvar = CVar.Get(target);
                if (cvar == null)
                {
                    UIConsole.WriteLine(TextStyle.Color_Outbad + "Cvar '" + TextStyle.Color_Standout + target +
                        TextStyle.Color_Outbad + "' does not exist!");
                }
                else
                {
                    UIConsole.WriteLine(TextStyle.Color_Outgood + cvar.Info());
                }
            }
        }
    }
}
