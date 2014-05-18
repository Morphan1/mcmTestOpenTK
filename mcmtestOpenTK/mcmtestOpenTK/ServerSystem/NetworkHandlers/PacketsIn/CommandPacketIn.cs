using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.ServerSystem.CommonHandlers;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.Global;
using mcmtestOpenTK.ServerSystem.PlayerCommands;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsIn
{
    class CommandPacketIn: AbstractPacketIn
    {
        string[] Arguments;

        public override void FromBytes(Player player, byte[] input)
        {
            if (input.Length == 0)
            {
                IsValid = false;
                return;
            }
            Arguments = FileHandler.encoding.GetString(input).Split('\n');
            IsValid = true;
        }

        public override void Execute(Player player)
        {
            if (!IsValid)
            {
                return;
            }
            PlayerCommandEngine.RunCommand(player, Arguments.ToList());
        }
    }
}
