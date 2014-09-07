using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Client.GameplayHandlers.Entities;

namespace mcmtestOpenTK.Client.CommandHandlers.KeyLockCmds
{
    class RightCommand: AbstractCommand
    {
        public RightCommand()
        {
            Name = "right";
            Arguments = "";
            Description = "Moves right.";
            IsDebug = true;
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Marker == 0)
            {
                entry.Bad("Must use +, -, or !");
            }
            else if (entry.Marker == 1)
            {
                Player.player.rright = true;
            }
            else if (entry.Marker == 2)
            {
                Player.player.rright = false;
            }
            else if (entry.Marker == 3)
            {
                Player.player.rright = !Player.player.rright;
            }
        }
    }
}
