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

        public override void Execute(CommandInfo info)
        {
            int qCount = info.Queue.CommandSystem.Queues.Count;
            info.Output.Good("Stopping <{color.emphasis}>" + qCount + "<{color.base}> queue" + (qCount == 1 ? ".": "s."));
            foreach (CommandQueue queue in info.Queue.CommandSystem.Queues)
            {
                queue.Stop();
            }
            info.Queue.Stop();
        }
    }
}
