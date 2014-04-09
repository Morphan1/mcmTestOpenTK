using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Shared.CommandSystem.QueueCmds
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
            info.Output.WriteLine(TextStyle.Color_Outgood + "Stopping current queue.");
            info.Queue.Stop();
        }
    }
}
