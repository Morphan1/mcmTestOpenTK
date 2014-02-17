﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;

namespace mcmtestOpenTK.Client.CommandHandlers.Common
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
            if (info.Arguments.Count == 0)
            {
                ShowUsage(info);
            }
            else
            {
                UIConsole.WriteLine(TextStyle.Color_Simple + Utilities.Concat(info.Arguments));
            }
        }
    }
}