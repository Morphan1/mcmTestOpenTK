using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Shared.CommandSystem.QueueCmds
{
    class WaitCommand: AbstractCommand
    {
        public WaitCommand()
        {
            Name = "wait";
            Arguments = "<time to wait in seconds>";
            Description = "Delays the current command queue a specified amount of time.";
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 1)
            {
                ShowUsage(entry);
            }
            else
            {
                string delay = entry.GetArgument(0);
                float seconds = Utilities.StringToFloat(delay);
                if (entry.Queue.Delayable)
                {
                    entry.Output.Good("Delaying for <{color.emphasis}>" + delay + "<{color.base}> seconds.");
                    entry.Queue.Wait = seconds;
                }
                else
                {
                    entry.Output.Bad("Cannot delay, inside an instant queue!");
                }
            }
        }
    }
}
