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
    class TimeCommand : AbstractCommand
    {
        public TimeCommand()
        {
            Name = "time";
            Arguments = "";
            Description = "Requests the current time from the Global Server.";
            IsDebug = true;
        }

        public override void Execute(CommandEntry entry)
        {
            GlobalTimeRequest.RequestTime(true);
            entry.Good("<{color.info}>Requesting Global time...");
        }
    }
}
