using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.CommonHandlers;

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

        public override void Good(string tagged_text)
        {
            string text = ClientCommands.CommandSystem.TagSystem.ParseTags(tagged_text, TextStyle.Color_Outgood, null);
            UIConsole.WriteLine(TextStyle.Color_Outgood + text);
        }

        public override void Bad(string tagged_text)
        {
            string text = ClientCommands.CommandSystem.TagSystem.ParseTags(tagged_text, TextStyle.Color_Outbad, null);
            UIConsole.WriteLine(TextStyle.Color_Outbad + text);
        }

        public ClientOutputter()
        {
            CVarSys = ClientCVar.system;
        }
    }
}
