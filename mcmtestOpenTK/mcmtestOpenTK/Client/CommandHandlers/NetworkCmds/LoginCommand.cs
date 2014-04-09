﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.Networking.Global;
using mcmtestOpenTK.Shared.CommandSystem;

namespace mcmtestOpenTK.Client.CommandHandlers.CommonCmds
{
    class LoginCommand: AbstractCommand
    {
        public LoginCommand()
        {
            Name = "login";
            Arguments = "<username> <password>";
            Description = "Logs in to the global server.";
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 2)
            {
                ShowUsage(entry);
            }
            else
            {
                string username = entry.Arguments[0];
                string password = entry.Arguments[1];
                GlobalLoginRequest.RequestLogin(true, username, password);
                UIConsole.WriteLine(TextStyle.Color_Outgood + "Trying to log in...");
            }
        }
    }
}
