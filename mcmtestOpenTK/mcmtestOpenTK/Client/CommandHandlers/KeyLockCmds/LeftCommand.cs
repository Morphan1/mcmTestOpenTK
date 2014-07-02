using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Client.GameplayHandlers;

namespace mcmtestOpenTK.Client.CommandHandlers.KeyLockCmds
{
    class LeftCommand: AbstractCommand
    {
        public LeftCommand()
        {
            Name = "left";
            Arguments = "";
            Description = "Moves left.";
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
                Player.player.rleft = true;
            }
            else if (entry.Marker == 2)
            {
                Player.player.rleft = false;
            }
            else if (entry.Marker == 3)
            {
                Player.player.rleft = !Player.player.rleft;
            }
        }
    }
}
