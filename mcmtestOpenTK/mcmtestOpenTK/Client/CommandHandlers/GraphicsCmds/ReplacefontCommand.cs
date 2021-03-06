﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers.Text;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

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

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 4)
            {
                ShowUsage(entry);
            }
            else
            {
                string oldfont = entry.GetArgument(0);
                int oldfontsize = Utilities.StringToInt(entry.GetArgument(1));
                string newfont = entry.GetArgument(2);
                int newfontsize = Utilities.StringToInt(entry.GetArgument(3));
                if (oldfontsize < 1 || oldfontsize > 100 || newfontsize < 1 || newfontsize > 100)
                {
                    entry.Bad("Font sizes must be between 1 and 100!");
                    return;
                }
                FontSet set = FontSet.GetFont(oldfont, oldfontsize);
                set.Load(newfont, newfontsize);
                entry.Good("Successfully replaced font!"); // TODO: font names and such in output?
            }
        }
    }
}
