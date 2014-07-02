using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Client.GameplayHandlers;

namespace mcmtestOpenTK.Client.CommandHandlers.KeyLockCmds
{
    class BackCommand: AbstractCommand
    {
        public BackCommand()
        {
            Name = "back";
            Arguments = "";
            Description = "Moves backward.";
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
                Player.player.rback = true;
            }
            else if (entry.Marker == 2)
            {
                Player.player.rback = false;
            }
            else if (entry.Marker == 3)
            {
                Player.player.rback = !Player.player.rback;
            }
        }
    }
}
