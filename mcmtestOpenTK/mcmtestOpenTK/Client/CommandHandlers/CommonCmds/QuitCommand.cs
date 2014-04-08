using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.GlobalHandler;

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

        public override void Execute(CommandInfo info)
        {
            MainGame.Exit();
        }
    }
}
