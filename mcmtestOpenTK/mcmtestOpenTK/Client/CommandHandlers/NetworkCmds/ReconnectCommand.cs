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
    class ReconnectCommand : AbstractCommand
    {
        public ReconnectCommand()
        {
            Name = "reconnect";
            Arguments = "";
            Description = "Reconnects to the server.";
        }

        public override void Execute(CommandEntry entry)
        {
                if (NetworkBase.IsActive || NetworkBase.Connected)
                {
                    entry.Good("<{color.info}>Disconnecting from server...");
                    string address = NetworkBase.Address;
                    // int port = NetworkBase.RemoteTarget.Port;
                    NetworkBase.Disconnect("/reconnect command");
                    entry.Good("<{color.info}>Reconnecting to server...");
                    NetworkBase.Connect(address);
                }
                else
                {
                    entry.Bad("Cannot reconnect: not connected!");
                }
        }
    }
}
