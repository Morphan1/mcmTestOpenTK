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
    class DisconnectCommand : AbstractCommand
    {
        public DisconnectCommand()
        {
            Name = "disconnect";
            Arguments = "";
            Description = "Disconnects from a server.";
        }

        public override void Execute(CommandEntry entry)
        {
                if (NetworkBase.IsActive || NetworkBase.Connected)
                {
                    entry.Good("<{color.info}>Disconnecting from server...");
                    NetworkBase.Disconnect("/disconnect command");
                }
                else
                {
                    entry.Bad("Cannot disconnect: not connected!");
                }
        }
    }
}
