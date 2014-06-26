using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.CommandHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.Networking.PacketsOut;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.Networking.OneOffs;
using mcmtestOpenTK.Shared.CommandSystem;

namespace mcmtestOpenTK.Client.Networking.PacketsIn
{
    class HelloPacketIn: AbstractPacketIn
    {
        bool ServerOnline;
        public override void FromBytes(byte[] input)
        {
            if (input.Length == 6 && input[0] == (byte)'H' &&
                input[1] == (byte)'E' && input[2] == (byte)'L' && input[3] == (byte)'L' && input[4] == (byte)'O')
            {
                ServerOnline = input[5] == 1;
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }
        }

        public override void Execute()
        {
            if (!IsValid)
            {
                return;
            }
            ClientCommands.Output.Bad("Server sent HELLO! Server is " + (ServerOnline ? "ONLINE" : "OFFLINE"), DebugMode.MINIMAL);
            NetworkBase.WaitingToIdentify = true;
            if (MainGame.Username == "")
            {
                ClientCommands.Output.Bad("The server requires you identify yourself...", DebugMode.MINIMAL);
            }
            else if (ServerOnline && MainGame.Session == "")
            {
                if (MainGame.Password == "")
                {
                    ClientCommands.Output.Bad("The server requires you identify yourself...", DebugMode.MINIMAL);
                }
                else
                {
                    ClientCommands.Output.Bad("Logging in to global server...", DebugMode.MINIMAL);
                    GlobalLoginRequest.RequestLogin(false, MainGame.Username, MainGame.Password);
                }
            }
            else
            {
                ClientCommands.Output.Bad("Sending identity to server.", DebugMode.MINIMAL);
                NetworkBase.Identify();
            }
        }
    }
}
