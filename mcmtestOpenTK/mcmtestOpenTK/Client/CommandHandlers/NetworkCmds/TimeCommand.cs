using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.Networking.Global;

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

        public override void Execute(CommandInfo info)
        {
            GlobalTimeRequest.RequestTime(true);
            UIConsole.WriteLine(TextStyle.Color_Outgood + "Requesting Global time...");
        }
    }
}
