using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsIn
{
    class DisconnectPacketIn: AbstractPacketIn
    {
        public override void FromBytes(Player player, byte[] input)
        {
            IsValid = true;
        }

        public override void Execute(Player player)
        {
            if (!IsValid)
            {
                return;
            }
            player.Kick("CLIENT SENT DISCONNECT");
        }
    }
}
