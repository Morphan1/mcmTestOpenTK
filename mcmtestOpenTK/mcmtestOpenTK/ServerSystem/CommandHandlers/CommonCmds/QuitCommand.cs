﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.CommandSystem;

namespace mcmtestOpenTK.ServerSystem.CommandHandlers.CommonCmds
{
    class QuitCommand: AbstractCommand
    {
        public QuitCommand()
        {
            Name = "quit";
            Arguments = "";
            Description = "Immediately closes the server.";
        }

        public override void Execute(CommandEntry entry)
        {
            SysConsole.Output(OutputType.SERVERINFO, TextStyle.Color_Outgood + "Server shutting down...");
            Program.CurrentProcess.Kill();
        }
    }
}
