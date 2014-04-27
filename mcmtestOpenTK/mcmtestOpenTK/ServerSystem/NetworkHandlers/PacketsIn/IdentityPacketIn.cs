using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut;
using mcmtestOpenTK.ServerSystem.GameHandlers;
using mcmtestOpenTK.ServerSystem.CommonHandlers;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.Global;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsIn
{
    class IdentityPacketIn: AbstractPacketIn
    {
        string Username = "";
        string Session = "";

        public override void FromBytes(Player player, byte[] input)
        {
            if (player.IsIdentified)
            {
                IsValid = false;
                return;
            }
            string[] Inputtext = FileHandler.encoding.GetString(input).Split('\n');
            if (Inputtext.Length == 2)
            {
                Username = Inputtext[0];
                Session = Inputtext[1];
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
            SysConsole.Output(OutputType.INFO, "Client trying to identify as " + Username);
            player.Username = Username;
            player.Session = Session;
            if (Util.IsAcceptableName(Username))
            {
                if (ServerCVar.g_online.ValueB)
                {
                    GlobalSessionRequest.RequestSession(player, Username, Session);
                }
                else
                {
                    player.Identified();
                }
            }
            else
            {
                player.Kick("Invalid identity.");
                SysConsole.Output(OutputType.INFO, "Client sent invalid IDENTITY packet.");
            }
        }
    }
}
