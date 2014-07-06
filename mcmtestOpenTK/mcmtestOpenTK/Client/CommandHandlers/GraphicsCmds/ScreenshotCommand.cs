using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Shared.CommandSystem;

namespace mcmtestOpenTK.Client.CommandHandlers.CommonCmds
{
    class ScreenshotCommand : AbstractCommand
    {
        public ScreenshotCommand()
        {
            Name = "screenshot";
            Arguments = "";
            Description = "Takes a screenshot and saves it to file.";
        }

        public override void Execute(CommandEntry entry)
        {
            MainGame.Screenshot = true;
            entry.Good("Taking screenshot...");
        }
    }
}
