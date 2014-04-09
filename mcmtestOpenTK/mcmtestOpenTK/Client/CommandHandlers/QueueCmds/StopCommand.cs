using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;

namespace mcmtestOpenTK.Client.CommandHandlers.QueueCmds
{
    class StopCommand: AbstractCommand
    {
        public StopCommand()
        {
            Name = "stop";
            Arguments = "";
            Description = "Stops the current command queue.";
        }

        public override void Execute(CommandInfo info)
        {
            UIConsole.WriteLine(TextStyle.Color_Outgood + "Stopping current queue.");
            info.Queue.Stop();
        }
    }
}
