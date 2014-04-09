using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Shared.CommandSystem.QueueCmds
{
    class StopallCommand: AbstractCommand
    {
        public StopallCommand()
        {
            Name = "stopall";
            Arguments = "";
            Description = "Stops all currently running command queues.";
        }

        public override void Execute(CommandEntry entry)
        {
            int qCount = entry.Queue.CommandSystem.Queues.Count;
            entry.Output.Good("Stopping <{color.emphasis}>" + qCount + "<{color.base}> queue" + (qCount == 1 ? "." : "s."));
            foreach (CommandQueue queue in entry.Queue.CommandSystem.Queues)
            {
                queue.Stop();
            }
            entry.Queue.Stop();
        }
    }
}
