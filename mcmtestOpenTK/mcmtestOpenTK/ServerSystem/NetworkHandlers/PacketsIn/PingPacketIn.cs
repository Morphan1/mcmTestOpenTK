using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsIn
{
    class PingPacketIn: AbstractPacketIn
    {
        byte ID;

        public override void FromBytes(Player player, byte[] input)
        {
            if (input.Length == 5 && input[0] == (byte)'P' &&
                input[1] == (byte)'I' && input[2] == (byte)'N' && input[3] == (byte)'G')
            {
                ID = input[4];
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }
        }

        public override void Execute(Player player)
        {
            if (!IsValid)
            {
                return;
            }
            if (ID == player.PingID)
            {
                player.Send(new PingPacketOut(player));
                player.Send(new TimePacketOut());
            }
            else
            {
                player.Kick("Invalid ping packet.");
                SysConsole.Output(OutputType.INFO, "Client sent invalid PING packet - expecting " + (int)player.PingID + ", got " + (int)ID);
            }
        }
    }
}
