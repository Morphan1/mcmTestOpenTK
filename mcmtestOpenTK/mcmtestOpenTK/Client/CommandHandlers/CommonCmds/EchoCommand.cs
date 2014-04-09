﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Shared.CommandSystem;

namespace mcmtestOpenTK.Client.CommandHandlers.CommonCmds
{
    class EchoCommand: AbstractCommand
    {
        public EchoCommand()
        {
            Name = "echo";
            Arguments = "<text to echo>";
            Description = "Echoes any input text back to the console.";
        }

        public override void Execute(CommandInfo info)
        {
            if (info.Arguments.Count < 1)
            {
                ShowUsage(info);
            }
            else
            {
                string args = info.AllArguments();
                SysConsole.Output(OutputType.CLIENTINFO, "Echoing " + args);
                UIConsole.WriteLine(args);
            }
        }
    }
}