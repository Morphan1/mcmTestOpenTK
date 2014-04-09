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
    class QuitCommand: AbstractCommand
    {
        public QuitCommand()
        {
            Name = "quit";
            Arguments = "";
            Description = "Closes the game immediately.";
        }

        public override void Execute(CommandEntry entry)
        {
            MainGame.Exit();
        }
    }
}
