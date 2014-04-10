using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.Networking;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.Client.CommandHandlers.NetworkCmds
{
    class ConnectCommand : AbstractCommand
    {
        public ConnectCommand()
        {
            Name = "connect";
            Arguments = "<IP>";
            Description = "Connects to a server at the given IP address.";
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 1)
            {
                ShowUsage(entry);
            }
            else
            {
                string IP = entry.GetArgument(0);
                if (NetworkBase.Connect(IP))
                {
                    entry.Good("<{color.info}>Connecting to <{color.emphasis}>" + NetworkBase.RemoteTarget.ToString() + "<{color.info}>...");
                }
                else
                {
                    entry.Bad("Cannot connect to '<{color.emphasis}>" + TagParser.Escape(IP) + "<{color.base}>': invalid IP address!");
                }
            }
        }
    }
}
