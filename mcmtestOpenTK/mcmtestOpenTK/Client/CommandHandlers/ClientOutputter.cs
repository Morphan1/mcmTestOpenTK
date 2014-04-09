using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Client.UIHandlers;

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
    }
}
