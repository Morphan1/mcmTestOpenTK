using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.Networking.OneOffs;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Shared.TagHandlers;

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
                string username = entry.GetArgument(0);
                string password = entry.GetArgument(1);
                GlobalLoginRequest.RequestLogin(true, username, password);
                entry.Good("<{color.info}>Trying to log in as '<{color.emphasis}>" + TagParser.Escape(username) + "<{color.base}>'...");
            }
        }
    }
}
