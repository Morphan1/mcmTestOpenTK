using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.Networking.Global;

namespace mcmtestOpenTK.Client.CommandHandlers.Common
{
    class LoginCommand: AbstractCommand
    {
        public LoginCommand()
        {
            Name = "login";
            Arguments = "<username> <password>";
            Description = "Logs in to the global server.";
        }

        public override void Execute(CommandInfo info)
        {
            if (info.Arguments.Count < 2)
            {
                ShowUsage(info);
            }
            else
            {
                string username = info.Arguments[0];
                string password = info.Arguments[1];
                GlobalLoginRequest.RequestLogin(true, username, password);
                UIConsole.WriteLine(TextStyle.Color_Outgood + "Trying to log in...");
            }
        }
    }
}
