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
            IsFlow = true;
        }

        public override void Execute(CommandEntry entry)
        {
            entry.Good("Stopping current queue.");
            entry.Queue.Stop();
        }
    }
}
