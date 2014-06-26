using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut;

namespace mcmtestOpenTK.ServerSystem.CommandHandlers.CommonCmds
{
    class SayCommand: AbstractCommand
    {
        public SayCommand()
        {
            Name = "say";
            Arguments = "<message>";
            Description = "Says something for all players to see.";
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 1)
            {
                ShowUsage(entry);
            }
            else
            {
                string message = entry.AllArguments();
                Server.MainWorld.SendToAllPlayers(new MessagePacketOut("^r^d^7[^3Server^7]: ^2" + message));
                Server.MainWorld.SendToAllPlayers(new PlaysoundPacketOut("common/chat"));
            }
        }
    }
}
