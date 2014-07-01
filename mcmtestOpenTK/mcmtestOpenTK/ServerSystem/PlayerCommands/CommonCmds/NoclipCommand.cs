using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut;

namespace mcmtestOpenTK.ServerSystem.PlayerCommands.CommonCmds
{
    public class NoclipCommand: PlayerAbstractCommand
    {
        public NoclipCommand()
        {
            Name = "noclip";
            Arguments = "(on/off/toggle)";
            Description = "Toggles noclip mode.";
        }

        public override void Execute(PlayerCommandEntry entry)
        {
            // TODO: Parse on/off/toggle arg
            entry.player.Noclip = !entry.player.Noclip;
            entry.player.Send(new SetcvarPacketOut("g_noclip", entry.player.Noclip.ToString().ToLower()));
            // TODO: output on/off
            entry.player.SendMessage("Noclip toggled.");
            entry.player.UpdateStatus();
        }
    }
}
