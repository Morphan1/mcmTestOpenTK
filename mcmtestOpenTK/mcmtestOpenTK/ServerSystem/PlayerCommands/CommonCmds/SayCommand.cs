using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;

namespace mcmtestOpenTK.ServerSystem.PlayerCommands.CommonCmds
{
    public class SayCommand: PlayerAbstractCommand
    {
        public SayCommand()
        {
            Name = "say";
            Arguments = "<Message to say>";
            Description = "Sends a chat message for all players to see.";
        }

        public override void Execute(PlayerCommandEntry entry)
        {
            string message = TextStyle.Color_Simple + entry.player.Username + TextStyle.Color_Simple + ": " + TextStyle.Color_Chat + entry.AllArguments();
            SysConsole.Output(OutputType.INFO, "CHAT: " + message);
            for (int i = 0; i < Server.MainWorld.Players.Count; i++)
            {
                Server.MainWorld.Players[i].SendMessage(message);
            }
        }
    }
}
