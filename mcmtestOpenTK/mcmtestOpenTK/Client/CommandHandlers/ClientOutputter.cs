using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.Networking;
using mcmtestOpenTK.Client.Networking.PacketsOut;

namespace mcmtestOpenTK.Client.CommandHandlers
{
    public class ClientOutputter: Outputter
    {
        public override void WriteLine(string text)
        {
            UIConsole.WriteLine(text);
        }

        public override void Write(string text)
        {
            UIConsole.Write(text);
        }

        public override void Good(string tagged_text, DebugMode mode)
        {
            string text = ClientCommands.CommandSystem.TagSystem.ParseTags(tagged_text, TextStyle.Color_Outgood, null, mode);
            UIConsole.WriteLine(TextStyle.Color_Outgood + text);
        }

        public override void Bad(string tagged_text, DebugMode mode)
        {
            string text = ClientCommands.CommandSystem.TagSystem.ParseTags(tagged_text, TextStyle.Color_Outbad, null, mode);
            UIConsole.WriteLine(TextStyle.Color_Outbad + text);
        }

        public override void UnknownCommand(string basecommand, string[] arguments)
        {
            if (NetworkBase.IsActive)
            {
                StringBuilder argstr = new StringBuilder();
                argstr.Append(basecommand);
                for (int i = 0; i < arguments.Length; i++)
                {
                    argstr.Append("\n" + arguments[i]);
                }
                NetworkBase.Send(new CommandPacketOut(argstr.ToString()));
            }
            else
            {
                WriteLine(TextStyle.Color_Error + "Unknown command '" +
                    TextStyle.Color_Standout + basecommand + TextStyle.Color_Error + "'.");
            }
        }
    }
}
