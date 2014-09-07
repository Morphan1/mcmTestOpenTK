using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Client.GameplayHandlers.Entities;

namespace mcmtestOpenTK.Client.CommandHandlers.KeyLockCmds
{
    class SlowCommand : AbstractCommand
    {
        public SlowCommand()
        {
            Name = "slow";
            Arguments = "";
            Description = "Moves downward (crouches).";
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
                Player.player.rslow = true;
            }
            else if (entry.Marker == 2)
            {
                Player.player.rslow = false;
            }
            else if (entry.Marker == 3)
            {
                Player.player.rslow = !Player.player.rslow;
            }
        }
    }
}
