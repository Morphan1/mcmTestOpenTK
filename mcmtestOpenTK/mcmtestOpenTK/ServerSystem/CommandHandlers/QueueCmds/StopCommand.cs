using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.ServerSystem.CommandHandlers.QueueCmds
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
            // TODO: Reformat output
            SysConsole.Output(OutputType.SERVERINFO, TextStyle.Color_Outgood + "Stopping current queue.");
            info.Queue.Stop();
        }
    }
}
