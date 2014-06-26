using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.Networking.OneOffs;
using mcmtestOpenTK.Shared.CommandSystem;

namespace mcmtestOpenTK.Client.CommandHandlers.NetworkCmds
{
    class PingCommand : AbstractCommand
    {
        public PingCommand()
        {
            Name = "ping";
            Arguments = "<server> [Port]";
            Description = "Requests basic data from a server.";
            IsDebug = true;
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 1)
            {
                ShowUsage(entry);
            }
            else
            {
                string address = entry.GetArgument(0);
                int port = entry.Arguments.Count > 1 ? Utilities.StringToInt(entry.GetArgument(1)): 26805;
                entry.Good("<{color.info}>Pinging server...");
                ServerPingRequest.SendPing(true, address, port);
            }
        }
    }
}
