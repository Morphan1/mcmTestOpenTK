﻿using System;
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
            Arguments = "(all)";
            Description = "Stops the current command queue.";
            IsFlow = true;
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count > 0 && entry.GetArgument(0).ToLower() == "all")
            {
                int qCount = entry.Queue.CommandSystem.Queues.Count;
                if (!entry.Queue.CommandSystem.Queues.Contains(entry.Queue))
                {
                    qCount++;
                }
                entry.Good("Stopping <{color.emphasis}>" + qCount + "<{color.base}> queue" + (qCount == 1 ? "." : "s."));
                foreach (CommandQueue queue in entry.Queue.CommandSystem.Queues)
                {
                    queue.Stop();
                }
                entry.Queue.Stop();
            }
            else
            {
                entry.Good("Stopping current queue.");
                entry.Queue.Stop();
            }
        }
    }
}
