using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;

namespace mcmtestOpenTK.Client.CommandHandlers.QueueCmds
{
    class WaitCommand: AbstractCommand
    {
        public WaitCommand()
        {
            Name = "wait";
            Arguments = "<time to wait in seconds>";
            Description = "Delays the current command queue a specified amount of time.";
        }

        public override void Execute(CommandInfo info)
        {
            if (info.Arguments.Count < 1)
            {
                ShowUsage(info);
            }
            else
            {
                string delay = info.GetArgument(0);
                float seconds = Utilities.StringToFloat(delay);
                if (info.Queue.Delayable)
                {
                    UIConsole.WriteLine(TextStyle.Color_Outgood + "Delaying for " + TextStyle.Color_Separate + delay + TextStyle.Color_Outgood + " seconds.");
                    info.Queue.Wait = seconds;
                }
                else
                {
                    UIConsole.WriteLine(TextStyle.Color_Outbad + "Cannot delay, inside an instant queue!");
                }
            }
        }
    }
}
