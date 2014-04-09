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
    class ReloadCommand: AbstractCommand
    {
        public ReloadCommand()
        {
            Name = "reload";
            Arguments = "";
            Description = "Reloads data from disk and applies delayed CVar changes.";
        }

        public override void Execute(CommandEntry entry)
        {
            UIConsole.WriteLine(TextStyle.Color_Separate + "Reloading...");
            MainGame.ReloadGraphics();
            UIConsole.WriteLine(TextStyle.Color_Outgood + "Reloaded successfully!");
        }
    }
}
