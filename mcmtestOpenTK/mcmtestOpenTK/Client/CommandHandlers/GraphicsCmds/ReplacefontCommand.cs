using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers.Text;
using mcmtestOpenTK.Shared.CommandSystem;

namespace mcmtestOpenTK.Client.CommandHandlers.GraphicsCmds
{
    public class ReplacefontCommand : AbstractCommand
    {
        public ReplacefontCommand()
        {
            Name = "replacefont";
            Arguments = "<font to replace> <size of font to replace> <new font> <new font size>";
            Description = "Replaces one font with another.";
            IsDebug = true;
        }

        public override void Execute(CommandInfo info)
        {
            if (info.Arguments.Count < 4)
            {
                ShowUsage(info);
            }
            else
            {
                string oldfont = info.GetArgument(0);
                int oldfontsize = Utilities.StringToInt(info.GetArgument(1));
                string newfont = info.GetArgument(2);
                int newfontsize = Utilities.StringToInt(info.GetArgument(3));
                if (oldfontsize < 1 || oldfontsize > 100 || newfontsize < 1 || newfontsize > 100)
                {
                    UIConsole.WriteLine(TextStyle.Color_Outbad + "Font sizes must be between 1 and 100!");
                    return;
                }
                FontSet set = FontSet.GetFont(oldfont, oldfontsize);
                set.Load(newfont, newfontsize);
                UIConsole.WriteLine(TextStyle.Color_Outgood + "Successfully replaced font!");
            }
        }
    }
}
